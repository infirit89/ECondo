import { OccupantFormValues, occupantSchema } from "@/utils/validationSchemas";
import { zodResolver } from "@hookform/resolvers/zod";
import { Button, Group, LoadingOverlay, Stack, TextInput, Text, Select } from "@mantine/core";
import { useReducer } from "react";
import { Controller, useForm } from "react-hook-form";
import { addOccupantToProperty, Occupant, OccupantTypeNameResult, updatePropertyOccupant } from "@/actions/propertyOccupant";
import formReducer, { initialFormState } from "@/lib/formState";
import { useQueryClient } from "@tanstack/react-query";
import { queryKeys } from "@/types/queryKeys";


interface OccupantFormProps {
    occupant?: Occupant,
    onCancel?: () => void,
    onSucess?: () => void,
    propertyId: string,
    occupantTypes: OccupantTypeNameResult,
}

export default function OccupantForm({
    occupant,
    onCancel,
    onSucess,
    propertyId,
    occupantTypes }: OccupantFormProps) {

    const isEditing = !!occupant;
    const {
        control,
        handleSubmit,
        setError,
        formState: { errors },
        reset,
    } = useForm<OccupantFormValues>({
        resolver: zodResolver(occupantSchema),
        defaultValues: occupant ?
            {
                firstName: occupant.firstName,
                middleName: occupant.middleName,
                lastName: occupant.lastName,
                email: occupant.email === null || occupant.email === undefined ? '' : occupant.email,
                occupantType: occupant.type,
            }
            :
            {
                firstName: '',
                middleName: '',
                lastName: '',
                email: '',
            },
    });

    const [formState, dispatch] = useReducer(formReducer, initialFormState);
    const queryClient = useQueryClient();

    const handleUpdate = async (data: OccupantFormValues) => {
        dispatch({ type: 'SUBMIT' });
        const email: string | null = !data.email ? null : data.email;

        if (!isEditing) {
            dispatch({ type: 'ERROR' });
            return;
        }

        const res = await updatePropertyOccupant({
            occupantId: occupant.id,
            firstName: data.firstName,
            middleName: data.middleName,
            lastName: data.lastName,
            email: email,
            type: data.occupantType,
        });

        if (!res.ok) {
            dispatch({ type: 'ERROR' });
            return;
        }

        dispatch({ type: 'SUCCESS' });

        queryClient.invalidateQueries({
            queryKey: queryKeys
                .occupants
                .inProperty(propertyId),
        });

        queryClient.invalidateQueries({
            queryKey: queryKeys
                .properties
                .all,
        });

        onSucess && onSucess();
    }

    const handleCreate = async (data: OccupantFormValues) => {
        dispatch({ type: 'SUBMIT' });

        const email: string | null = !data.email ? null : data.email;

        const res = await addOccupantToProperty({
            firstName: data.firstName,
            middleName: data.middleName,
            lastName: data.lastName,
            email: email,
            propertyId: propertyId,
            occupantType: data.occupantType,
        });

        if (!res.ok) {
            dispatch({ type: 'ERROR' });
            return;
        }

        dispatch({ type: 'SUCCESS' });

        queryClient.invalidateQueries({
            queryKey: queryKeys
                .occupants
                .inProperty(propertyId),
        });

        queryClient.invalidateQueries({
            queryKey: queryKeys
                .properties
                .all,
        });

        onSucess && onSucess();
    }

    const isLoading = formState === 'loading';
    const hasError = formState === 'error';
    return (
        <form onSubmit={handleSubmit(!isEditing ? handleCreate : handleUpdate)}>
            <Stack gap="md">
                <LoadingOverlay
                    visible={isLoading}
                    zIndex={1000}
                    overlayProps={{ radius: "sm", blur: 2 }} />
                <Controller
                    name="firstName"
                    control={control}
                    render={({ field }) => (
                        <TextInput
                            label="Първо име"
                            error={errors.firstName?.message}
                            withAsterisk
                            {...field}
                        />
                    )}
                />

                <Controller
                    name="middleName"
                    control={control}
                    render={({ field }) => (
                        <TextInput
                            label="Презиме"
                            error={errors.middleName?.message}
                            withAsterisk
                            {...field}
                        />
                    )}
                />

                <Controller
                    name="lastName"
                    control={control}
                    render={({ field }) => (
                        <TextInput
                            label="Фамилно име"
                            error={errors.lastName?.message}
                            withAsterisk
                            {...field}
                        />
                    )}
                />

                <Controller
                    name='occupantType'
                    control={control}
                    render={({ field }) => (
                        <Select
                            label="Тип"
                            error={errors.occupantType?.message}
                            withAsterisk
                            {...field}
                            data={occupantTypes.occupantTypes}
                            searchable
                        />
                    )}
                />

                <Controller
                    name="email"
                    control={control}
                    render={({ field }) => (
                        <TextInput
                            label="Имейл"
                            error={errors.email?.message}
                            {...field}
                        />
                    )}
                />

                {hasError ?
                    <Text
                        c={'red'}
                        pt={10}
                        fw={500}
                        size={'sm'}>
                        Грешка при {isEditing ? 'промяната' : 'добавянето'} на контакт! Моля пробвайте отново!
                    </Text> :
                    <></>}

                <Group justify='end' mt="md">
                    {
                        onCancel && (
                            <Button
                                variant="outline"
                                onClick={onCancel}>
                                Затвори
                            </Button>
                        )
                    }
                    <Button type="submit" disabled={isLoading}>
                        {isEditing ? 'Промени' : 'Добави'}
                    </Button>
                </Group>
            </Stack>
        </form>
    );
}
