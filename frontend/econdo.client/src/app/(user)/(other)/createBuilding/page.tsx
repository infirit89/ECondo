import { getProvinces } from "@/actions/condo";
import { Container, Anchor, Center, Box, AppShellMain, Title } from "@mantine/core";
import { IconArrowLeft } from "@tabler/icons-react";
import Link from "next/link";
import RegisterBuildingPaper from "./registerBuildingPaper";

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

                <RegisterBuildingPaper provinces={provinces}/>
            </Container>
        </AppShellMain>
    );
}
