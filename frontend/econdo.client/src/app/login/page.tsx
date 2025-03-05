import { Alert, Anchor, Container, List, ListItem, Paper, Text, Title } from "@mantine/core";
import LoginForm from "./loginForm";

export default async function Login({ searchParams } : { searchParams : Promise<{ [key: string]: string | string[] | undefined }> }) {
    const { emailConfirmation = '' } = await searchParams;

    return (
        <Container size={420} my={40} mb={100}>
            <Alert variant="light" mb={'md'} hidden={(emailConfirmation as string) !== 't'}>
                <Text fz={'sm'}>Акаунтът Ви беше създаден успешно, проверете имейла си за потвърждение.</Text>
                <Text fz={'sm'} mt={'xs'}>Не го намирате?</Text>
                <List type={'unordered'}>
                    <ListItem fz={'sm'}>Проверете папка спам.</ListItem>
                    <ListItem fz={'sm'}><Anchor href="#" fz={'sm'}>Повторно изпращане</Anchor></ListItem>
                </List>
                {/* <Button size={'sm'} mt={'md'}>Повторно изпращане</Button> */}
            </Alert>
            <Title ta="center" fw={900} ff={'Greycliff CF, var(--mantine-font-family)'}>
            Добре дошъл отново!
            </Title>
            <Text c="dimmed" size="sm" ta="center" mt={5}>
            Нямаш профил?{' '}
            <Anchor size="sm" href="/register">
                Създай профил
            </Anchor>
            </Text>

            <Paper withBorder shadow="md" p={30} mt={30} radius="md">
                <LoginForm/>
            </Paper>
        </Container>
    )
}
