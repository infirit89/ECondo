import { Alert, Anchor, Container, Paper, Text, Title } from "@mantine/core";
import LoginForm from "./loginForm";
import AccountConfirmationAlert from "./accountConfirmationAlert";

export default async function Login({ searchParams } : { searchParams : Promise<{ [key: string]: string | string[] | undefined }> }) {
    const { emailConfirmation = '', forgottenPassword = '' } = await searchParams;

    return (
        <Container size={420} my={40} mb={100}>
            <AccountConfirmationAlert hidden={(emailConfirmation as string) !== 't'}/>

            <Alert variant="light" mb={'md'} hidden={(forgottenPassword as string) !== 't'}>
                <Text fz={'sm'}>Моля, проверете мейла си. Изпратили сме Ви линк, чрез който можете да промените паролата си. Ако не сте получили писмо проверете дали не е попаднало в папка SPAM.</Text>
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
