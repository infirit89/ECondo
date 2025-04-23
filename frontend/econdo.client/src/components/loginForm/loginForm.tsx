'use client';

import { TextInput } from "@mantine/core";

import { Box, LoadingOverlay, PasswordInput, Group, Checkbox, Anchor, Button, Text } from "@mantine/core";
import { useEffect, useReducer } from "react";
import { useForm } from "react-hook-form";
import { UserErrorCode } from "@/types/apiErrors";
import { login } from "@/actions/auth";
import { redirect } from "next/navigation";
import formReducer, { initialFormState } from "@/lib/formState";
import { z, ZodSchema } from "zod";
import { emailSchema } from "@/utils/validationSchemas";
import { zodResolver } from "@hookform/resolvers/zod";

interface LoginFormFields {
    email: string;
    password: string;
}

const LoginSchema: ZodSchema<LoginFormFields> = z
    .object({
        email: emailSchema,
        password: z.string().nonempty('Паролата е задължително поле'),
    });

export default function LoginForm() {
    const [loginState, dispatch] = useReducer(formReducer, initialFormState);
 
    useEffect(() => {
        if(loginState === 'success') 
            redirect('/condos/properties');
    }, [loginState]);

    const form = useForm<LoginFormFields>({
        defaultValues: {
            email: '',
            password: '',
        },
        resolver: zodResolver(LoginSchema),
    });
    
    const onSubmit = async(data: LoginFormFields) => {
        dispatch({ type: 'SUBMIT' });
        const res = await login(data);
        console.log(res);
        if(!res.ok) {
            if(res.error.title === UserErrorCode.NotFound) {
                form.setValue('password', '');
                dispatch({type: 'ERROR'});
                return;
            }
        }

        dispatch({type: 'SUCCESS'});
    }

    const isLoading = loginState === 'loading';
    const hasError = loginState === 'error';

    return (
        <form onSubmit={form.handleSubmit(onSubmit)}>
            <Box pos={'relative'}>
                <LoadingOverlay visible={isLoading} zIndex={1000} overlayProps={{ radius: "sm", blur: 2 }}/>
                <TextInput ta={'start'} label="Имейл" required {...form.register('email')} withAsterisk
                error={form.formState.errors.email && form.formState.errors.email.message}/>
                
                <PasswordInput ta={'start'} label="Парола" required mt="md" {...form.register('password')} withAsterisk
                error={form.formState.errors.password && form.formState.errors.password.message} />

                { hasError ? <Text c={'red'} pt={10} fw={500} size={'sm'}>Потребителското име или паролата са неправилни!</Text> : <></> }
                <Group justify="space-between" mt="lg">
                <Checkbox label="Запомни ме" />
                <Anchor size="sm" href="/forgotPassword">
                    Забравена парола?
                </Anchor>
                </Group>
            </Box>
            <Button fullWidth mt="xl" type={'submit'} disabled={isLoading}>
                Влез
            </Button>
        </form>
    )
}