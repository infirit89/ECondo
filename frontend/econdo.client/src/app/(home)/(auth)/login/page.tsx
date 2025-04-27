import { Alert, Anchor, Container, Text, Title } from "@mantine/core";
import LoginForm from "@/components/loginForm/loginForm";
import AccountConfirmationAlert from "@/components/accountConfirmationAlert/accountConfirmationAlert";
import { AuthEvent } from "@/types/auth";
import Link from "next/link";

export default async function Login({ searchParams } : { searchParams : Promise<{ [key: string]: string | string[] | undefined }> }) {
    const { event = '' } = await searchParams;

    return (
        <Container size={420} my={40} mb={100}>
            <AccountConfirmationAlert hidden={(event as AuthEvent) !== 'confirmAccount'}/>

            <Alert variant="light" mb={'md'} hidden={(event as AuthEvent) !== 'accountVerified'}>
                <Text fz={'sm'}>Акаунтът Ви беше потвърден успешно!</Text>
            </Alert>

            <Alert variant="light" mb={'md'} hidden={(event as AuthEvent) !== 'forgotPassword'}>
                <Text fz={'sm'}>Моля, проверете мейла си. Изпратили сме Ви линк, чрез който можете да промените паролата си. Ако не сте получили писмо проверете дали не е попаднало в папка SPAM.</Text>
            </Alert>

            <Alert variant="light" mb={'md'} hidden={(event as AuthEvent) !== 'resetPassword'} color={'teal'}>
                <Text fz={'sm'}>Паролата Ви беше нулирана успешно!</Text>
            </Alert>            
            
            <Title ta="center" fw={900} ff={'Greycliff CF, var(--mantine-font-family)'}>
            Добре дошъл отново!
            </Title>
            <Text c="dimmed" size="sm" ta="center" mt={5}>
            Нямаш профил?{' '}
            <Anchor component={Link} size="sm" href="/register">
                Създай профил
            </Anchor>
            </Text>

            <LoginForm/>
        </Container>
    )
}
