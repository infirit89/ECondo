'use client';
import { getAllBuildings } from "@/actions/condo";
import CondoCard from "@/components/condoCard/condoCard";
import Loading from "@/components/loading";
import { queryKeys } from "@/types/queryKeys";
import { Center, Container, Flex, Pagination, SimpleGrid, Title } from "@mantine/core";
import { IconMoodPuzzled } from "@tabler/icons-react";
import { useQuery } from "@tanstack/react-query";
import Link from "next/link";
import { useState } from "react";

// note: hard coded for now
const pageSize = 9;

const useBuildingsPagedQuery = (page: number) => {
    return useQuery({
        queryKey: queryKeys.buildings.pagedForUser(page, pageSize),
        queryFn: () => getAllBuildings(
            page,
            pageSize,
        )
    });
}


export default function BuildingsPage() {
    const [page, setPage] = useState(0);
    const { data: buildings, isLoading } = useBuildingsPagedQuery(page);

    if(isLoading || !buildings?.ok)
        return <Loading/>;
    
    return (
        <Container size="lg" py="xl">
            <Flex justify={'space-between'} mb={'md'}>
                <Title>Сгради</Title>
            </Flex>
            {
                buildings.value && buildings.value.items.length > 0 ?
                <>
                    <Flex justify={'center'} mt={'lg'} mb={'lg'} hiddenFrom='lg'>
                        <Pagination total={buildings.value.totalPages}
                        value={page + 1}
                        onChange={(value) => setPage(value - 1)}/>
                    </Flex>
                    <SimpleGrid
                    cols={{ base: 1, md: 2, lg: 3 }}
                    spacing={{ base: 'sm', md: 'md', lg: 'lg' }}>
                        {buildings.value.items.map((value, index) => (
                            <Link key={index} href={`/admin/properties/${value.id}/${value.entranceNumber}`}>
                                <CondoCard key={index} {...value}/>
                            </Link>
                        ))}
                    </SimpleGrid>
                    <Flex justify={'center'} mt={'xl'}>
                        <Pagination total={buildings.value.totalPages}
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