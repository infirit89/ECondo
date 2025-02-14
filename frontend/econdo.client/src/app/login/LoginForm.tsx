'use client';

import { TextInput } from "@mantine/core";

import { Box, LoadingOverlay, PasswordInput, Group, Checkbox, Anchor, Button } from "@mantine/core";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { LoginFields } from "@/app/_data/loginData";
import { login, setAuthCookie } from "../actions";

export default function LoginForm() {
    const [isLoading, setLoading] = useState(false);
    
    const form = useForm<LoginFields>({
        defaultValues: {
            email: '',
            password: '',
        },
    });
    
    const onSubmit = async(data: LoginFields) => {
        const loginRes = await login(data);
        if('errors' in loginRes) {
            console.error(loginRes.errors);
            return;
        }

        await setAuthCookie({ accessToken: loginRes.accessToken, maxAge: loginRes.expiresIn * 60, refreshToken: loginRes.refreshToken });
    }

    return (
        <form onSubmit={form.handleSubmit(onSubmit)}>
            <Box pos={'relative'}>
                <LoadingOverlay visible={isLoading} zIndex={1000} overlayProps={{ radius: "sm", blur: 2 }}/>
                <TextInput ta={'start'} label="Имейл" required {...form.register('email')} withAsterisk
                error={form.formState.errors.email && form.formState.errors.email.message}/>
                
                <PasswordInput ta={'start'} label="Парола" required mt="md" {...form.register('password')} withAsterisk
                error={form.formState.errors.password && form.formState.errors.password.message} />

                {/* { hasError ? <Text c={'red'} pt={10} fw={500} size={'sm'}>Потребителското име или паролата са неправилни!</Text> : <></> } */}
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