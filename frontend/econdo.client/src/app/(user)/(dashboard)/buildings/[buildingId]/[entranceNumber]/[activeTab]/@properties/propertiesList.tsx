'use client';

import { deleteProperty, getPropertiesInEntrance } from "@/actions/property";
import Loading from "@/components/loading";
import { Center, Grid, GridCol, Title, Pagination, Alert } from "@mantine/core";
import { IconExclamationCircle, IconMoodPuzzled } from "@tabler/icons-react";
import { useParams } from "next/navigation";
import { useState } from "react";
import PropertyCard from "./propertyCard";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { queryKeys } from "@/types/queryKeys";

// hard coded for now
const pageSize = 8;

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

    const [isDeleteError, setDeleteError] = useState(false);
    const [deletedPropertyId, setDeletedPropertyId] = useState<string | undefined>(undefined);
    const [page, setPage] = useState(0);

    const queryClient = useQueryClient();
    const { data: properties, isLoading } = useQueryPropertiesPaged(buildingId, entranceNumber, page);

    const useDeleteProperty = () => {
        return useMutation({
            mutationFn: (propertyId: string) => deleteProperty({buildingId, entranceNumber, propertyId}),
            onSuccess: (data) => {
                if(!data.ok) {
                    setDeleteError(true);
                    setDeletedPropertyId(undefined);
                    return;
                }
                
                queryClient.invalidateQueries({
                    queryKey: queryKeys.properties.pagedInEntrance(
                        buildingId,
                        entranceNumber,
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
                    <Grid>
                        {
                            properties
                            .value
                            .items.map((value, index) => (
                            <GridCol key={index} span={{ base: 2, xs: 3 }}>
                                <PropertyCard
                                key={index}
                                isDeleting={value.id === deletedPropertyId}
                                property={value}
                                handleDelete={(id) => deletePropertyMutation(id)} 
                                buildingId={buildingId}
                                entranceNumber={entranceNumber}
                                />
                            </GridCol>
                            ))    
                        }
                    </Grid>
                    <Pagination total={properties.value.totalPages}
                    value={page + 1}
                    onChange={(value) => setPage(value - 1)}
                    mt={'md'}/>
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