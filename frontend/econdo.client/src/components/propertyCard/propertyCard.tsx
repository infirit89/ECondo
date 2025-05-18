import { deleteProperty, PropertyOccupantResult, PropertyResult } from "@/actions/property";
import {
    Card,
    Text,
    Group,
    ActionIcon,
    Avatar,
    Badge,
    Stack,
    Divider,
    AvatarGroup,
} from "@mantine/core";
import {
    IconBuildingCottage,
    IconBuildingFactory,
    IconBuildingSkyscraper,
    IconBuildingStore,
    IconBuildingWarehouse,
    IconChartPie,
    IconEdit,
    IconHome,
    IconPalette,
    IconRuler,
    IconStairs,
    IconTrash,
    IconUsers
} from "@tabler/icons-react";
import { useState } from "react";
import PropertyEditModal from "@/components/propertyEditModal";
import { queryKeys } from "@/types/queryKeys";
import { useModals } from "@mantine/modals";
import { useMutation, useQueryClient } from "@tanstack/react-query";

const getPropertyTypeInfo = (propertyType: string) => {
    switch (propertyType) {
        case "апартамент":
            return { color: "blue", icon: <IconHome size={24} />, label: 'апартамент' }
        case "гараж":
            return { color: "gray", icon: <IconBuildingWarehouse size={24} />, label: 'гараж' }
        case "офис":
            return { color: "green", icon: <IconBuildingSkyscraper size={24} />, label: 'офис' }
        case "магазин":
            return { color: "orange", icon: <IconBuildingStore size={24} />, label: 'магазин' }
        case "мазе":
            return { color: "violet", icon: <IconStairs size={24} />, label: 'мазе' }
        case "склад":
            return { color: "yellow", icon: <IconBuildingFactory size={24} />, label: 'склад' }
        case "таванско помещение":
            return { color: "cyan", icon: <IconBuildingCottage size={24} />, label: 'таванско' }
        case "ателие":
            return { color: "pink", icon: <IconPalette size={24} />, label: 'ателие' }
        default:
            return { color: "blue", icon: <IconHome size={24} />, label: 'апартамент' }
    }
}

interface ProperyCardProps {
    property: PropertyOccupantResult,
    canDelete: boolean,
    canEdit: boolean,
    onDeleteError?: () => void
}

export function PropertyCard({
    property,
    canDelete,
    canEdit,
    onDeleteError }: ProperyCardProps) {

    const propertyInfo = getPropertyTypeInfo(property.property.propertyType.toLowerCase());
    const [isDeleting, setIsDeleting] = useState(false);

    const [editModalOpened, setEditModalOpen] = useState(false);

    const modals = useModals();
    const queryClient = useQueryClient();
    const useDeleteProperty = () => {
        return useMutation({
            mutationFn: (propertyId: string) => deleteProperty(propertyId),
            onSuccess: (data) => {
                if (!data.ok) {
                    onDeleteError && onDeleteError();
                    setIsDeleting(false);
                    return;
                }

                queryClient.invalidateQueries({
                    queryKey: queryKeys.properties.all,
                });

                setIsDeleting(false);
            },
            onMutate: () => {
                setIsDeleting(true);
            }
        });
    }

    const { mutate: deletePropertyMutation } = useDeleteProperty();

    const handleDeleteProperty = (id: string) => {
        const modalId = modals.openConfirmModal({
            title: "Потвърди изтриване",
            children: <Text size="sm">Сигурни ли сте, че искате да изтриете този имот? Това действие не може да бъде отменено.</Text>,
            labels: { confirm: "Изтрий", cancel: "Отмяна" },
            confirmProps: { color: "red" },
            onConfirm: () => {
                deletePropertyMutation(id);
                modals.closeModal(modalId);
            },
        })
    }

    return (
        <>
            {
                canEdit && (
                    <PropertyEditModal
                        opened={editModalOpened}
                        onClose={() => setEditModalOpen(false)}
                        propertyId={property.property.id}
                    />
                )
            }
            <Card
                shadow="sm"
                padding="xl"
                radius="lg"
                withBorder
                style={{ transition: "transform 0.2s ease, box-shadow 0.2s ease" }}
                onMouseEnter={(e) => (e.currentTarget.style.transform = "translateY(-4px)")}
                onMouseLeave={(e) => (e.currentTarget.style.transform = "translateY(0)")}>
                <Card.Section
                    p="md"
                    style={{
                        background: `var(--mantine-color-${propertyInfo.color}-0)`,
                        borderBottom: `1px solid var(--mantine-color-${propertyInfo.color}-2)`,
                        borderRadius: "var(--mantine-radius-lg) var(--mantine-radius-lg) 0 0",
                    }}>
                    <Group justify='space-between'>
                        <Group>
                            <Avatar size="md" radius="xl" color={propertyInfo.color}>
                                {propertyInfo.icon}
                            </Avatar>
                            <div>
                                <Group gap="xs">
                                    <Text fw={700} size="xl">
                                        {property.property.number}
                                    </Text>
                                    <Badge color={propertyInfo.color} size="sm">
                                        {propertyInfo.label.charAt(0).toUpperCase() + propertyInfo.label.slice(1)}
                                    </Badge>
                                </Group>
                                <Text size="sm" c="dimmed">
                                    Етаж {property.property.number}
                                </Text>
                            </div>
                        </Group>
                        {
                            canEdit && canDelete && (
                                <Group gap={8}>
                                    <ActionIcon
                                        variant='subtle'
                                        color="blue"
                                        disabled={isDeleting}
                                        onClick={() => setEditModalOpen(true)}>
                                        <IconEdit size={18} />
                                    </ActionIcon>
                                    <ActionIcon
                                        variant='subtle'
                                        color="red"
                                        disabled={isDeleting}
                                        onClick={() => handleDeleteProperty(property.property.id)}>
                                        <IconTrash size={18} />
                                    </ActionIcon>
                                </Group>
                            )
                        }
                    </Group>
                </Card.Section>

                <Stack gap="xs" mt="md">
                    <Group gap="xs">
                        <IconRuler size={16} color="gray" />
                        <Text size="sm">
                            <Text span fw={500}>
                                Застроена площ:
                            </Text>{" "}
                            {property.property.builtArea} m²
                        </Text>
                    </Group>

                    <Group gap="xs">
                        <IconChartPie size={16} color="gray" />
                        <Text size="sm">
                            <Text span fw={500}>
                                Идеални части:
                            </Text>{" "}
                            {property.property.idealParts}
                        </Text>
                    </Group>

                    <Divider
                        my="xs"
                        label={
                            <Group gap="xs">
                                <IconUsers size={16} />
                                <Text size="sm">Контакти</Text>
                            </Group>
                        }
                        labelPosition="left"
                    />

                    {property.occupants.length > 0 ? (
                        <AvatarGroup spacing={'6'}>
                            {property.occupants.map((occupant, index) => (
                                <Avatar
                                    key={index}
                                    size="sm"
                                    radius="xl"
                                    color={'initials'}
                                    name={`${occupant.firstName} ${occupant.lastName}`}
                                >
                                </Avatar>
                            ))}
                            {
                                property.remainingOccupants > 0 && (
                                    <Avatar size={'sm'} radius={'xl'}>+{property.remainingOccupants}</Avatar>
                                )
                            }
                        </AvatarGroup>
                    ) : (
                        <Text size="sm" c="dimmed" fs={'italic'}>
                            Няма назначени контакти
                        </Text>
                    )}
                </Stack>
            </Card>
        </>
    );
}
