import { Button, Center, Container, Paper, Stack, Title } from "@mantine/core";
import Link from "next/link";

export default function StripeSuccessPage() {
    return (
        <Container size="sm" py="xl" my={40} mb={100}>
        <Paper shadow="md" p="xl" radius="md" withBorder>
            <Stack gap="xl">

            <Title order={1} ta="center">
                Добре дошъл обратно!
            </Title>

            <Center mt="md">
                <Button
                component={Link}
                href={`/condos/buildings`} size="md">
                    Към сградите ми
                </Button>
            </Center>
            </Stack>
        </Paper>
        </Container>
    );
}
