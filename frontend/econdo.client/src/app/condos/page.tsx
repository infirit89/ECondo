import { getBuildingsForUser } from "@/actions/condo";
import CondoCard from "@/components/condoCard/condoCard";
import { AppShellMain, SimpleGrid, Center, Title } from "@mantine/core";
import { IconMoodPuzzled } from "@tabler/icons-react";

export default async function CondosPage() {

    const buildingsResult = await getBuildingsForUser();
    if(buildingsResult.ok)
        console.log(buildingsResult.value);

    return (
        <AppShellMain>
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
        </AppShellMain>
    );
}