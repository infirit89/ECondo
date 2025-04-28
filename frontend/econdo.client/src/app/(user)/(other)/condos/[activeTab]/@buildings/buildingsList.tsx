'use client';

import { getBuildingsForUser } from "@/actions/condo";
import { queryKeys } from "@/types/queryKeys";
import { useQuery } from "@tanstack/react-query";
import { useState } from "react";
import Loading from "../loading";
import { Center, Flex, Pagination, SimpleGrid, Title } from "@mantine/core";
import Link from "next/link";
import CondoCard from "@/components/condoCard/condoCard";
import { IconMoodPuzzled } from "@tabler/icons-react";

// note: hard coded for now
const pageSize = 9;

const useBuildingsPagedQuery = (page: number, buildingName?: string) => {
    return useQuery({
        queryKey: queryKeys.buildings.pagedForUser(page, pageSize, buildingName),
        queryFn: () => getBuildingsForUser(
            page,
            pageSize,
            buildingName
        )
    });
}


export default function BuildingsList({ buildingQuery } : { buildingQuery: string | undefined }) {
    const [page, setPage] = useState(0);
    const { data: buildings, isLoading } = useBuildingsPagedQuery(page, buildingQuery);

    if(isLoading || !buildings?.ok)
        return <Loading/>;


    return (
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
                <Link key={index} href={`/buildings/${value.id}/${value.entranceNumber}/properties`}>
                    <CondoCard key={index} {...value} canEdit={false} canDelete/>
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
            <Title c={'dimmed'}>Нямате регистрирани сгради</Title>
        </Center>
    </>
    );
}