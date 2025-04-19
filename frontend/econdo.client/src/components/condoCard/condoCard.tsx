'use client';
import { Card, CardSection, Text, Image, Center } from "@mantine/core";
import classes from './condoCard.module.css';
import { useHover } from "@mantine/hooks";
import { redirect } from "next/navigation";

export interface CondoCardProps {
    id: string;
    name: string;
    image?: string;
    provinceName: string;
    municipality: string;
    settlementPlace: string;
    neighborhood: string;
    postalCode: string;
    street: string;
    streetNumber: string;
    buildingNumber: string;
    entranceNumber: string;
}

export default function CondoCard({ 
    id,
    name,
    image,
    provinceName,
    municipality,
    settlementPlace,
    neighborhood,
    postalCode,
    street,
    streetNumber,
    buildingNumber,
    entranceNumber,
}: CondoCardProps) {
    const {hovered, ref} = useHover();
    
    const selectBuilding = () => {
        redirect(`/buildings/${id}/${entranceNumber}/properties`);
    }

    return (
        <Card onClick={selectBuilding} className={`max-w-xs ${classes.condoCard}`} ref={ref} shadow={hovered ? 'xl' : 'sm'} padding="lg" radius="md" withBorder>
            <CardSection>
                <Image
                className={`h-[180]`}
                src={image ? image : "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d4/The_Lauren_condo_Bethesda_MD_2021-12-12_10-11-55_1.jpg/1200px-The_Lauren_condo_Bethesda_MD_2021-12-12_10-11-55_1.jpg"}
                alt={name}
                />
            </CardSection>

            {/* <Group justify="space-between" mt="md" mb="xs"> */}
            <Center mt={'md'} mb={'xs'}>
                <Text fw={500}>{name} (вх. {entranceNumber})</Text>
            </Center>
                {/* <Badge color="pink">On Sale</Badge> */}
            {/* </Group> */}

            <Text size="sm" c="dimmed" ta={'center'}>
                обл. {provinceName}, общ. {municipality}, {settlementPlace}, кв. {neighborhood}, ул. {street} {streetNumber}, сг. {buildingNumber}, вх. {entranceNumber}
            </Text>
        </Card>
    );
}