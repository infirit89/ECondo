import { Anchor, Box, Button, Center, Container, Paper, Stack, Text, Title } from "@mantine/core";
import { IconArrowRight, IconCheck } from "@tabler/icons-react";
import Link from "next/link";

export default async function AcceaptInvitationPage(
    {searchParams} : { searchParams : Promise<{ [key: string]: string | string[] | undefined }> }
) {
    const { token = '', email = '' } = await searchParams;
    console.log(token);
    console.log(email);
    
    return (
        <Container size="sm" py="xl" my={40} mb={100}>
            <Paper shadow="md" p="xl" radius="md" withBorder>
                <Stack gap="xl">
                    <Title order={1} ta="center" c="green">
                    <Center>
                        <IconCheck size={32} />
                    </Center>
                    Поканата е приета!
                    </Title>

                    <Text size="lg" ta="center">
                    Успешно се присъединихте към имота. Вече имате достъп до информацията и функциите за управление.
                    </Text>

                    <Anchor component={Link} href={'/condos/properties'} ta={'center'}>
                        <Center inline>
                            <Box mr={5} mb={2}>
                                Към имотите ми
                            </Box>
                            <IconArrowRight size={20}/>
                        </Center>
                    </Anchor>
    
                </Stack>
            </Paper>
        </Container>
    );
}