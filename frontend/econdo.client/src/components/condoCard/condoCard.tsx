'use client';
import { Card, CardSection, Text, Image, Center, Divider } from "@mantine/core";
import classes from './condoCard.module.css';
import { useHover } from "@mantine/hooks";
import Link from "next/link";

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
    street,
    streetNumber,
    buildingNumber,
    entranceNumber,
}: CondoCardProps) {
    const {hovered, ref} = useHover();

    return (
        <Card
        className={`${classes.condoCard}`} 
        ref={ref} 
        shadow={hovered ? 'xl' : 'sm'} 
        padding="xl"
        radius="lg"
        withBorder
        style={{ transition: "transform 0.2s ease, box-shadow 0.2s ease" }}
        onMouseEnter={(e) => (e.currentTarget.style.transform = "translateY(-4px)")}
        onMouseLeave={(e) => (e.currentTarget.style.transform = "translateY(0)")}>
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