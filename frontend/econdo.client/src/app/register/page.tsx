import { Container, Title, Text, Anchor, Paper } from "@mantine/core";
import RegisterForm from "./registerForm";

export default function Register() {
    return (
        <Container size={420} my={40} mb={100}>
            <Title ta="center" fw={900} ff={'Greycliff CF, var(--mantine-font-family)'}>
                Регистрация
            </Title>
            <Text c="dimmed" size="sm" ta="center" mt={5}>
                Вече имаш профил?{' '}
                <Anchor size="sm" href="/login">
                    Вход
                </Anchor>
            </Text>

            <Paper withBorder shadow="md" p={30} mt={30} radius="md">
                <RegisterForm/>
            </Paper>
        </Container>
    )
}