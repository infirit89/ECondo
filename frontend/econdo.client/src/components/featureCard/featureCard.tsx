import { ThemeIcon, Title, Text, Paper } from "@mantine/core";

export function FeatureCard({ icon: Icon, title, description }: 
    { icon: any, title: string, description: string }) {
    return (
    <Paper withBorder radius="md" shadow="sm" p="lg" ta="center" h={'100%'}>
      <ThemeIcon
      variant="light"
      size="xl"
      radius="xl"
      color="blue"
      mb="md"
      ta={'center'}>
        <Icon size={36} />
      </ThemeIcon>

      <Title order={4}>{title}</Title>
      
      <Text c="dimmed" size="sm" mt="xs">
        {description}
      </Text>

    </Paper>
    );
}
