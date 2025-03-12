'use client';

import { zodResolver } from "@hookform/resolvers/zod";
import { Box, Button, LoadingOverlay, TextInput } from "@mantine/core";
import { useEffect, useReducer, useState } from "react";
import { useForm } from "react-hook-form";
import { z, ZodSchema } from "zod";
import { ProfileDetails } from "../_data/profileData";
import { getProfile, updateProfile } from "@/actions/profile";

interface ProfileFormFields {
    firstName: string;
    middleName: string;
    lastName: string;
}

const UpdateProfileSchema : ZodSchema<ProfileFormFields> = z
    .object({
        firstName: z.string().min(1, 'Първо име е задължително поле'),
        middleName: z.string().min(1, 'Презиме е задължително поле'),
        lastName: z.string().min(1, 'Фамилно име е задължително поле'),
    });

type UpdateProfileState = 'idle' | 'loading' | 'error' | 'success';

type UpdateProfileAction = 
    | { type: 'START_UPDATE' }
    | { type: 'UPDATE_SUCESS' }
    | { type: 'UPDATE_ERROR' };

const initialProfileFormState: UpdateProfileState = 'idle';

function profileUpdateReducer(state: UpdateProfileState, action: UpdateProfileAction): UpdateProfileState {
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

export default function UpdateProfileForm({ onSuccess } : { onSuccess?: () => void }) {
    const [profileFormState, dispatch] = useReducer(profileUpdateReducer, initialProfileFormState);
    const [profileDetails, setProfileDetails] = useState<ProfileDetails | null>(null);
    
    const form = useForm<ProfileFormFields>({
        defaultValues: {
            firstName: '',
            middleName: '',
            lastName: '',
        },
        resolver: zodResolver(UpdateProfileSchema),
    });

    useEffect(() => {
        if(profileDetails)
            return;

        getProfile()
        .then(x => {
            if(x.ok) {
                setProfileDetails(x.value);
                form.setValue('firstName', x.value.firstName);
                form.setValue('middleName', x.value.middleName);
                form.setValue('lastName', x.value.lastName);

                return;
            }

            console.error(x.error);
        });
    }, [profileDetails]);


    const onSubmit = async(data: ProfileFormFields) => {
        if(data.firstName === profileDetails?.firstName 
            && data.middleName === profileDetails.middleName 
            && data.lastName === profileDetails.lastName) {
                return;
        }
        
        dispatch({ type: 'START_UPDATE' });

        const res = await updateProfile(data);
        if(!res.ok) {
            console.error(res.error);
            dispatch({ type: 'UPDATE_ERROR' });
            return;
        }

        if(onSuccess)
            onSuccess();

        setProfileDetails(data);
        dispatch({ type: 'UPDATE_SUCESS' });
    }

    const isLoading = profileFormState === 'loading' || !profileDetails;

    return (
        <form onSubmit={form.handleSubmit(onSubmit)}>
            <Box pos={'relative'}>
                <LoadingOverlay visible={isLoading} zIndex={1000} overlayProps={{ radius: "sm" }}/>

                <TextInput ta={'start'} label="Първо име" placeholder='Петър' {...form.register('firstName')} withAsterisk
                    error={form.formState.errors.firstName && form.formState.errors.firstName.message} />
                
                <TextInput ta={'start'} label="Презиме" mt={'sm'} placeholder='Петров' {...form.register('middleName')} withAsterisk
                    error={form.formState.errors.middleName && form.formState.errors.middleName.message} />

                <TextInput ta={'start'} label="Фамилно име" mt={'sm'} placeholder='Попов' {...form.register('lastName')} withAsterisk
                    error={form.formState.errors.lastName && form.formState.errors.lastName.message} />
            </Box>
            <Button fullWidth mt="xl" type={'submit'} disabled={isLoading}>
                Запази
            </Button>
        </form>
    );
}
