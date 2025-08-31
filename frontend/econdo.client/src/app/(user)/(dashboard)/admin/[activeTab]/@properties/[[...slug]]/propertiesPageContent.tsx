'use client';

import { getAllProperties } from "@/actions/property";
import Loading from "@/components/loading";
import PropertyCard from "@/components/propertyCard";
import { queryKeys } from "@/types/queryKeys";
import { Alert, Center, Container, Flex, Pagination, SimpleGrid, Title } from "@mantine/core";
import { IconExclamationCircle, IconMoodPuzzled } from "@tabler/icons-react";
import { useQuery } from "@tanstack/react-query";
import { notFound, useParams } from "next/navigation";
import { useState } from "react";

// note: hard coded for now
const pageSize = 9;

const usePropertiesPagedQuery = (page: number, buildingId?: string, entranceNumber?: string) => {
    return useQuery({
        queryKey: queryKeys.properties.allPaged(page, pageSize, buildingId, entranceNumber),
        queryFn: () => getAllProperties(
            page,
            pageSize,
            buildingId && entranceNumber ? {
                buildingId: buildingId,
                entranceNumber: entranceNumber,
            } : undefined,
        ),
    });
}

export default function PropertiesPageContent() {
    const { slug } = useParams<{ slug: string[] | undefined }>();
    const buildingId = slug?.at(0);
    const entranceNumber = slug?.at(1);
    
    if((buildingId === undefined && entranceNumber !== undefined) || (entranceNumber === undefined && buildingId !== undefined))
        notFound();

    const [isDeleteError, setDeleteError] = useState(false);
    const [page, setPage] = useState(0);
    const { data: properties, isLoading } = usePropertiesPagedQuery(
        page, 
        buildingId, 
        !entranceNumber ? undefined : decodeURIComponent(entranceNumber));

    if(isLoading || !properties?.ok)
        return <Loading/>;

    const onPropertyDeleteError = () => {
        setDeleteError(true);
    }

    return (
        <Container size="lg" py="xl">
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
                        {properties.value.items.map((value, index) => (
                            <PropertyCard
                            key={index}
                            property={value}
                            canEdit
                            canDelete
                            onDeleteError={onPropertyDeleteError}
                            />
                        ))}
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
                        <Title c={'dimmed'}>Няма намерени сгради</Title>
                    </Center>
                </>
            }
        </Container>
    );
}