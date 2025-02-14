'use client';

import { emailSchema, firstNameSchema, lastNameSchema, middleNameSchema, passwordSchema, phoneNumberSchema } from "@/utils/ValidationSchemas";
import { zodResolver } from "@hookform/resolvers/zod";
import { TextInput } from "@mantine/core";

import { Box, LoadingOverlay, PasswordInput, Button } from "@mantine/core";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { z, ZodSchema } from "zod";
import { IRegisterFields } from "./RegisterData";

const RegisterSchema : ZodSchema<IRegisterFields> = z
    .object({
        firstName: firstNameSchema,
        middleName: middleNameSchema,
        lastName: lastNameSchema,
        email: emailSchema,
        phone: phoneNumberSchema,
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

export default function RegisterForm() {
    const [isLoading, setLoading] = useState(false);
    
    const form = useForm<IRegisterFields>({
        defaultValues: {
            firstName: '',
            middleName: '',
            lastName: '',
            email: '',
            phone: '',
            password: '',
            confirmPassword: '',
        },
        resolver: zodResolver(RegisterSchema),
    });
    
    const onSubmit = async(data: IRegisterFields) => {
        console.log(data);
    }

    return (
        <form onSubmit={form.handleSubmit(onSubmit)}>
            <Box pos={'relative'}>
                <LoadingOverlay visible={isLoading} zIndex={1000} overlayProps={{ radius: "sm", blur: 2 }}/>
                <TextInput ta={'start'} label="Първо име" {...form.register('firstName')} withAsterisk
                error={form.formState.errors.firstName && form.formState.errors.firstName.message}/>
                
                <TextInput ta={'start'} label="Презиме" {...form.register('middleName')} withAsterisk
                error={form.formState.errors.middleName && form.formState.errors.middleName.message}/>
                
                <TextInput ta={'start'} label="Фамилно име" {...form.register('lastName')} withAsterisk
                error={form.formState.errors.lastName && form.formState.errors.lastName.message}/>
                
                <TextInput ta={'start'} label="Имейл" {...form.register('email')} withAsterisk
                error={form.formState.errors.email && form.formState.errors.email.message}/>
                
                <PasswordInput ta={'start'} label="Парола" mt="md" {...form.register('password')} withAsterisk
                error={form.formState.errors.password && form.formState.errors.password.message} />

                <PasswordInput ta={'start'} label="Потвърди Парола" mt="md" {...form.register('confirmPassword')} withAsterisk
                error={form.formState.errors.confirmPassword && form.formState.errors.confirmPassword.message} />
            </Box>
            <Button fullWidth mt="xl" type={'submit'} disabled={isLoading}>
                Влез
            </Button>
        </form>
    )
}