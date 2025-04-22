import { getProvinces } from "@/actions/condo";
import RegisterBuildingEntranceForm from "@/components/registerBuildingEntranceForm/registerBuildingEntranceForm";
import { Container, Anchor, Paper, Center, Box, AppShellMain, Title } from "@mantine/core";
import { IconArrowLeft } from "@tabler/icons-react";
import Link from "next/link";

export default async function CreateBuildingPage() {

    const provinces = await getProvinces();

    return (
        <AppShellMain>
            <Container size={700} my={40} mb={100}>
                <Title ta="center" fw={900} ff={'Greycliff CF, var(--mantine-font-family)'}>
                    Регистрирай сграда
                </Title>
                
                <Anchor component={Link} size="sm" href="/condos/buildings">
                    <Center inline>
                        <IconArrowLeft size={12}/>
                        <Box ml={5}>Назад</Box>
                    </Center>
                </Anchor>

                <Paper withBorder shadow="md" p={30} mt={10} radius="md">
                    <RegisterBuildingEntranceForm provinces={provinces}/>
                </Paper>
            </Container>
        </AppShellMain>
    );
}
