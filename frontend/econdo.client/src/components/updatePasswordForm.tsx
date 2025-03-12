'use client';

import { updatePassword } from "@/actions/auth";
import { passwordSchema } from "@/utils/validationSchemas";
import { zodResolver } from "@hookform/resolvers/zod";
import { Box, Button, LoadingOverlay, PasswordInput } from "@mantine/core";
import { useReducer } from "react";
import { useForm } from "react-hook-form";
import { z, ZodSchema } from "zod";
import { isValidationError } from "@/types/apiResponses";

interface PasswordFormFields {
    currentPassword: string;
    newPassword: string;
    confirmPassword: string;
}

const UpdatePasswordSchema : ZodSchema<PasswordFormFields> = z
    .object({
        currentPassword: z.string().min(1, 'Стара парола е задължително поле'),
        newPassword: passwordSchema,
        confirmPassword: z.string(),
    })
    .refine((data) =>
        {
            return data.newPassword === data.confirmPassword
        }, {
        message: "Паролите не съвпадат",
        path: ["confirmPassword"], // path of error
    });

type UpdatePasswordState = 'idle' | 'loading' | 'error' | 'success';

type UpdatePasswordAction = 
    | { type: 'START_UPDATE' }
    | { type: 'UPDATE_SUCESS' }
    | { type: 'UPDATE_ERROR' };

const initialPasswordFormState: UpdatePasswordState = 'idle';

function passwordUpdateReducer(state: UpdatePasswordState, action: UpdatePasswordAction): UpdatePasswordState {
    switch(action.type) {
        case 'START_UPDATE':
            return 'loading';
        case 'UPDATE_SUCESS':
            return 'success';
        case 'UPDATE_ERROR':
            return 'error';
        default:
            return state;
    }
}

export default function UpdatePasswordForm({ onSuccess } : {onSuccess?: () => void}) {

    const [passwordFormState, dispatch] = useReducer(passwordUpdateReducer, initialPasswordFormState);
     
    const form = useForm<PasswordFormFields>({
        defaultValues: {
            currentPassword: '',
            newPassword: '',
            confirmPassword: ''
        },
        resolver: zodResolver(UpdatePasswordSchema),
    });

    const onSubmit = async(data: PasswordFormFields) => {
        dispatch({ type: 'START_UPDATE' });

        const res = await updatePassword(data.currentPassword, data.newPassword);

        if(!res.ok) {
            form.setValue('newPassword', '');
            form.setValue('confirmPassword', '');

            if(isValidationError(res.error) && res.error.errors['PasswordMismatch']) {
                form.setError('currentPassword', {
                    message: 'Грешна парола',
                });
            }

            dispatch({ type: 'UPDATE_ERROR' });
            return;
        }

        if(onSuccess)
            onSuccess();

        form.reset();
        dispatch({ type: 'UPDATE_SUCESS' });
    }
    
    const isLoading = passwordFormState === 'loading';
    return (
        <form onSubmit={form.handleSubmit(onSubmit)}>
            <Box pos={'relative'}>
                <LoadingOverlay visible={isLoading} zIndex={1000} overlayProps={{ radius: "sm", blur: 2 }}/>
                <PasswordInput ta={'start'} label={'Стара парола'} placeholder="" {...form.register('currentPassword', {
                    required: true,
                })} withAsterisk
                error={form.formState.errors.currentPassword && form.formState.errors.currentPassword.message}/>

                <PasswordInput ta={'start'} label={'Нова парола'} mt={"md"} placeholder="" {...form.register('newPassword', {
                    required: true,
                })} withAsterisk
                error={form.formState.errors.newPassword && form.formState.errors.newPassword.message}/>

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