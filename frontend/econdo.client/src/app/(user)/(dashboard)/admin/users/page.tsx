'use client';

import { getAllProfiles } from "@/actions/profile";
import Loading from "@/components/loading";
import UserPaper from "@/components/userPaper/userPaper";
import { queryKeys } from "@/types/queryKeys";
import { Center, Container, Flex, Pagination, SimpleGrid, Title } from "@mantine/core";
import { IconMoodPuzzled } from "@tabler/icons-react";
import { useQuery } from "@tanstack/react-query";
import { useState } from "react";

// note: hard coded for now
const pageSize = 9;

const useProfilesPagedQuery = (page: number) => {
    return useQuery({
        queryKey: queryKeys.profiles.allPaged(page, pageSize),
        queryFn: () => getAllProfiles(
            page,
            pageSize,
        ),
    });
}

export default function UserPage() {
    const [page, setPage] = useState(0);
    const { data: profiles, isLoading } = useProfilesPagedQuery(page);

    if(isLoading || !profiles?.ok)
        return <Loading/>;
    
    
    return (
        <Container size="lg" py="xl">
            <Flex justify={'space-between'} mb={'md'}>
                <Title>Потребители</Title>
            </Flex>
            {
                profiles.value && profiles.value.items.length > 0 ?
                <>
                    <Flex justify={'center'} mt={'lg'} mb={'lg'} hiddenFrom='lg'>
                        <Pagination total={profiles.value.totalPages}
                        value={page + 1}
                        onChange={(value) => setPage(value - 1)}/>
                    </Flex>
                    <SimpleGrid
                    cols={{ base: 1, md: 2, lg: 3 }}
                    spacing={{ base: 'sm', md: 'md', lg: 'lg' }}>
                        {profiles.value.items.map((value, index) => (
                            <UserPaper user={value} key={index}/>
                        ))}
                    </SimpleGrid>
                    <Flex justify={'center'} mt={'xl'}>
                        <Pagination total={profiles.value.totalPages}
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
                        <Title c={'dimmed'}>Няма намерени потребители</Title>
                    </Center>
                </>
            }
        </Container>
    );
}