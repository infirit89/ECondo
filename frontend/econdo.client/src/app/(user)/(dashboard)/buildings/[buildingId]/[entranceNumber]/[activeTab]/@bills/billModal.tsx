import { createBill } from "@/actions/condo";
import formReducer, { initialFormState } from "@/lib/formState";
import { RecurringInterval } from "@/types/recurringInterval";
import { zodResolver } from "@hookform/resolvers/zod";
import { Box, Button, Checkbox, Group, LoadingOverlay, Modal, NumberInput, Select, Stack, Text, TextInput } from "@mantine/core";
import { DatePickerInput } from "@mantine/dates";
import { useReducer } from "react";
import { Controller, useForm } from "react-hook-form";
import { z } from "zod";

const billSchema = z.object({
    title: z.string().nonempty(),
    description: z.string().optional().or(z.literal('')),
    amount: z.number().positive(),
    isRecurring: z.boolean(),
    recurringInterval: z.string().optional(),
    startDate: z.date(),
    endDate: z.date().optional(),
});

type BillFormValues = z.infer<typeof billSchema>;

interface BillModalProps {
    isOpen: boolean,
    onClose: () => void,
    buildingId: string,
    entranceNumber: string,
}

export default function BillModal({ isOpen, onClose, buildingId, entranceNumber } : 
    BillModalProps) {
    const form = useForm<BillFormValues>({
        defaultValues: {
            title: '',
            description: '',
            amount: 0,
            isRecurring: false,
            recurringInterval: '',
            startDate: new Date(),
            endDate: undefined,
        },
        resolver: zodResolver(billSchema),
    });

    const [formState, dispatch] = useReducer(formReducer, initialFormState);
    const showRecurringInterval = form.watch('isRecurring');
    const handleSubmit = async(data: BillFormValues) => {
        dispatch({type: 'SUBMIT'});
        let recurringInterval: RecurringInterval | undefined = undefined;
        const endDate: Date | undefined = !data.endDate ? undefined : data.endDate;
        const description: string | undefined = !data.description ? undefined : data.description;

        switch(data.recurringInterval) {
            case 'Месечно':
                recurringInterval = RecurringInterval.Monthly;
            case 'Годишно':
                recurringInterval = RecurringInterval.Yearly;
            default:
                recurringInterval = undefined;
        }

        const res = await createBill(
            buildingId, 
            entranceNumber, 
            data.title, 
            data.amount, 
            data.isRecurring, 
            data.startDate.toISOString(),
            description, 
            recurringInterval, 
            endDate?.toISOString());

        if(!res.ok) {
            if(res.error.errors && res.error.errors['LessThanValidator'] !== undefined) {
                form.setError('endDate', {
                    message: 'Крайната дата трябва да е след началната дата',
                });
            }
            dispatch({type: 'ERROR'});
        }

        dispatch({type: 'SUCCESS'});
    }

    const isLoading = formState === 'loading';
    return (
        <Modal
        opened={isOpen}
        onClose={onClose}
        title={<Text>Нов Разход</Text>} size="md" centered>
            <form onSubmit={form.handleSubmit(handleSubmit)}>
                <Box pos={'relative'}>
                    <Stack gap="md">
                    
                    <LoadingOverlay visible={isLoading} zIndex={1000} overlayProps={{ radius: "sm", blur: 2 }} />
                    <TextInput label="Име" 
                    {...form.register("title")} 
                    withAsterisk
                    error={form.formState.errors.title?.message}/>
                    <TextInput 
                    label="Описание" 
                    {...form.register("description")} 
                    error={form.formState.errors.description?.message}/>

                    <Controller
                        name='amount'
                        control={form.control}
                        render={({ field }) => (
                        <NumberInput
                        label="Количество" 
                        withAsterisk
                        {...field}
                        min={0}
                        decimalScale={2}
                        error={form.formState.errors.amount?.message}/>
                        )}
                    />

                    <Checkbox
                    label="Повтаряща се?"
                    {...form.register('isRecurring')}
                    />

                    {showRecurringInterval && (
                        <Controller
                            name='recurringInterval'
                            control={form.control}
                            render={({ field }) => (
                            <Select
                                label="Интервал на повтаряне"
                                error={form.formState.errors.recurringInterval?.message}
                                withAsterisk
                                {...field}
                                data={['Месечно', 'Годишно']}
                                searchable
                            />
                            )}
                        />
                    )}

                    <Controller
                        name="startDate"
                        control={form.control}
                        render={({ field }) => (
                            <DatePickerInput
                            label="Начална дата"
                            withAsterisk
                            {...field}
                            error={form.formState.errors.startDate?.message}/>
                        )}
                    />

                    <Controller
                        name="endDate"
                        control={form.control}
                        render={({ field }) => (
                            <DatePickerInput
                            label="Крайна дата"
                            {...field}
                            error={form.formState.errors.endDate?.message}/>
                        )}
                    />
                    </Stack>
                </Box>

                <Group justify="flex-end" mt="md">
                    <Button type="submit" disabled={isLoading}>Създай</Button>
                </Group>
            </form>
        </Modal>
    )
}