'use client';

import { deleteProperty, getPropertiesInEntrance } from "@/actions/property";
import Loading from "@/components/loading";
import { Center, Title, Pagination, Alert, SimpleGrid, Flex, Text } from "@mantine/core";
import { IconExclamationCircle, IconMoodPuzzled } from "@tabler/icons-react";
import { useParams } from "next/navigation";
import { useState } from "react";
import PropertyCard from "@/components/propertyCard";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { queryKeys } from "@/types/queryKeys";
import { useModals } from "@mantine/modals";

// hard coded for now
const pageSize = 9;

const useQueryPropertiesPaged = (buildingId: string, entranceNumber: string, page: number) => {
    return useQuery({
        queryKey: queryKeys.properties.pagedInEntrance(
            buildingId,
            entranceNumber,
            page,
            pageSize),
        queryFn: () => getPropertiesInEntrance(
            buildingId,
            entranceNumber,
            page,
            pageSize),
    })
}

export default function PropertiesList() {
    const { buildingId, entranceNumber } = useParams<{
        buildingId: string,
        entranceNumber: string }>();

    const decodedEntranceNumber = decodeURIComponent(entranceNumber);
    const [isDeleteError, setDeleteError] = useState(false);
    const [deletedPropertyId, setDeletedPropertyId] = useState<string | undefined>(undefined);
    const [page, setPage] = useState(0);

    const modals = useModals();
    const queryClient = useQueryClient();
    const { data: properties, isLoading } = useQueryPropertiesPaged(buildingId, decodedEntranceNumber, page);

    const useDeleteProperty = () => {
        return useMutation({
            mutationFn: (propertyId: string) => deleteProperty(propertyId),
            onSuccess: (data) => {
                if(!data.ok) {
                    setDeleteError(true);
                    setDeletedPropertyId(undefined);
                    return;
                }
                
                queryClient.invalidateQueries({
                    queryKey: queryKeys.properties.pagedInEntrance(
                        buildingId,
                        decodedEntranceNumber,
                        page,
                        pageSize),
                    });
            },
            onMutate: (propertyId) => {
                setDeleteError(false);
                setDeletedPropertyId(propertyId);
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

    if(isLoading || !properties?.ok)
        return <Loading/>;

    return (
        <> 
            { 
                isDeleteError ?
                <Alert
                variant="light"
                color="red"
                title="Грешка"
                icon={<IconExclamationCircle/>} mb={'md'}>
                    Грешка при изтриването на имот!
                </Alert>
                :
                <></>
            }
            {
                properties.value && properties.value.items.length > 0 ?
                <>
                    <Flex justify={'center'} mt={'lg'} mb={'lg'} hiddenFrom='lg'>
                        <Pagination total={properties.value.totalPages}
                        value={page + 1}
                        onChange={(value) => setPage(value - 1)}/>
                    </Flex>
                    <SimpleGrid
                    cols={{ base: 1, md: 2, lg: 3 }}
                    spacing={{ base: 'sm', md: 'md', lg: 'lg' }}>
                        {
                            properties
                            .value
                            .items.map((value, index) => (
                                <PropertyCard
                                key={index}
                                isDeleting={value.id === deletedPropertyId}
                                property={value}
                                handleDelete={handleDeleteProperty}
                                canEdit
                                />
                            ))    
                        }
                    </SimpleGrid>
                    <Flex justify={'center'} mt={'xl'}>
                        <Pagination total={properties.value.totalPages}
                        value={page + 1}
                        onChange={(value) => setPage(value - 1)}/>
                    </Flex>
                </>
                : 
                <>
                    <Center mt={90} mb={20}>
                        <IconMoodPuzzled size={100} color="#868e96"/>
                    </Center>
                    <Center>
                        <Title c={'dimmed'}>Този вход няма регистрирани имоти</Title>
                    </Center>
                </> 
            }
        </>
    );
}