'use client';

import { passwordSchema } from "@/utils/validationSchemas";
import { zodResolver } from "@hookform/resolvers/zod";
import { Box, Button, LoadingOverlay, PasswordInput } from "@mantine/core";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { z, ZodSchema } from "zod";

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


    const form = useForm<ResetPasswordFields>({
        defaultValues: {
            password: '',
            confirmPassword: '',
        },
        resolver: zodResolver(ResetPasswordSchema),
    });

    const onSubmit = async(data: ResetPasswordFields) => {
        setLoading(true);
        // const res = await axios.post('/api/user/resetPassword', {
        //     email: email,
        //     token: userToken,
        //     password: data.password,
        // }).catch((ex) => {
        //     console.error(ex);
        //     setLoading(false);
        // });

        // if(res && res.status === 200) {
        //     console.log(res);
        //     redirect('/login');
        // }
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
            </Box>
            <Button fullWidth mt="xl" type={'submit'} disabled={isLoading}>
                Промени парола
            </Button>
        </form>
    );
}