'use client';

import { createProperty, PropertyTypeNameResult } from "@/actions/property";
import formReducer, { initialFormState } from "@/lib/formState";
import { zodResolver } from "@hookform/resolvers/zod";
import { Button, Group, LoadingOverlay, Modal, NumberInput, Stack, TextInput, Title, Text, Select } from "@mantine/core";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useReducer } from "react";
import { Controller, useForm } from "react-hook-form";
import { z } from "zod";

const propertySchema = z.object({
    floor: z.string().min(1, "Етаж е задължително поле"),
    number: z.string().min(1, "Номер е задължително поле"),
    propertyType: z.string()
        .nonempty('Видът на имота е задължително поле'),
    builtArea: z
      .number({
        required_error: "Застроената площ е задължително поле",
        invalid_type_error: "Застроената площ трябва да бъде число",
      })
      .positive("Застроената площ трябва да бъде положително число"),
    idealParts: z
      .number({
        required_error: "Идеалните части е задължително поле",
        invalid_type_error: "Идеалните части трябва да бъдат число",
      })
      .int("Идеалните части трябва да бъдат цяло число")
      .positive("Идеалните части трябва да бъдат положително число"),
  })
  
type PropertyFormValues = z.infer<typeof propertySchema>
  
interface PropertyModalProps {
    isOpen: boolean,
    onClose: () => void,
    propertyTypes: PropertyTypeNameResult,
}

export function PropertyCreationModal(
    { isOpen, onClose, propertyTypes }: 
    PropertyModalProps) {
    const { buildingId, entranceNumber } = 
        useParams<{ buildingId: string, entranceNumber: string }>();

    const [formState, dispatch] = useReducer(formReducer, initialFormState);

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
            window.location.reload();
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
    }
    const isLoading = formState === 'loading';
    const hasError = formState === 'error';
    return (
        <Modal 
        opened={isOpen} 
        onClose={onClose} 
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
                    <Button variant="outline" onClick={onClose}>
                    Cancel
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