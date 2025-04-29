import { Paper, ThemeIcon, Title, Text } from "@mantine/core";

export function UserRoleCard({ icon: Icon, role, description }: 
    { icon: any, role: string, description: string }) {
    return (
    <Paper withBorder shadow="sm" radius="md" p="lg" ta="center" h={'100%'}>
        <ThemeIcon variant="light" size="xl" radius="xl" color="cyan" mb="md">
            <Icon size={36} />
        </ThemeIcon>
        
        <Title order={4}>{role}</Title>
        
        <Text c="dimmed" size="sm" mt="xs">
            {description}
        </Text>
    </Paper>
    );
}
