'use client';

import { resetPassword } from "@/actions/auth";
import { passwordSchema } from "@/utils/validationSchemas";
import { zodResolver } from "@hookform/resolvers/zod";
import { Box, Button, LoadingOverlay, PasswordInput, Text } from "@mantine/core";
import { redirect } from "next/navigation";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { z, ZodSchema } from "zod";
import { isValidationError } from "@/types/apiResponses";
import { resetPasswordEvent } from "@/types/auth";

interface ResetPasswordFields {
    password: string;
    confirmPassword: string;
}

const ResetPasswordSchema : ZodSchema<ResetPasswordFields> = z
    .object({
        password: passwordSchema,
        confirmPassword: z.string(),
    })
    .refine((data) =>
        {
            return data.password === data.confirmPassword
        }, {
        message: "Паролите не съвпадат",
        path: ["confirmPassword"], // path of error
    });


export default function ResetPasswordForm({ email, token }: { email: string, token: string }) {

    console.log('email: ', email);
    console.log('token: ', token);
    
    const [isLoading, setLoading] = useState(false);
    const [isError, setError] = useState(false);


    const form = useForm<ResetPasswordFields>({
        defaultValues: {
            password: '',
            confirmPassword: '',
        },
        resolver: zodResolver(ResetPasswordSchema),
    });

    const onSubmit = async(data: ResetPasswordFields) => {
        setLoading(true);
        const res = await resetPassword(email, token, data.password);

        if(res.ok)
            redirect(`/login?event=${resetPasswordEvent}`);

        if(isValidationError(res.error)) {
            if(res.error.errors['InvalidToken']) {
                setError(true);      
            }
        }

        setLoading(false);
    }

    return (
        <form onSubmit={form.handleSubmit(onSubmit)}>
            <Box pos={'relative'}>
                <LoadingOverlay visible={isLoading} zIndex={1000} overlayProps={{ radius: "sm", blur: 2 }}/>
                <PasswordInput ta={'start'} label={'Парола'} placeholder="" {...form.register('password', {
                    required: true,
                })} withAsterisk
                error={form.formState.errors.password && form.formState.errors.password.message}/>
                <PasswordInput ta={'start'} label={'Потвърди парола'} mt={"md"} placeholder="" {...form.register('confirmPassword', {
                    required: true,
                })} withAsterisk
                error={form.formState.errors.confirmPassword && form.formState.errors.confirmPassword.message}/>

                { isError ? <Text c={'red'} pt={10} fw={500} size={'sm'}>Този линк за смяна на парола е невалиден или изтекъл!</Text> : <></> }
            </Box>
            <Button fullWidth mt="xl" type={'submit'} disabled={isLoading}>
                Промени парола
            </Button>
        </form>
    );
}