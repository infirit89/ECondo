import { confirmEmail } from "@/actions/auth";
import { Alert, Container } from "@mantine/core";
import { IconExclamationCircle } from "@tabler/icons-react";
import { isApiError } from "../_data/apiResponses";
import { redirect } from "next/navigation";

export default async function ConfirmAccount({searchParams} : { searchParams : Promise<{ [key: string]: string | string[] | undefined }> }) {
    const { token = '', email = '' } = await searchParams;
    
    const res = await confirmEmail(token as string, email as string);
    
    if(!isApiError(res))
        redirect('/login');

    return (
        <Container size={420} my={40} mb={100}>
            <Alert variant="light" color={'red'} title='Грешка при потвърждаване на акаунта' icon={<IconExclamationCircle/>}>
                Възникна грешка при потвърждаване на акаунта, моля опитайте отново.
            </Alert>
        </Container>
    );
}