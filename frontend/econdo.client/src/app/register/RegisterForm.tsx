'use client';

import { emailSchema, firstNameSchema, lastNameSchema, middleNameSchema, passwordSchema, phoneNumberSchema } from '@/utils/validationSchemas';
import { zodResolver } from "@hookform/resolvers/zod";

import { Box, LoadingOverlay, PasswordInput, Button, TextInput } from "@mantine/core";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { z, ZodSchema } from "zod";
import { RegisterFields } from "@/app/_data/registerData";
import { register } from '@/actions/auth';
import { isApiError } from '../_data/apiResponses';

const RegisterSchema : ZodSchema<RegisterFields> = z
    .object({
        firstName: firstNameSchema,
        middleName: middleNameSchema,
        lastName: lastNameSchema,
        username: z.string(),
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
    
    const form = useForm<RegisterFields>({
        defaultValues: {
            firstName: '',
            middleName: '',
            lastName: '',
            email: '',
            phone: '',
            password: '',
            confirmPassword: '',
            username: '',
        },
        resolver: zodResolver(RegisterSchema),
    });
    
    const onSubmit = async(data: RegisterFields) => {
        data.username = data.email;
        console.log('aaaaaaaaaaaaaaaaaaaaaaa');
        const res = await register(data);
        if(isApiError(res))
            console.error(res);
        
        console.log(data);
    }

    return (
        <form onSubmit={form.handleSubmit(onSubmit)}>
            <Box pos={'relative'}>
                <LoadingOverlay visible={isLoading} zIndex={1000} overlayProps={{ radius: "sm", blur: 2 }}/>
                <TextInput ta={'start'} label="Първо име" placeholder='Петър' {...form.register('firstName')} withAsterisk
                error={form.formState.errors.firstName && form.formState.errors.firstName.message}/>
                
                <TextInput ta={'start'} label="Презиме" mt={'sm'} placeholder='Петров' {...form.register('middleName')} withAsterisk
                error={form.formState.errors.middleName && form.formState.errors.middleName.message}/>
                
                <TextInput ta={'start'} label="Фамилно име" mt={'sm'} placeholder='Попов' {...form.register('lastName')} withAsterisk
                error={form.formState.errors.lastName && form.formState.errors.lastName.message}/>
                
                <TextInput ta={'start'} label='Телефон' mt={'sm'} placeholder='0881234567' {...form.register('phone')} withAsterisk
                error={form.formState.errors.phone && form.formState.errors.phone.message} />

                <TextInput ta={'start'} lightHidden darkHidden/>
                
                <TextInput ta={'start'} label="Имейл" mt={'sm'} placeholder='peter@gmail.com' {...form.register('email')} withAsterisk
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