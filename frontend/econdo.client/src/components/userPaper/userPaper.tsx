import { UserProfileResult } from "@/actions/profile";
import { Avatar, Box, Group, Paper, Text } from "@mantine/core";

interface UserPaperProps {
    user: UserProfileResult,
}

export default function UserPaper({ user }: UserPaperProps) {

    return (
    <Paper p="md" withBorder>
        <Group justify="space-between">
        <Box>
            <Group align="center" gap="xs">
                <Avatar color='initials' name={`${user.firstName} ${user.lastName}`}/>
                <Text fw={500}>
                    {user.firstName} {user.middleName} {user.lastName}
                </Text>
            </Group>
            <Text size="sm" c="dimmed">
                {user.email}
            </Text>
        </Box>
        {/* <Group>
            <ActionIcon
            variant="light"
            color="blue"
            disabled={isDeleting}
            onClick={() => handleEdit(occupant)}
            aria-label="Edit occupant">
                <IconEdit size={16} />
            </ActionIcon>
            <ActionIcon
            variant="light"
            color="red"
            disabled={isDeleting}
            onClick={() => handleDelete(occupant.id)}
            aria-label="Delete occupant">
                <IconTrash size={16} />
            </ActionIcon>
        </Group> */}
        </Group>
    </Paper>
    );
}