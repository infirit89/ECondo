'use client';

import { deleteOccupant, getTenantsForProperty, Occupant, OccupantTypeNameResult } from "@/actions/propertyOccupant";
import Loading from "@/components/loading";
import OccupantForm from "@/components/occupantForm";
import OccupantPaper from "@/components/occupantPaper";
import { queryKeys } from "@/types/queryKeys";
import { Center, Flex, Pagination, SimpleGrid, Title, Text } from "@mantine/core";
import { useModals } from "@mantine/modals";
import { IconMoodPuzzled } from "@tabler/icons-react";
import { QueryClient, useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useState } from "react";

interface OccupantsListProps {
    occupantTypes: OccupantTypeNameResult,
    propertyId: string,
}

// hard coded for now
const pageSize = 10;

const useTenantsPagedQuery = (propertyId: string, page: number) => {
    return useQuery({
        queryKey: queryKeys.occupants.tenantsInPropertyPaged(
            propertyId,
            page,
            pageSize),
        queryFn: () => getTenantsForProperty(propertyId, page, pageSize),
    });
}

export default function OccupantsList({ occupantTypes, propertyId } : OccupantsListProps) {

    const [page, setPage] = useState(0);
    const { data: tenants, isLoading } = useTenantsPagedQuery(propertyId, page)
    const [occupantsInDeletion, setOccupantsInDeletion] = useState<Set<string>>(new Set<string>());

    const modals = useModals();
    const queryClient = useQueryClient();
    const useDeleteOccupantMutation = (
    propertyId: string) => {
    return useMutation({
        mutationFn: (occupantId: string) => deleteOccupant(occupantId),
        onSuccess: (data, occupantId) => {
            if(!data.ok)
                return; // TODO:
        
            queryClient.invalidateQueries({
                queryKey: queryKeys.occupants.tenantsInPropertyPaged(
                    propertyId,
                    page,
                    pageSize),
            });

            setOccupantsInDeletion(prev => {
                prev.delete(occupantId);
                return new Set(prev);
            })
        },
        onMutate: (occupantId) => {
            setOccupantsInDeletion(prev => new Set(prev.add(occupantId)));
        },
    });
    }
    
    const { mutate: deleteOccupantMutation } = 
          useDeleteOccupantMutation(propertyId);


    const handleEditOccupant = (occupant: Occupant) => {
            const modalId = modals.openModal({
              title: "Edit Occupant",
              children: (
                  <OccupantForm
                      occupant={occupant}
                      propertyId={propertyId}
                      occupantTypes={occupantTypes}
                      onCancel={() => modals.closeModal(modalId)}
                      onSucess={() => modals.closeModal(modalId)}
                  />
              ),
            })
        }

    const handleDeleteOccupant = (id: string) => {
        const modalId = modals.openConfirmModal({
            title: "Потвърди изтриване",
            children: <Text size="sm">Сигурни ли сте, че искате да изтриете този ползвател? Това действие не може да бъде отменено.</Text>,
            labels: { confirm: "Изтрий", cancel: "Отмяна" },
            confirmProps: { color: "red" },
            onConfirm: () => {
                deleteOccupantMutation(id);
                modals.closeModal(modalId);
            },
        })
    }

    if(isLoading || !tenants?.ok)
        return <Loading/>;

    return (
        <>
        {
            tenants.value && tenants.value.items.length > 0 ?
            <>
                <Flex justify={'center'} mt={'lg'} mb={'lg'} hiddenFrom='lg'>
                    <Pagination total={tenants.value.totalPages}
                    value={page + 1}
                    onChange={(value) => setPage(value - 1)}/>
                </Flex>
                <SimpleGrid
                cols={{ base: 1, md: 2, lg: 2 }}
                spacing={{ base: 'sm', md: 'md', lg: 'lg' }}>
                    {
                        tenants
                        .value
                        .items.map((value, index) => (
                            <OccupantPaper
                            key={index}
                            isDeleting={occupantsInDeletion.has(value.id)}
                            occupant={value}
                            handleDelete={handleDeleteOccupant}
                            handleEdit={handleEditOccupant}/>
                        ))    
                    }
                </SimpleGrid>
                <Flex justify={'center'} mt={'xl'}>
                    <Pagination total={tenants.value.totalPages}
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