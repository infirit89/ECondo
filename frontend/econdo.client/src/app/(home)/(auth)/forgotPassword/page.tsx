import { Container, Paper, Title } from "@mantine/core";
import { ForgotPasswordForm } from "@/components/forgotPasswordForm/forgotPasswordForm";

export default function ForgotPassword() {

    return (
        <Container size={420} my={100} mb={100}>
            <Title ta="center" fw={900}>
                Нулирай паролата си
            </Title>
            <Paper withBorder shadow="md" p={30} mt={30} radius="md">
                <ForgotPasswordForm/>
            </Paper>
        </Container>
    );
}
