import { confirmEmail } from "@/actions/auth";
import { Alert, Container } from "@mantine/core";
import { IconExclamationCircle } from "@tabler/icons-react";
import { isValidationError } from "@/types/apiResponses";
import { redirect } from "next/navigation";
import { confirmAccountEvent } from "@/types/auth";

export default async function ConfirmAccount({searchParams} : { searchParams : Promise<{ [key: string]: string | string[] | undefined }> }) {
    const { token = '', email = '' } = await searchParams;
    
    const res = await confirmEmail(token as string, email as string);
    
    if(!isValidationError(res))
        redirect(`/login?event=${confirmAccountEvent}`);

    return (
        <Container size={420} my={40} mb={100}>
            <Alert variant="light" color={'red'} title='Грешка при потвърждаване на акаунта' icon={<IconExclamationCircle/>}>
                Възникна грешка при потвърждаване на акаунта, моля опитайте отново.
            </Alert>
        </Container>
    );
}