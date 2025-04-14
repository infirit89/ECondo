import { getBuildingsForUser, registerBuildingEntrance } from "@/actions/condo";
import CondoCard, { CondoCardProps } from "@/components/condoCard/condoCard";
import { 
    SimpleGrid,
    Center,
    Title,
    Button,
    Flex, 
    Grid,
    GridCol,
    Container,
    Card,
    Group, CardSection, Text} from "@mantine/core";
import { IconMoodPuzzled } from "@tabler/icons-react";

import classes from './buildingsPage.module.css';

interface CardItem {
    id: number;
    title: string;
    content: string;
  }

export default async function BuildingsPage() {

    const cards: CardItem[] = [
        { id: 1, title: 'Card 1', content: 'This is the first card' },
    { id: 2, title: 'Card 2', content: 'This is the second card' },
    { id: 3, title: 'Card 3', content: 'This is the third card' },
    ];

    const buildingsResult = await getBuildingsForUser();
    const props: CondoCardProps = {
        id: '',
        name: 'Тест буилдинг',
        image: '',
        provinceName: 'София град',
        municipality: 'Столична',
        settlementPlace: 'София',
        neighborhood: 'Дървеница',
        postalCode: '1756',
        street: 'Равногор',
        streetNumber: '1',
        buildingNumber: '1',
        entranceNumber: '1',
    }

    return (
        <Container size="lg" py="xl">
            <Flex justify={'flex-end'} align="center" mb="lg">
                <Button size="md" component="a" href="/createBuilding">Добави сграда</Button>
            </Flex>
            {
                buildingsResult.ok ?
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