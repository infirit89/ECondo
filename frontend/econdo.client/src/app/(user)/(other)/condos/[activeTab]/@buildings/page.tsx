'use client';

import {
    Button,
    Flex,
    Container,
    ActionIcon,
    useMantineTheme,
    TextInput,
    Stack
} from "@mantine/core";
import { IconArrowRight, IconSearch } from "@tabler/icons-react";

import Link from "next/link";
import { useState } from "react";
import { z } from "zod";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import BuildingsList from "./buildingsList";

const searchSchema = z.object({
    searchQuery: z.string()
    .optional()
    .or(z.literal('')),
});

type SearchFormValues = z.infer<typeof searchSchema>;

export default function BuildingsPage() {

    const theme = useMantineTheme();

    const [buildingQuery, setBuildingQuery] = useState<string | undefined>();

    const { register, handleSubmit } = useForm<SearchFormValues>({
        resolver: zodResolver(searchSchema),
    });

    const handleSearch = (data: SearchFormValues) => {
        setBuildingQuery(data.searchQuery);
    }

    return (
        <Container size="lg" py="xl">
            <Flex justify={'space-between'} align="center" mb="xl" visibleFrom="sm">
                <form onSubmit={handleSubmit(handleSearch)} style={{ width: '80%' }}>
                    <TextInput
                    radius="xl"
                    size="md"
                    placeholder="Потърси сграда по име"
                    rightSectionWidth={42}
                    pr={'xs'}
                    w={'100%'}
                    leftSection={<IconSearch size={18} stroke={1.5} />}
                    rightSection={
                        <ActionIcon type="submit" size={32} radius="xl" color={theme.primaryColor} variant="filled">
                            <IconArrowRight size={18} stroke={1.5} />
                        </ActionIcon>
                    }
                    {...register('searchQuery')}
                    />
                </form>
                <Button size="md" component={Link} href="/createBuilding">Добави сграда</Button>
            </Flex>
            <Stack align="center" mb="xl" hiddenFrom="sm">
                <TextInput
                radius="xl"
                size="md"
                placeholder="Потърси сграда по име"
                rightSectionWidth={42}
                w={'100%'}
                leftSection={<IconSearch size={18} stroke={1.5} />}
                rightSection={
                    <ActionIcon size={32} radius="xl" color={theme.primaryColor} variant="filled">
                    <IconArrowRight size={18} stroke={1.5} />
                    </ActionIcon>
                }
                />
                <Button size="md" component={Link} href="/createBuilding">Добави сграда</Button>
            </Stack>
            <BuildingsList buildingQuery={buildingQuery}/>
        </Container>
    );
}
