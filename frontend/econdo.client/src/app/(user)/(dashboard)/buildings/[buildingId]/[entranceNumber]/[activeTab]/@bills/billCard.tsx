import { RecurringInterval } from "@/types/recurringInterval";
import { formatCurrency } from "@/utils/currency";
import { ActionIcon, Badge, Card, CardSection, Group, Paper, Space, Stack, Text } from "@mantine/core";
import { IconAlignLeft, IconCalendar, IconCalendarDue, IconCash, IconEdit, IconTrash } from "@tabler/icons-react";

export interface BillProps {
    title: string
    description?: string
    amount: number
    startDate: string
    endDate?: string
    interval?: RecurringInterval
}

export default function BillCard({
    title,
    description,
    amount,
    startDate,
    endDate,
    interval,
}: BillProps) {
    return (
        <Card withBorder p="lg" shadow="sm" radius={'lg'}>
            <CardSection inheritPadding py={'xs'} withBorder>
                <Group justify='space-between' mb="xs">
                    <Text fw={600} lineClamp={1}>{title}</Text>
                    <Group gap="xs">
                        <ActionIcon variant="subtle" color="blue" >
                            <IconEdit size={18} />
                        </ActionIcon>
                        <ActionIcon variant="subtle" color="red" >
                            <IconTrash size={18} />
                        </ActionIcon>
                    </Group>
                </Group>
            </CardSection>

            <CardSection my={'md'} withBorder inheritPadding pb={'md'}>
                <Group gap="xs" wrap="nowrap" align="flex-start">
                    <IconAlignLeft size={16} color="gray" style={{ marginTop: 2 }} />
                    {description ? (
                        <Text size="sm" c="dimmed">
                            {description}
                        </Text>
                    ) : (
                        <Text size="sm" c="dimmed" fs="italic">
                            No description
                        </Text>
                    )}
                </Group>
            </CardSection>
            <Space h="sm" />
            <Stack gap="xs">
                <Group gap={'xs'}>
                    <IconCash size={16} color="gray" />
                    <Text size="sm">
                        <Text span fw={500}>
                            Сума:
                        </Text>{' '}
                        {formatCurrency(amount)}
                    </Text>
                </Group>
                <Group gap={'xs'}>
                    <IconCalendar size={16} color="gray" />
                    <Text size="sm">
                        <Text span fw={500}>
                            Начална дата:
                        </Text>{' '}
                        {new Date(startDate).toLocaleDateString()}
                    </Text>
                </Group>
                <Group gap={'xs'}>
                    <IconCalendarDue size={16} color="gray" />
                    <Text size="sm">
                        <Text span fw={500}>
                            Крайна дата:
                        </Text>{' '}
                        {endDate ?
                            new Date(endDate).toLocaleDateString() :
                            'Няма'}
                    </Text>
                </Group>
            </Stack>
        </Card>
    )
}
