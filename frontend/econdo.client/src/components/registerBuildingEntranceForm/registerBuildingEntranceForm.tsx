'use client';

import { zodResolver } from "@hookform/resolvers/zod";

import { 
    Box,
    LoadingOverlay,
    Button,
    TextInput,
    Select,
    Grid,
    GridCol,
    Text,
    Group
} from "@mantine/core";
import { useReducer } from "react";
import { Controller, useForm } from "react-hook-form";
import { z, ZodSchema } from "zod";
import { BuildingResult, ProvinceNameResult, RegisterBuilding, registerBuildingEntrance } from '@/actions/condo';
import formReducer, { initialFormState } from "@/lib/formState";

const RegisterBuildingSchema: ZodSchema<RegisterBuilding> = z
    .object({
        buildingName: z.string()
            .nonempty('Името на сградата е задължително поле'),
        provinceName: z.string()
            .nonempty('Областта е задължително поле'),
        municipality: z.string()
            .nonempty('Общината е задължително поле'),
        settlementPlace: z.string()
            .nonempty('Населеното място е задължително поле'),
        neighborhood: z.string()
            .nonempty('Кварталът е задължително поле'),
        postalCode: z.string()
            .nonempty('Пощенският код е задължително поле')
            .min(4, 'Пощенският код не е разпознат')
            .regex(/[\d\d\d\d]/, 'Пощенският код не е разпознат')
            .max(4, 'Пощенският код не е разпознат'),
        street: z.string()
            .nonempty('Улицата е задължително поле'),
        streetNumber: z.string()
            .nonempty('Номерът е задължително поле'),
        buildingNumber: z.string()
            .nonempty('Блокът е задължително поле'),
        entranceNumber: z.string()
            .nonempty('Входът е задължително поле'),
    });

interface BuildingEntraceFormProps {
    provinces: ProvinceNameResult,
    onSuccess?: () => void,
    onCancel?: () => void,
    building?: BuildingResult,
}

export default function RegisterBuildingEntranceForm(
    { provinces, onSuccess, onCancel, building, }: BuildingEntraceFormProps) {

    const isEditing = !!building;
    const [registerState, dispatch] =
        useReducer(formReducer, initialFormState);

    const form = useForm<RegisterBuilding>({
        defaultValues: building ? 
        {
            buildingName: building.name,
            provinceName: building.provinceName,
            municipality: building.municipality,
            settlementPlace: building.settlementPlace,
            neighborhood: building.neighborhood,
            postalCode: building.postalCode,
            street: building.street,
            streetNumber: building.streetNumber,
            buildingNumber: building.buildingNumber,
            entranceNumber: building.entranceNumber,
        }
        :
        {
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
        resolver: zodResolver(RegisterBuildingSchema),
    });

    const onSubmit = async (data: RegisterBuilding) => {
        dispatch({ type: 'SUBMIT' });

        const createResult = await registerBuildingEntrance(data);
        if(!createResult.ok) {
            dispatch({ type: 'ERROR' });
            return;
        }

        onSuccess && onSuccess();
        dispatch({ type: 'SUCCESS' });
    }

    const isLoading = registerState === 'loading';
    const hasError = registerState === 'error';

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
                                    error={form.formState.errors.provinceName && form.formState.errors.provinceName.message}
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

                { hasError ? <Text c={'red'} pt={10} fw={500} size={'sm'}>Грешка при {isEditing ? 'промяната' : 'добавянето'} на сградата! Моля пробвайте отново!</Text> : <></> }
            </Box>
            {
                !onCancel ? (
                <Button fullWidth mt="xl" type={'submit'} disabled={isLoading}>
                    Добави
                </Button>
                ) : (
                <Group justify="flex-end" gap={8}>
                    <Button variant="outline" onClick={onCancel}>
                        Затвори
                    </Button>
                    <Button mt="xl" type={'submit'} disabled={isLoading}>
                        { !isEditing ? 'Добави' : 'Запази' }
                    </Button>
                </Group>
                )
            }
        </form>
    )
}
