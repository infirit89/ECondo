'use client';

import { Paper, TextInput } from "@mantine/core";

import { 
    Box,
    LoadingOverlay,
    PasswordInput,
    Group,
    Checkbox,
    Anchor,
    Button,
    Text } from "@mantine/core";
import { useEffect, useReducer } from "react";
import { useForm } from "react-hook-form";
import { UserErrorCode } from "@/types/apiErrors";
import { login } from "@/actions/auth";
import { redirect } from "next/navigation";
import formReducer, { initialFormState } from "@/lib/formState";
import { z, ZodSchema } from "zod";
import { emailSchema } from "@/utils/validationSchemas";
import { zodResolver } from "@hookform/resolvers/zod";
import { useQueryClient } from "@tanstack/react-query";
import { motion } from "framer-motion";

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
    const queryClient = useQueryClient();
 
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
        if(!res.ok) {
            if(res.error.title === UserErrorCode.NotFound || 
                res.error.title === 'Validation.General') {
                form.setValue('password', '');
                dispatch({type: 'ERROR'});
                return;
            }
        }

        dispatch({type: 'SUCCESS'});
        queryClient.clear();
    }

    const isLoading = loginState === 'loading';
    const hasError = loginState === 'error';

    return (
    <motion.div
      initial={{ opacity: 0, y: 40 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.6, ease: 'easeOut' }}>
        <Paper withBorder shadow="md" p={30} mt={30} radius="md">
            <form onSubmit={form.handleSubmit(onSubmit)}>
                <Box pos={'relative'}>
                    <LoadingOverlay visible={isLoading} zIndex={1000} overlayProps={{ radius: "sm", blur: 2 }}/>
                    <TextInput 
                    ta={'start'} 
                    label="Имейл" 
                    required {...form.register('email')} 
                    withAsterisk
                    error={form.formState.errors.email && form.formState.errors.email.message}
                    styles={{
                        input: { transition: 'border-color 0.2s ease' },
                    }}/>
                    
                    <PasswordInput 
                    ta={'start'} 
                    label="Парола" 
                    required 
                    mt="md" 
                    {...form.register('password')} 
                    withAsterisk
                    error={form.formState.errors.password && form.formState.errors.password.message} 
                    styles={{
                        input: { transition: 'border-color 0.2s ease' },
                    }}/>

                    { hasError ? <Text c={'red'} pt={10} fw={500} size={'sm'}>Потребителското име или паролата са неправилни!</Text> : <></> }
                    <Group justify="space-between" mt="lg">
                        <Checkbox label="Запомни ме" />
                        <Anchor size="sm" href="/forgotPassword">
                            Забравена парола?
                        </Anchor>
                    </Group>
                </Box>
                <motion.div whileHover={{ scale: 1.03 }} whileTap={{ scale: 0.98 }}>
                    <Button
                    fullWidth 
                    mt="xl" 
                    type={'submit'} 
                    disabled={isLoading}
                    size="md"
                    style={{
                        background: 'linear-gradient(135deg, #228be6, #15aabf)',
                        transition: 'background 0.3s ease',
                    }}>
                        Влез
                    </Button>
                </motion.div>
            </form>
        </Paper>
    </motion.div>
    )
}