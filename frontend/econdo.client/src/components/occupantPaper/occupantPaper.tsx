import { Occupant } from "@/actions/propertyOccupant";
import { InvitationStatus } from "@/types/propertyOccupant";
import { ActionIcon, Badge, Box, Group, Paper, Text, Tooltip } from "@mantine/core";
import { IconClockHour4, IconEdit, IconMail, IconMailCheck, IconMailX, IconTrash } from "@tabler/icons-react";

const getBadgeColor = (occupantType: string): string => {
  switch (occupantType) {
      case "Собственик":
          return "blue"
      case "Наемател":
          return "green"
      case "Представител":
          return "violet"
      case "Ползвател":
          return "yellow"
      default:
          return "gray"
  }
}

const getInvitationStatusInfo = (status: InvitationStatus): 
{ icon: React.ReactNode; color: string; tooltip: string } => {
  switch (status) {
    case InvitationStatus.Pending:
      return {
        icon: <IconMail size={16} />,
        color: "blue",
        tooltip: "Изпратена покана",
      }
    case InvitationStatus.Accepted:
      return {
        icon: <IconMailCheck size={16} />,
        color: "green",
        tooltip: "Приета покана",
      }
    case InvitationStatus.Declined:
      return {
        icon: <IconMailX size={16} />,
        color: "red",
        tooltip: "Отказана покана",
      }
    case InvitationStatus.Expired:
      return {
        icon: <IconClockHour4 size={16} />,
        color: "gray",
        tooltip: "Изтекла покана",
      }
    case InvitationStatus.NotInvited:
    default:
      return {
        icon: null,
        color: "",
        tooltip: "Няма покана",
      }
  }
}

interface OccupantPaperProps {
    occupant: Occupant,
    handleEdit: (occupant: Occupant) => void,
    handleDelete: (id: string) => void,
    isDeleting: boolean,
}

export default function OccupantPaper(
  { occupant, handleEdit, handleDelete, isDeleting } : OccupantPaperProps) {
    const invitationStatusInfo = getInvitationStatusInfo(occupant.invitationStatus);
    
    return (
        <Paper p="md" withBorder>
            <Group justify="space-between">
            <Box>
                <Group align="center" gap="xs">
                    <Text fw={500}>
                        {occupant.firstName} {occupant.middleName} {occupant.lastName}
                    </Text>
                    <Badge color={getBadgeColor(occupant.type)} size="sm">
                        {occupant.type}
                    </Badge>
                    {invitationStatusInfo.icon && (
                    <Tooltip label={invitationStatusInfo.tooltip}>
                        <ActionIcon
                        variant="transparent"
                        color={invitationStatusInfo.color}
                        size="sm"
                        aria-label={invitationStatusInfo.tooltip}
                        >
                        {invitationStatusInfo.icon}
                        </ActionIcon>
                    </Tooltip>
                    )}
                </Group>
                {
                occupant.email && (
                    <Text size="sm" c="dimmed">
                    {occupant.email}
                    </Text>
                )}
            </Box>
            <Group>
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
            </Group>
            </Group>
        </Paper>
    )
}