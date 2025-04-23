import { Paper, Stack, Container, Title, Text, Center, Button } from "@mantine/core";
import Link from "next/link";

export default async function AcceptInvitationLandingPage(
    {searchParams} : { searchParams : Promise<{ [key: string]: string | string[] | undefined }> }
) {
    const { token = '', email = '' } = await searchParams;

    return (
        <Container size="sm" py="xl" my={40} mb={100}>
        <Paper shadow="md" p="xl" radius="md" withBorder>
            <Stack gap="xl">

            <Title order={1} ta="center">
                Покана за присъединяване
            </Title>

            <Text size="lg" ta="center">
            Поканени сте да се присъедините към управлението на имот в системата ECondo.
            </Text>

            <Center mt="md">
                <Button component={Link} href={`/acceptedInvitation?token=${token}&email=${email}`} size="lg">
                    Приемам поканата
                </Button>
            </Center>
          
            {/* <>
              <Title order={1} align="center" color="green">
                <Center>
                  <Check size={32} />
                </Center>
                Поканата е приета!
              </Title>

              <Text size="lg" align="center">
                Успешно се присъединихте към имота. Вече имате достъп до информацията и функциите за управление.
              </Text>
            </> */}

          </Stack>
        </Paper>
      </Container>
    );
}