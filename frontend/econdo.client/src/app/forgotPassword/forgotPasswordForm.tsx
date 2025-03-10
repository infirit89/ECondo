'use client';

import { forgotPassword } from "@/actions/auth";
import { emailSchema } from "@/utils/validationSchemas";
import { zodResolver } from "@hookform/resolvers/zod";
import { Box, Button, LoadingOverlay, TextInput } from "@mantine/core";
import { redirect } from "next/navigation";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { z, ZodSchema } from "zod";

interface ForgotPasswordFields {
    email: string;
}

const ResetPasswordSchema : ZodSchema<ForgotPasswordFields> = z
    .object({
        email: emailSchema,
    });

export function ForgotPasswordForm() {
    const [isLoading, setLoading] = useState(false);

    const form = useForm<ForgotPasswordFields>({
        defaultValues: {
            email: ''
        },
        resolver: zodResolver(ResetPasswordSchema),
    });

    const onSubmit = async(data: ForgotPasswordFields) => {
        setLoading(true);

        const res = await forgotPassword(data.email);
        if(res.ok) {
            redirect('/login');
        }

        console.error(res.error);
        setLoading(false);
    }
    
    return (
        <form onSubmit={form.handleSubmit(onSubmit)}>
            <Box pos={'relative'}>
                <LoadingOverlay visible={isLoading} zIndex={1000} overlayProps={{ radius: "sm", blur: 2 }}/>
                <TextInput ta={'start'} label={'Имейл'} placeholder="peter@gmail.com" {...form.register('email', {
                    required: true,
                })} withAsterisk
                error={form.formState.errors.email && form.formState.errors.email.message}/>
            </Box>
            <Button fullWidth mt="xl" type={'submit'} disabled={isLoading}>
                Нулирай парола
            </Button>
        </form>
    );
}