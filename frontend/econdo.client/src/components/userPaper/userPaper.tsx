'use client';

import { deleteUser } from "@/actions/auth";
import { UserProfileResult } from "@/actions/profile";
import { queryKeys } from "@/types/queryKeys";
import { ActionIcon, Avatar, Box, Group, Paper, Text } from "@mantine/core";
import { useModals } from "@mantine/modals";
import { IconTrash } from "@tabler/icons-react";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useState } from "react";

interface UserPaperProps {
    user: UserProfileResult,
}

export default function UserPaper({ user }: UserPaperProps) {

    const [isDeleting, setIsDeleting] = useState(false);

    const modals = useModals();
    const queryClient = useQueryClient();

    const useDeleteUser = () => {
        return useMutation({
            mutationFn: (email: string) => deleteUser(email),
            onSuccess: (data) => {
                if(!data.ok) {
                    // onDeleteError && onDeleteError();
                    setIsDeleting(false);
                    return;
                }
                
                queryClient.invalidateQueries({
                    queryKey: queryKeys.profiles.all,
                });

                setIsDeleting(false);
            },
            onMutate: () => {
                setIsDeleting(true);
            }
        });
    }

    const { mutate: deleteUserMutation } = useDeleteUser();

    const handleDelete = () => {
        const modalId = modals.openConfirmModal({
            title: "Потвърди изтриване",
            children: <Text size="sm">Сигурни ли сте, че искате да изтриете този потребител?</Text>,
            labels: { confirm: "Изтрий", cancel: "Отмяна" },
            confirmProps: { color: "red" },
            onConfirm: () => {
                deleteUserMutation(user.email);
                modals.closeModal(modalId);
            },
        })
    }

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
        <Group>
            {/* <ActionIcon
            variant="light"
            color="blue"
            disabled={isDeleting}
            onClick={() => handleEdit(occupant)}
            aria-label="Edit occupant">
                <IconEdit size={16} />
            </ActionIcon> */}
            <ActionIcon
            variant="light"
            color="red"
            disabled={isDeleting}
            onClick={() => handleDelete()}
            aria-label="Delete occupant">
                <IconTrash size={16} />
            </ActionIcon>
        </Group>
        </Group>
    </Paper>
    );
}