'use client';

import { createProfile } from "@/actions/profile";
import { isValidationError } from "@/types/apiResponses";
import { CreateProfileData } from "@/types/profileData";
import { firstNameSchema, lastNameSchema, middleNameSchema, phoneNumberSchema } from "@/utils/validationSchemas";
import { zodResolver } from "@hookform/resolvers/zod";
import { Box, Button, LoadingOverlay, Modal, TextInput, Text } from "@mantine/core";
import { useRouter } from "next/navigation";
import { useEffect, useReducer } from "react";
import { useForm } from "react-hook-form";
import { ZodSchema, z } from "zod";

const ProfileSchema: ZodSchema<CreateProfileData> = z
    .object({
        firstName: firstNameSchema,
        middleName: middleNameSchema,
        lastName: lastNameSchema,
        phoneNumber: phoneNumberSchema,
    });

type ProfileCreationState = 'idle' | 'loading' | 'error' | 'success';

type ProfileCreationAction = 
    | { type: 'START_CREATION' }
    | { type: 'CREATION_SUCCESS' }
    | { type: 'CREATION_ERROR' };

const initialProfileCreationState: ProfileCreationState = 'idle';

function profileCreationReducer(state: ProfileCreationState, action: ProfileCreationAction): ProfileCreationState {
    switch(action.type) {
        case 'START_CREATION':
            return 'loading';
        case 'CREATION_SUCCESS':
            return 'success';
        case 'CREATION_ERROR':
            return 'error';
        default:
            return state;
    }
}

export function ProfileCreationModal(
    { opened }: { opened: boolean }) {
    const [profileCreationState, dispatch] = useReducer(profileCreationReducer, initialProfileCreationState);
    const router = useRouter();

    const form = useForm<CreateProfileData>({
        defaultValues: {
            firstName: '',
            middleName: '',
            lastName: '',
            phoneNumber: '',
        },
        resolver: zodResolver(ProfileSchema),
    });

    useEffect(() => {
        if(profileCreationState === 'success')
            router.refresh();
    }, [profileCreationState]);

    const onSubmit = async (data: CreateProfileData) => {
        dispatch({ type: 'START_CREATION' });
        const res = await createProfile(data);
        if(isValidationError(res)) {
            console.error(res);
            dispatch({type: 'CREATION_ERROR'});
            return;
        }

        dispatch({type: 'CREATION_SUCCESS'});
    };

    const isLoading = profileCreationState === 'loading';

    return (
        <Modal onClose={() => { }} opened={opened} withCloseButton={false} centered title={<Text fw={'bold'}>Въведете лични ви данни</Text>}>
            <form onSubmit={form.handleSubmit(onSubmit)}>
                <Box>
                    <LoadingOverlay visible={isLoading} zIndex={1000} overlayProps={{ radius: "sm", blur: 2 }} />

                    <TextInput ta={'start'} label="Първо име" placeholder='Петър' {...form.register('firstName')} withAsterisk
                        error={form.formState.errors.firstName && form.formState.errors.firstName.message} />

                    <TextInput ta={'start'} label="Презиме" mt={'sm'} placeholder='Петров' {...form.register('middleName')} withAsterisk
                        error={form.formState.errors.middleName && form.formState.errors.middleName.message} />

                    <TextInput ta={'start'} label="Фамилно име" mt={'sm'} placeholder='Попов' {...form.register('lastName')} withAsterisk
                        error={form.formState.errors.lastName && form.formState.errors.lastName.message} />

                    <TextInput ta={'start'} label='Телефон' mt={'sm'} placeholder='0881234567' {...form.register('phoneNumber')} withAsterisk
                        error={form.formState.errors.phoneNumber && form.formState.errors.phoneNumber.message} />
                </Box>
                <Button fullWidth mt="xl" type={'submit'} disabled={isLoading}>
                    Запази
                </Button>
            </form>
        </Modal>
    );
}
