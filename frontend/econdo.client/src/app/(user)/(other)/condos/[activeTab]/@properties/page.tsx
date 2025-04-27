'use client';

import { getPropertiesForUser } from "@/actions/property";
import Loading from "@/components/loading";
import PropertyCard from "@/components/propertyCard";
import { queryKeys } from "@/types/queryKeys";
import { SimpleGrid, Center, Title, Flex, Pagination, Container } from "@mantine/core";
import { IconMoodPuzzled } from "@tabler/icons-react";
import { useQuery } from "@tanstack/react-query";
import Link from "next/link";
import { useState } from "react";

const pageSize = 9;

const useQueryPropertiesPaged = (page: number) => {
    return useQuery({
        queryKey: queryKeys.properties.pagedForUser(
            page,
            pageSize),
        queryFn: () => getPropertiesForUser(
            page,
            pageSize),
    })
}

export default function CondosPage() {

    const [page, setPage] = useState(0);
    const { data: properties, isLoading } = useQueryPropertiesPaged(page);
    
    if(isLoading || !properties?.ok)
        return <Loading/>;
    
    return (
        <Container size={'lg'} py={'xl'}>
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
                            <Link key={index} href={`/properties/${value.id}`}>
                                <PropertyCard
                                key={index}
                                isDeleting={false}
                                property={value}
                                canEdit={false}
                                />
                            </Link>
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
                    <Title c={'dimmed'}>Нямате регистрирани имоти</Title>
                </Center>
            </> 
        }
        </Container>
    );
}