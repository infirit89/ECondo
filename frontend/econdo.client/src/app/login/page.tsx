import { Anchor, Container, Paper, Text, Title } from "@mantine/core";
import LoginForm from "./LoginForm";

export default function Login() {
    return (
        <Container size={420} my={40} mb={100}>
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
