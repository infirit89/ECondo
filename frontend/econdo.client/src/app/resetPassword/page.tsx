import { Container, Paper, Title } from "@mantine/core";
import ResetPasswordForm from "./resetPasswordForm";

export default async function ResetPassword({searchParams} : { searchParams : Promise<{ [key: string]: string | string[] | undefined }> }) {
    const { token = '', email = '' } = await searchParams;
    
    return (
        <Container size={420} my={100} mb={100}>
            <Title ta="center" fw={900}>
                Промени паролата си
            </Title>
            <Paper withBorder shadow="md" p={30} mt={30} radius="md">
                <ResetPasswordForm email={email as string} token={token as string}/>
            </Paper>
        </Container>
    );
}
