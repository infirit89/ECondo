'use client';
import { Card, CardSection, Text, Image, Center, MantineShadow, useMantineTheme } from "@mantine/core";
import { useHover } from "@mantine/hooks";

export interface CondoCardProps {
    name: string;
    image?: string;
}

export default function CondoCard({ name, image }: CondoCardProps) {
    const {hovered, ref} = useHover();
    const theme = useMantineTheme();

    return (
        <Card ref={ref} shadow={hovered ? 'xl' : 'sm'} padding="lg" radius="md" withBorder>
            <CardSection>
                <Image
                src={image ? image : "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d4/The_Lauren_condo_Bethesda_MD_2021-12-12_10-11-55_1.jpg/1200px-The_Lauren_condo_Bethesda_MD_2021-12-12_10-11-55_1.jpg"}
                height={180}
                alt="Norway"
                />
            </CardSection>

            {/* <Group justify="space-between" mt="md" mb="xs"> */}
            <Center mt={'md'} mb={'xs'}>
                <Text fw={500}>{name}</Text>
            </Center>
                {/* <Badge color="pink">On Sale</Badge> */}
            {/* </Group> */}

            {/* <Text size="sm" c="dimmed">
                With Fjord Tours you can explore more of the magical fjord landscapes with tours and
                activities on and around the fjords of Norway
            </Text> */}
        </Card>
    );
}