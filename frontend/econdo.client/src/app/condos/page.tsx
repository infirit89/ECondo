import CondoCard from "@/components/condoCard";
import { AppShellMain, SimpleGrid, Center } from "@mantine/core";

export default function CondosPage() {

    return (
        <AppShellMain>
            <Center>
                <SimpleGrid cols={{ base: 1, sm: 2 }} my={60} spacing={50}>
                    <CondoCard name="Дигитална собственост 1"/>
                    <CondoCard name="Дигитална собственост 2"/>
                    <CondoCard name="Дигитална собственост 3"/>
                    <CondoCard name="Дигитална собственост 4"/>
                </SimpleGrid>
            </Center>
        </AppShellMain>
    );
}