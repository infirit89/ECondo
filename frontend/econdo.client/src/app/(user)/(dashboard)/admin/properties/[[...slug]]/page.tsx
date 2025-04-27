'use client';

import { getAllProperties } from "@/actions/property";
import Loading from "@/components/loading";
import PropertyCard from "@/components/propertyCard";
import { queryKeys } from "@/types/queryKeys";
import { Center, Container, Flex, Pagination, SimpleGrid, Title } from "@mantine/core";
import { IconMoodPuzzled } from "@tabler/icons-react";
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
        enabled: !!page
    });
}

export default function PropertiesPage() {
    const { slug } = useParams<{ slug: string[] | undefined }>();
    const buildingId = slug?.at(0);
    const entranceNumber = slug?.at(1);
    
    if((buildingId === undefined && entranceNumber !== undefined) || (entranceNumber === undefined && buildingId !== undefined))
        notFound();

    const [page, setPage] = useState(0);
    const { data: properties, isLoading } = usePropertiesPagedQuery(page, buildingId, entranceNumber);

    if(isLoading || !properties?.ok)
        return <Loading/>;

    return (
        <Container size="lg" py="xl">
            <Flex justify={'space-between'} mb={'md'}>
                <Title>Имоти</Title>
            </Flex>
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
                        // <Link key={index} href={`/properties/${value.id}`}>
                            <PropertyCard
                            key={index}
                            isDeleting={false}
                            property={value}
                            canEdit={false}
                            />
                        // </Link>
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