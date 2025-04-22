'use client';

import { emailSchema, passwordSchema } from '@/utils/validationSchemas';
import { zodResolver } from "@hookform/resolvers/zod";

import { Box, LoadingOverlay, PasswordInput, Button, TextInput } from "@mantine/core";
import { useEffect, useReducer } from "react";
import { useForm } from "react-hook-form";
import { z, ZodSchema } from "zod";
import { register } from '@/actions/auth';
import { isValidationError } from '@/types/apiResponses';
import { redirect } from 'next/navigation';
import { confirmAccountEvent } from '@/types/auth';
import formReducer, { initialFormState } from '@/lib/formState';

interface RegisterFormFields {
    username: string;
    email: string;
    password: string;
    confirmPassword: string;
}

const RegisterSchema: ZodSchema<RegisterFormFields> = z
    .object({
        username: z.string(),
        email: emailSchema,
        password: passwordSchema,
        confirmPassword: z.string(),
    })
    .refine((data) => {
        return data.password === data.confirmPassword
    }, {
        message: "Паролите не съвпадат",
        path: ["confirmPassword"], // path of error
    });

export default function RegisterForm() {
    const [registerState, dispatch] = useReducer(formReducer, initialFormState);

    const form = useForm<RegisterFormFields>({
        defaultValues: {
            email: '',
            password: '',
            confirmPassword: '',
            username: '',
        },
        resolver: zodResolver(RegisterSchema),
    });

    useEffect(() => {
        if (registerState === 'success')
            redirect(`/login?event=${confirmAccountEvent}`);

    }, [registerState]);

    const onSubmit = async (data: RegisterFormFields) => {
        dispatch({ type: 'SUBMIT' });
        data.username = data.email;
        const registerResult = await register({
            email: data.email,
            username: data.username,
            password: data.password,
        });
        if(!registerResult.ok && isValidationError(registerResult.error)) {
            form.setError(
                'email',
                {
                    message: `Имейлът ${data.email} вече съществува`,
                },
            );
            dispatch({ type: 'ERROR' });
            return;
        }

        dispatch({ type: 'SUCCESS' });
    }

    const isLoading = registerState === 'loading';

    return (
        <form onSubmit={form.handleSubmit(onSubmit)}>
            <Box pos={'relative'}>
                <LoadingOverlay visible={isLoading} zIndex={1000} overlayProps={{ radius: "sm", blur: 2 }} />
                {/* <TextInput ta={'start'} label="Първо име" placeholder='Петър' {...form.register('firstName')} withAsterisk
                error={form.formState.errors.firstName && form.formState.errors.firstName.message}/>
                
                <TextInput ta={'start'} label="Презиме" mt={'sm'} placeholder='Петров' {...form.register('middleName')} withAsterisk
                error={form.formState.errors.middleName && form.formState.errors.middleName.message}/>
                
                <TextInput ta={'start'} label="Фамилно име" mt={'sm'} placeholder='Попов' {...form.register('lastName')} withAsterisk
                error={form.formState.errors.lastName && form.formState.errors.lastName.message}/>
                 */}
                <TextInput ta={'start'} label="Имейл" mt={'sm'} {...form.register('email')} withAsterisk
                    error={form.formState.errors.email && form.formState.errors.email.message} />

                {/* <TextInput ta={'start'} label='Телефон' mt={'sm'} placeholder='0881234567' {...form.register('phone')} withAsterisk
                error={form.formState.errors.phone && form.formState.errors.phone.message} /> */}

                <TextInput ta={'start'} hidden lightHidden darkHidden />

                <PasswordInput ta={'start'} label="Парола" mt="md" {...form.register('password')} withAsterisk
                    error={form.formState.errors.password && form.formState.errors.password.message} />

                <PasswordInput ta={'start'} label="Потвърди Парола" mt="md" {...form.register('confirmPassword')} withAsterisk
                    error={form.formState.errors.confirmPassword && form.formState.errors.confirmPassword.message} />
            </Box>
            <Button fullWidth mt="xl" type={'submit'} disabled={isLoading}>
                Регистрирай се
            </Button>
        </form>
    )
}
