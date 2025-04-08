import { getBuildingsForUser } from "@/actions/condo";
import CondoCard from "@/components/condoCard/condoCard";
import { SimpleGrid, Center, Title, Box, Button, Flex } from "@mantine/core";
import { IconMoodPuzzled } from "@tabler/icons-react";

export default async function BuildingsPage() {

    const buildingsResult = await getBuildingsForUser();

    return (
        <>
            <Flex justify={'flex-end'} w={'68%'} mt={'lg'}>
                <Button>Добави сграда</Button>
            </Flex>
            {
                buildingsResult.ok ?
                <Center>
                    <SimpleGrid cols={{ base: 1, sm: 2}} my={60} spacing={50}>
                        {buildingsResult.value?.map((value, index) => (
                            <CondoCard key={index} {...value}/>
                        ))}
                    </SimpleGrid>
                </Center>
                :
                <>
                    <Center mt={90} mb={20}>
                        <IconMoodPuzzled size={100} color="#868e96"/>
                    </Center>
                    <Center>
                        <Title c={'dimmed'}>Нямате регистрирани сгради</Title>
                    </Center>
                </>
            }
        </>
    );
}