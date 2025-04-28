'use client';
import { Card, CardSection, Text, Image, Center, Group, ActionIcon } from "@mantine/core";
import classes from './condoCard.module.css';
import { useHover } from "@mantine/hooks";
import { IconEdit, IconTrash } from "@tabler/icons-react";
import { useState } from "react";
import { useModals } from "@mantine/modals";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { deleteEntrance } from "@/actions/condo";
import { queryKeys } from "@/types/queryKeys";

export interface CondoCardProps {
    id: string;
    name: string;
    image?: string;
    provinceName: string;
    municipality: string;
    settlementPlace: string;
    neighborhood: string;
    postalCode: string;
    street: string;
    streetNumber: string;
    buildingNumber: string;
    entranceNumber: string;
    canEdit: boolean;
    canDelete: boolean;
}

export default function CondoCard({ 
    id,
    name,
    image,
    provinceName,
    municipality,
    settlementPlace,
    neighborhood,
    street,
    streetNumber,
    buildingNumber,
    entranceNumber,
    canEdit,
    canDelete,
}: CondoCardProps) {
    const {hovered, ref} = useHover();

    const [isDeleting, setIsDeleting] = useState(false);
    const modals = useModals();
    const queryClient = useQueryClient();

    const useDeleteEntrance = () => {
        return useMutation({
            mutationFn: () => deleteEntrance(id, entranceNumber),
            onSuccess: () => {

                queryClient.invalidateQueries({
                    queryKey: queryKeys.buildings.all,
                });
                setIsDeleting(false);
            },
            onMutate: () => {
                setIsDeleting(true);
            }
        });
    }

    const { mutate: deleteEntranceMutation } = useDeleteEntrance();

    const handleDelete = () => {
        const modalId = modals.openConfirmModal({
            title: "Потвърди изтриване",
            children: <Text size="sm">Сигурни ли сте, че искате да изтриете този вход? Това действие не може да бъде отменено.</Text>,
            labels: { confirm: "Изтрий", cancel: "Отмяна" },
            confirmProps: { color: "red" },
            onConfirm: () => {
                deleteEntranceMutation();
                modals.closeModal(modalId);
            },
        });
    }
    const handleEdit = () => {
        
    }

    return (
        <Card
        className={`${classes.condoCard}`} 
        ref={ref} 
        shadow={hovered ? 'xl' : 'sm'} 
        padding="xl"
        radius="lg"
        withBorder
        style={{ transition: "transform 0.2s ease, box-shadow 0.2s ease" }}
        onMouseEnter={(e) => (e.currentTarget.style.transform = "translateY(-4px)")}
        onMouseLeave={(e) => (e.currentTarget.style.transform = "translateY(0)")}>
            <CardSection>
                <Image
                className={`h-[180]`}
                src={image ? image : "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d4/The_Lauren_condo_Bethesda_MD_2021-12-12_10-11-55_1.jpg/1200px-The_Lauren_condo_Bethesda_MD_2021-12-12_10-11-55_1.jpg"}
                alt={name}
                />
            </CardSection>

            {/* <Group justify="space-between" mt="md" mb="xs"> */}
            <Center mt={'md'} mb={'xs'}>
                <Text fw={500}>{name} (вх. {entranceNumber})</Text>
            </Center>
                {/* <Badge color="pink">On Sale</Badge> */}
            {/* </Group> */}

            <Text size="sm" c="dimmed" ta={'center'}>
                обл. {provinceName}, общ. {municipality}, {settlementPlace}, кв. {neighborhood}, ул. {street} {streetNumber}, сг. {buildingNumber}, вх. {entranceNumber}
            </Text>

            <Group gap={8} justify='flex-end' mt={'md'}>
                {
                    canEdit && (
                    <ActionIcon
                    variant='subtle'
                    color="blue"
                    disabled={isDeleting}
                    onClick={(e) => {
                        e.stopPropagation();
                        e.preventDefault();
                        handleEdit()
                    }}>
                        <IconEdit size={18} />
                    </ActionIcon>
                    )
                }
                {
                    canDelete && (
                        <ActionIcon 
                        variant='subtle'
                        color="red"
                        disabled={isDeleting}
                        onClick={(e) => {
                            e.stopPropagation();
                            e.preventDefault();
                            handleDelete();
                        }}>
                            <IconTrash size={18} />
                        </ActionIcon>
                    )
                }
            </Group>
        </Card>
    );
}