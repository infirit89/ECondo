import { Avatar, Card, Divider, Group, Text } from "@mantine/core";
import { IconQuote } from "@tabler/icons-react";

export function TestimonialCard({ name, role, testimony }: { name: string, role: string, testimony: string }) {
    return (
      <Card withBorder radius="md" shadow="sm" p="xl" h={'100%'}>
        <Group mb="md">
          <Avatar radius="xl" name={name} color='initials' />
          <div>
            <Text fw={700}>{name}</Text>
            <Text size="xs" c="dimmed">{role}</Text>
          </div>
        </Group>
        <Divider mb="md" />
        <Group align="flex-start">
          <IconQuote size={32} color="gray" />
          <Text size="sm" c="dimmed">{testimony}</Text>
        </Group>
      </Card>
    );
}