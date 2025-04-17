import { getBuildingsForUser } from "@/actions/condo";
import CondoCard from "@/components/condoCard/condoCard";
import { 
    Center,
    Title,
    Button,
    Flex, 
    Grid,
    GridCol,
    Container
} from "@mantine/core";
import { IconMoodPuzzled } from "@tabler/icons-react";

import classes from './buildingsPage.module.css';

export default async function BuildingsPage() {

    const buildingsResult = await getBuildingsForUser();

    return (
        <Container size="lg" py="xl">
            <Flex justify={'flex-end'} align="center" mb="lg">
                <Button size="md" component="a" href="/createBuilding">Добави сграда</Button>
            </Flex>
            {
                buildingsResult.ok && buildingsResult.value?.length! > 0 ?
                <Grid justify='center' gutter={'xl'} columns={2} mt={50}>
                    {buildingsResult.value?.map((value, index) => (
                        <GridCol key={index} span={{ base: 2, xs: 1 }}>
                            <Flex className={classes.cardContainer} justify={index % 2 == 0 ? 'flex-end' : 'flex-start'}>
                                <CondoCard key={index} {...value}/>
                            </Flex>
                        </GridCol>
                    ))}
                </Grid>
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
