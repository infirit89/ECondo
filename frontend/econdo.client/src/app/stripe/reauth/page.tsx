import { Button, Center, Container, Paper, Stack, Title } from "@mantine/core";
import { IconX } from "@tabler/icons-react";
import Link from "next/link";

export default function StripeReautPage() {
    return (
        <Container size="sm" py="xl" my={40} mb={100}>
            <Paper shadow="md" p="xl" radius="md" withBorder>
                <Stack gap="xl">

                <Title order={1} ta="center" c="red">
                    <Center>
                        <IconX size={32} />
                    </Center>
                Оопс, грешка при свързването със Stripe! Моля опитай отново!
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
