'use client';

import { TextInput } from "@mantine/core";

import { Box, LoadingOverlay, PasswordInput, Group, Checkbox, Anchor, Button, Text } from "@mantine/core";
import { useEffect, useReducer } from "react";
import { useForm } from "react-hook-form";
import { isValidationError } from "@/types/apiResponses";
import { login } from "@/actions/auth";
import { redirect } from "next/navigation";

interface LoginFormFields {
    email: string;
    password: string;
}

type LoginState = 'idle' | 'loading' | 'error' | 'success';

type LoginAction = 
    | { type: 'START_LOGIN' }
    | { type: 'LOGIN_SUCESS' }
    | { type: 'LOGIN_ERROR' };

const initialLoginState: LoginState = 'idle';

function loginReducer(state: LoginState, action: LoginAction): LoginState {
    switch(action.type) {
        case 'START_LOGIN':
            return 'loading';
        case 'LOGIN_SUCESS':
            return 'success';
        case 'LOGIN_ERROR':
            return 'error';
        default:
            return state;
    }
}

export default function LoginForm() {
    const [loginState, dispatch] = useReducer(loginReducer, initialLoginState);
 
    useEffect(() => {
        if(loginState === 'success') 
            redirect('/dashboard');
    }, [loginState]);

    const form = useForm<LoginFormFields>({
        defaultValues: {
            email: '',
            password: '',
        },
    });
    
    const onSubmit = async(data: LoginFormFields) => {
        dispatch({ type: 'START_LOGIN' });
        const res = await login(data);
        console.log('AAAAAAAAAAAAA', res);
        if(!res.ok) {
            if(isValidationError(res.error)) {
                form.setValue('password', '');
                dispatch({type: 'LOGIN_ERROR'});
                return;
            }
        }

        dispatch({type: 'LOGIN_SUCESS'});
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