import { PropertyFormValues, propertySchema } from "@/utils/validationSchemas";
import { zodResolver } from "@hookform/resolvers/zod";
import { Alert, Box, Button, Group, LoadingOverlay, NumberInput, Select, Stack, TextInput } from "@mantine/core";
import { Controller, useForm } from "react-hook-form";
import { usePropertyTypes } from "./propertyTypeProvider";
import { useEffect, useReducer } from "react";
import formReducer, { initialFormState } from "@/lib/formState";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import { getPropertyById, updateProperty } from "@/actions/property";
import { IconExclamationCircle } from "@tabler/icons-react";
import { queryKeys } from "@/types/queryKeys";

interface PropertyFormProps {
    onCancel?: () => void,
    propertyId: string,
}

export const useQueryPropertyById = (propertyId: string) => {
    return useQuery({ 
        queryKey: queryKeys.properties.details(propertyId),
        queryFn: () => getPropertyById(propertyId),
        enabled: !!propertyId,
    });
}

export default function PropertyForm({ onCancel, propertyId } : PropertyFormProps) {

    const queryClient = useQueryClient();
    const { propertyTypes } = usePropertyTypes();
    const [formState, dispatch] = useReducer(formReducer, initialFormState);
    const { data: propertyData, isLoading } = useQueryPropertyById(propertyId);
    
    const {
        control,
        handleSubmit,
        formState: { errors },
        reset,
        setError
    } = useForm<PropertyFormValues>({
        resolver: zodResolver(propertySchema),
        defaultValues: {
            floor: '',
            number: '',
            propertyType: '',
            builtArea: 0,
            idealParts: 0,
        }
    });
    
    useEffect(() => {
        if(propertyData?.ok) {
            reset({
                floor: propertyData.value?.floor,
                number: propertyData.value?.number,
                propertyType: propertyData.value?.propertyType,
                builtArea: propertyData.value?.builtArea,
                idealParts: propertyData.value?.idealParts,
            });
        }
        
    }, [propertyData, reset]);

    const handleFormSubmit = async(data: PropertyFormValues) => {
        dispatch({ type: 'SUBMIT' });
        
        const res = await updateProperty({
            ...data,
            id: propertyId,
        });
        
        console.log(res);

        if(!res.ok) {
            if(res.error.title === 'Properties.AlreadyExists') {
                setError('number', {
                    message: 'Имот с този номер вече съществува във входа',
                });
            }
            dispatch({ type: 'ERROR' });    
            return;
        }
        
        dispatch({ type: 'SUCCESS' });
        queryClient.invalidateQueries({
            queryKey: queryKeys.properties.details(propertyId),
        });

        queryClient.invalidateQueries({
            queryKey: queryKeys.properties.all,
        });
    }
    
    const isFormLoading = formState === 'loading';

    return (
        <>
            { 
                !isLoading && !propertyData?.ok ?
                <Alert
                variant="light"
                color="red"
                title="Грешка"
                icon={<IconExclamationCircle/>} mb={'md'}>
                    Грешка при четенето на информацията на имота!
                </Alert>
                :
                <></>
            }
            <form onSubmit={handleSubmit(handleFormSubmit)}>
                <Box pos={'relative'}>
                    <Stack gap="md">
                        <LoadingOverlay visible={isFormLoading || isLoading || !propertyData?.ok} zIndex={1000} overlayProps={{ radius: "sm", blur: 2 }} />
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
                    </Stack>
                </Box>

                <Group justify="flex-end" mt="xl">
                    {
                    onCancel !== undefined ?
                    <Button variant="outline" onClick={onCancel}>
                        Затвори
                    </Button>
                    :
                    <></>
                    }
                    <Button type="submit" disabled={isFormLoading || isLoading || !propertyData?.ok}>
                        Запази
                    </Button>
                </Group>
            </form>
        </>
    );
}