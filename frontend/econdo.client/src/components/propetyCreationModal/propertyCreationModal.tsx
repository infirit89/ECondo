'use client';

import { createProperty } from "@/actions/property";
import { usePropertyTypes } from "@/app/(user)/(dashboard)/buildings/[buildingId]/[entranceNumber]/[activeTab]/@properties/propertyTypeProvider";
import formReducer, { initialFormState } from "@/lib/formState";
import { queryKeys } from "@/types/queryKeys";
import { propertySchema } from "@/utils/validationSchemas";
import { zodResolver } from "@hookform/resolvers/zod";
import { 
    Button,
    Group,
    LoadingOverlay,
    Modal,
    NumberInput,
    Stack,
    TextInput,
    Text,
    Select
} from "@mantine/core";
import { useQueryClient } from "@tanstack/react-query";
import { useParams } from "next/navigation";
import { useEffect, useReducer } from "react";
import { Controller, useForm } from "react-hook-form";
import { z } from "zod";

type PropertyFormValues = z.infer<typeof propertySchema>
  
interface PropertyModalProps {
    isOpen: boolean,
    onClose: () => void,
}

export function PropertyCreationModal({ 
    isOpen, 
    onClose, }: PropertyModalProps) {
    const { buildingId, entranceNumber } = 
        useParams<{ buildingId: string, entranceNumber: string }>();

    const { propertyTypes } = usePropertyTypes();
    const [formState, dispatch] = 
        useReducer(formReducer, initialFormState);

    const queryClient = useQueryClient();

    const {
        control,
        handleSubmit,
        setError,
        formState: { errors },
        reset,
        } = useForm<PropertyFormValues>({
        resolver: zodResolver(propertySchema),
        defaultValues: {
            floor: "",
            number: "",
            builtArea: 0,
            idealParts: 1,
        },
        });

    useEffect(() => {
        if(formState === 'success') {
            reset();
            onClose();
        }
    }, [formState]);

    const handleFormSubmit = async (data: PropertyFormValues) => {
        dispatch({ type: 'SUBMIT' });

        const createResult = 
            await createProperty({
                ...data,
                buildingId: buildingId,
                entranceNumber: entranceNumber,
            });

        if(!createResult.ok) {
            if(createResult.error.title === 
                'Properties.AlreadyExists') {
                    setError('number', {
                        message: 'Имот с този номер вече съществува във входа',
                    });
                }

            dispatch({ type: 'ERROR' });
            return;
        }
        
        dispatch({type: 'SUCCESS'});
        queryClient.invalidateQueries({
            queryKey: queryKeys.properties.all,
        });
    }
    const isLoading = formState === 'loading';
    const hasError = formState === 'error';
    return (
        <Modal 
        opened={isOpen} 
        onClose={!isLoading ? onClose : () => {}} 
        title={<Text>Нов имот</Text>} size="md" centered>
        <form onSubmit={handleSubmit(handleFormSubmit)}>
            <Stack gap="md">
                <LoadingOverlay visible={isLoading} zIndex={1000} overlayProps={{ radius: "sm", blur: 2 }} />
                <Controller
                    name="floor"
                    control={control}
                    render={({ field }) => (
                    <TextInput
                        label="Етаж"
                        error={errors.floor?.message}
                        withAsterisk
                        {...field}
                    />
                    )}
                />

                <Controller
                    name="number"
                    control={control}
                    render={({ field }) => (
                    <TextInput
                        label="Номер"
                        error={errors.number?.message}
                        withAsterisk
                        {...field}
                    />
                    )}
                />

                <Controller
                    name='propertyType'
                    control={control}
                    render={({ field }) => (
                    <Select
                        label="Вид"
                        error={errors.propertyType?.message}
                        withAsterisk
                        {...field}
                        data={propertyTypes.propertyTypes}
                        searchable
                    />
                    )}
                />
                
                <Controller
                    name="builtArea"
                    control={control}
                    render={({ field }) => (
                    <NumberInput
                        label="Застроена площ (m²)"
                        error={errors.builtArea?.message}
                        withAsterisk
                        min={0}
                        decimalScale={2}
                        step={0.5}
                        {...field}
                    />
                    )}
                />

                <Controller
                    name="idealParts"
                    control={control}
                    render={({ field }) => (
                    <NumberInput
                        label="Идеални части"
                        error={errors.idealParts?.message}
                        withAsterisk
                        min={1}
                        allowDecimal={false}
                        {...field}
                    />
                    )}
                />

                { hasError ? 
                <Text 
                c={'red'} 
                pt={10} 
                fw={500} 
                size={'sm'}>
                    Грешка при създаването на имот! Моля пробвайте отново!
                </Text> :
                <></> }
                <Group justify='end' mt="md">
                    <Button variant="outline" disabled={isLoading} onClick={onClose}>
                    Отмени
                    </Button>
                    <Button type="submit" disabled={isLoading}>
                        Добави имот
                    </Button>
                </Group>
            </Stack>
        </form>
        </Modal>
    );
}