'use client';

import { zodResolver } from "@hookform/resolvers/zod";

import { Box, LoadingOverlay, Button, TextInput, Select, Grid, GridCol } from "@mantine/core";
import { useReducer } from "react";
import { Controller, useForm } from "react-hook-form";
import { z, ZodSchema } from "zod";
import { ProvinceNameResult, RegisterBuilding } from '@/actions/condo';
import formReducer, { initialFormState } from "@/lib/formState";


const RegisterSchema: ZodSchema<RegisterBuilding> = z
    .object({
        buildingName: z.string(),
        provinceName: z.string(),
        municipality: z.string(),
        settlementPlace: z.string(),
        neighborhood: z.string(),
        postalCode: z.string(),
        street: z.string(),
        streetNumber: z.string(),
        buildingNumber: z.string(),
        entranceNumber: z.string(),
    });

export default function RegisterBuildingEntranceForm(
    { provinces }: { provinces: ProvinceNameResult }) {

    const [registerState, dispatch] =
        useReducer(formReducer, initialFormState);

    const form = useForm<RegisterBuilding>({
        defaultValues: {
            buildingName: '',
            provinceName: '',
            municipality: '',
            settlementPlace: '',
            neighborhood: '',
            postalCode: '',
            street: '',
            streetNumber: '',
            buildingNumber: '',
            entranceNumber: '',
        },
        resolver: zodResolver(RegisterSchema),
    });

    // useEffect(() => {
    //     if (registerState === 'success')
    //         redirect(`/login?event=${confirmAccountEvent}`);

    // }, [registerState]);

    const onSubmit = async (data: RegisterBuilding) => {
        dispatch({ type: 'SUBMIT' });
        // data.username = data.email;
        // const registerResult = await register({
        //     email: data.email,
        //     username: data.username,
        //     password: data.password,
        // });
        // if(!registerResult.ok && isValidationError(registerResult.error)) {
        //     form.setError(
        //         'email',
        //         {
        //             message: `Имейлът ${data.email} вече съществува`,
        //         },
        //     );
        //     dispatch({ type: 'REGISTER_ERROR' });
        //     return;
        // }

        dispatch({ type: 'SUCCESS' });
    }

    const isLoading = registerState === 'loading';

    return (
        <form onSubmit={form.handleSubmit(onSubmit)}>
            <Box pos={'relative'}>
                <LoadingOverlay visible={isLoading} zIndex={1000} overlayProps={{ radius: "sm", blur: 2 }} />

                <Grid>
                    <GridCol span={{ base: 12 }}>
                        <TextInput ta={'start'} label="Име на сградата" mt={'sm'} {...form.register('buildingName')} withAsterisk
                            error={form.formState.errors.buildingName && form.formState.errors.buildingName.message} />
                    </GridCol>

                    <GridCol span={{base: 12, sm: 3}}>
                        <Controller
                            render={({field}) => (
                                <Select
                                    label="Област"
                                    mt={'sm'}
                                    {...field}
                                    data={provinces.provinces}
                                    searchable
                                    withAsterisk
                                />
                            )}
                            control={form.control}
                            name='provinceName'>
                        </Controller>
                    </GridCol>
                    <GridCol span={{base: 12, sm: 3}}>
                        <TextInput ta={'start'} label="Община" mt={'sm'} {...form.register('municipality')} withAsterisk
                            error={form.formState.errors.municipality && form.formState.errors.municipality.message} />
                    </GridCol>
                
                    <GridCol span={{base: 12, sm: 3}}>
                        <TextInput ta={'start'} label="Населено място" mt={'sm'} {...form.register('settlementPlace')} withAsterisk
                            error={form.formState.errors.settlementPlace && form.formState.errors.settlementPlace.message} />
                    </GridCol>

                    <GridCol span={{base: 12, sm: 3}}>
                        <TextInput ta={'start'} label="Квартал" mt={'sm'} {...form.register('neighborhood')} withAsterisk
                            error={form.formState.errors.neighborhood && form.formState.errors.neighborhood.message} />
                    </GridCol>

                    <GridCol span={{base: 8, xs: 10, sm: 3}}>
                        <TextInput ta={'start'} label="Улица" mt={'sm'} {...form.register('street')} withAsterisk
                            error={form.formState.errors.street && form.formState.errors.street.message} />
                    </GridCol>
                    
                    <GridCol span={{base: 4, xs: 2}}>
                        <TextInput ta={'start'} label="Номер" mt={'sm'} {...form.register('streetNumber')} withAsterisk
                            error={form.formState.errors.streetNumber && form.formState.errors.streetNumber.message} />
                    </GridCol>
                    
                    <GridCol span={{base: 3, sm: 2}}>
                        <TextInput ta={'start'} label="Блок" mt={'sm'} {...form.register('buildingNumber')} withAsterisk
                            error={form.formState.errors.buildingNumber && form.formState.errors.buildingNumber.message} />
                    </GridCol>

                    <GridCol span={{base: 3, sm: 2}}>
                        <TextInput ta={'start'} label="Вход" mt={'sm'} {...form.register('entranceNumber')} withAsterisk
                            error={form.formState.errors.entranceNumber && form.formState.errors.entranceNumber.message} />
                    </GridCol>

                    <GridCol span={{base: 6, sm: 3}}>
                        <TextInput ta={'start'} label="Пощенски код" mt={'sm'} {...form.register('postalCode')} withAsterisk
                            error={form.formState.errors.postalCode && form.formState.errors.postalCode.message} />
                    </GridCol>
                </Grid>

            </Box>
            <Button fullWidth mt="xl" type={'submit'} disabled={isLoading}>
                Добави
            </Button>
        </form>
    )
}
