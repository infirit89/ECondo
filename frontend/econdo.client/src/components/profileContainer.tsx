'use client'; 

import { Alert, Divider, Grid, GridCol, Paper } from "@mantine/core";
import UpdatePasswordForm from "./updatePasswordForm";
import { useReducer } from "react";
import UpdateProfileForm from "./updateProfileForm";

type ProfileState = 'idle' | 'updatedPassword' | 'updateProfile';

type ProfileAction =
    | { type: 'UPDATE_PASSWORD' }
    | { type: 'UPDATE_PROFILE' }
    | { type: 'SET_IDLE' };

const initialProfileState: ProfileState = 'idle';

function profileReducer(state: ProfileState, action: ProfileAction): ProfileState {
    switch(action.type) {
        case 'SET_IDLE':
            return 'idle';
        case 'UPDATE_PASSWORD':
            return 'updatedPassword';
        case 'UPDATE_PROFILE':
            return 'updateProfile';
        default:
            return state;
    }
}

export default function ProfileContainer() {
    const [profileState, dispatch] = useReducer(profileReducer, initialProfileState);

    const onUpdatePasswordSuccess = () => {
        dispatch({ type: 'UPDATE_PASSWORD' });

        setTimeout(() => {
            dispatch({ type: 'SET_IDLE' });
        }, 1000 * 10);
    }

    const onUpdateProfileSuccess = () => {
        dispatch({ type: 'UPDATE_PROFILE' });

        setTimeout(() => {
            dispatch({ type: 'SET_IDLE' });
        }, 1000 * 10);
    }

    return (
        <>
            <Alert withCloseButton variant="light" color={'teal'} onClose={() => { dispatch({type: 'SET_IDLE'}) }} hidden={ profileState !== 'updatedPassword' }>
                Паролата Ви беше променена успешно!
            </Alert>
            <Alert withCloseButton variant="light" color={'teal'} onClose={() => { dispatch({type: 'SET_IDLE'}) }} hidden={ profileState !== 'updateProfile' }>
                Личните Ви данни бяха променени успешно!
            </Alert>
            <Paper withBorder shadow="md" p={30} mt={30} radius="md">
                <Grid gutter={'xl'}>
                    <GridCol span={{ base: 12, sm: 6 }}>
                        <Divider label='Лична информация'/>
                        <UpdateProfileForm onSuccess={onUpdateProfileSuccess}/>
                    </GridCol>
                    <GridCol span={{ base: 12, sm: 6 }}>
                        <Divider label='Промени парола'/>
                        <UpdatePasswordForm onSuccess={onUpdatePasswordSuccess}/>
                    </GridCol>
                </Grid>
            </Paper>
        </>
    );
}