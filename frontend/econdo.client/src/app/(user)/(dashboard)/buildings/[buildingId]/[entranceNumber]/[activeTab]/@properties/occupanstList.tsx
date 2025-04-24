import { OccupantFormValues } from "@/utils/validationSchemas";
import { Box, Button, Group, Title, Text, Stack, Paper, ActionIcon, Flex, Badge } from "@mantine/core";
import { useModals } from "@mantine/modals";
import { IconEdit, IconPlus, IconTrash } from "@tabler/icons-react";
import OccupantForm from "./occupantForm";
import { useOccupantTypes } from "./occupantTypeProvider";
import { useQuery } from "@tanstack/react-query";
import { queryKeys } from "@/types/queryKeys";
import { getOccupantsInProperty, Occupant } from "@/actions/propertyOccupant";
import Loading from "@/app/(user)/(other)/condos/[activeTab]/loading";

interface OccupantsListProps {
    onClose?: () => void,
    propertyId: string,
    buildingId: string,
    entranceNumber: string,
}

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

const useQueryOccupantsInProperty = (propertyId: string) => {
  return useQuery({
    queryKey: queryKeys.occupants.inProperty(propertyId),
    queryFn: () => getOccupantsInProperty(propertyId),
  })
}

export default function OccupantsList({
    onClose,
    propertyId,
    buildingId,
    entranceNumber } : OccupantsListProps) {

    const { occupantTypes } = useOccupantTypes();
    const modals = useModals();

    const { data: occupants, isLoading } = useQueryOccupantsInProperty(propertyId);

    const handleAddOccupant = () => {
        const modalId = modals.openModal({
          title: "Add New Occupant",
          children: (
            <OccupantForm
                buildingId={buildingId}
                entranceNumber={entranceNumber}
                propertyId={propertyId}
                occupantTypes={occupantTypes}
                onCancel={() => modals.closeModal(modalId)}
            //   onSubmit={(newOccupant) => {
            //     const occupantWithId = {
            //       ...newOccupant,
            //       id: Date.now().toString(),
            //     }
            //     onUpdate([...occupants, occupantWithId])
            //     modals.closeModal(modalId)
            //   }}
            />
          ),
        })
      }

    const handleEditOccupant = (occupant: Occupant) => {
        const modalId = modals.openModal({
          title: "Edit Occupant",
          children: (
            <OccupantForm
                occupant={occupant}
                buildingId={buildingId}
                entranceNumber={entranceNumber}
                propertyId={propertyId}
                occupantTypes={occupantTypes}
                onCancel={() => modals.closeModal(modalId)}
            //   onSubmit={(updatedOccupant) => {
            //     // onUpdate(occupants.map((occ) => (occ.id === updatedOccupant.id ? updatedOccupant : occ)))
            //     // modals.closeModal(modalId)
            //   }}
            />
          ),
        })
      }

    const handleDeleteOccupant = (id: string) => {
        const modalId = modals.openConfirmModal({
          title: "Confirm deletion",
          children: <Text size="sm">Are you sure you want to delete this occupant? This action cannot be undone.</Text>,
          labels: { confirm: "Delete", cancel: "Cancel" },
          confirmProps: { color: "red" },
          onConfirm: () => {
            // TODO:
          },
        })
      }
    
    if(isLoading || !occupants?.ok)
      return <Loading/>

    return (
    <Box>
      <Group justify="space-between" mb="md">
        <Title order={3}>Контакти</Title>
        <Button leftSection={<IconPlus size={16} />} onClick={handleAddOccupant} size="sm">
          Добави контакт
        </Button>
      </Group>

      { occupants.value && occupants.value.length > 0 ? (
        <Stack gap="md" style={{ maxHeight: "400px", overflowY: "auto" }}>
          {occupants.value.map((occupant, index) => (
            <Paper key={index} p="md" withBorder>
              <Group justify="space-between">
                <Box>
                    <Group align="center" gap="xs">
                        <Text fw={500}>
                            {occupant.firstName} {occupant.middleName} {occupant.lastName}
                        </Text>
                        <Badge color={getBadgeColor(occupant.type)} size="sm">
                            {occupant.type}
                        </Badge>
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
                    onClick={() => handleEditOccupant(occupant)}
                    aria-label="Edit occupant"
                  >
                    <IconEdit size={16} />
                  </ActionIcon>
                  <ActionIcon
                    variant="light"
                    color="red"
                    onClick={() => handleDeleteOccupant('')}
                    aria-label="Delete occupant"
                  >
                    <IconTrash size={16} />
                  </ActionIcon>
                </Group>
              </Group>
            </Paper>
          ))}
        </Stack>
        
      ) : (
        <Text c="dimmed" ta="center" py="xl">
          Няма добавени контакти
        </Text>
      )}
      <Flex justify={'flex-end'} mt={'xl'}>
        {
            onClose !== undefined ?
            <Button variant="outline" onClick={onClose}>
                Затвори
            </Button>
            :
            <></>
        }
      </Flex>
    </Box>
    );
}