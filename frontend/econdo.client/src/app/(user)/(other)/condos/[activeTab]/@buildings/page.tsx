import { getBuildingsForUser } from "@/actions/condo";
import CondoCard from "@/components/condoCard/condoCard";
import { 
    Center,
    Title,
    Button,
    Flex,
    Container,
    SimpleGrid
} from "@mantine/core";
import { IconMoodPuzzled } from "@tabler/icons-react";

import Link from "next/link";

export default async function BuildingsPage() {
    const buildingsResult = await getBuildingsForUser();

    return (
        <Container size="lg" py="xl">
            <Flex justify={'flex-end'} align="center" mb="lg">
                <Button size="md" component={Link} href="/createBuilding">Добави сграда</Button>
            </Flex>
            {
                buildingsResult.ok && buildingsResult.value?.length! > 0 ?
                <SimpleGrid
                cols={{ base: 1, md: 2, lg: 3 }}
                spacing={{ base: 'sm', md: 'md', lg: 'lg' }}>
                    {buildingsResult.value?.map((value, index) => (
                        <CondoCard key={index} {...value}/>
                    ))}
                </SimpleGrid>
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
        </Container>
    );
}
