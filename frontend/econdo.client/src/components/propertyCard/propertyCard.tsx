import { PropertyResult } from "@/actions/property";
import { 
    Card,
    Text,
    Group, 
    ActionIcon,
    Avatar,
    Badge,
    Stack,
    Divider,
} from "@mantine/core";
import { IconBuildingCottage, IconBuildingFactory, IconBuildingSkyscraper, IconBuildingStore, IconBuildingWarehouse, IconChartPie, IconEdit, IconHome, IconPalette, IconPencil, IconRuler, IconStairs, IconTrash, IconUsers } from "@tabler/icons-react";
import { MouseEventHandler, useState } from "react";
import PropertyEditModal from "@/components/propertyEditModal";

const getPropertyTypeInfo = (propertyType: string) => {
    switch (propertyType) {
        case "апартамент":
            return { color: "blue", icon: <IconHome size={24} />, label: 'апартамент' }
        case "гараж":
            return { color: "gray", icon: <IconBuildingWarehouse size={24} />, label: 'гараж' }
        case "офис":
            return { color: "green", icon: <IconBuildingSkyscraper size={24} />, label: 'офис' }
        case "магазин":
            return { color: "orange", icon: <IconBuildingStore size={24} />, label: 'магазин' }
        case "мазе":
            return { color: "violet", icon: <IconStairs size={24} />, label: 'мазе' }
        case "склад":
            return { color: "yellow", icon: <IconBuildingFactory size={24} />, label: 'склад' }
        case "таванско помещение":
            return { color: "cyan", icon: <IconBuildingCottage size={24} />, label: 'таванско' }
        case "ателие":
            return { color: "pink", icon: <IconPalette size={24} />, label: 'ателие' }
        default:
            return { color: "blue", icon: <IconHome size={24} />, label: 'апартамент' }
    }
}

interface ProperyCardProps {
    property: PropertyResult,
    handleDelete?: (id: string) => void,
    canEdit: boolean,
    isDeleting: boolean,
}

export function PropertyCard({ 
    property, 
    handleDelete,
    canEdit,
    isDeleting, } : ProperyCardProps) {

    const propertyInfo = getPropertyTypeInfo(property.propertyType.toLowerCase());

    const [editModalOpened, setEditModalOpen] = useState(false);

    return (
        <>
            {
                canEdit && (
                    <PropertyEditModal
                    opened={editModalOpened}
                    onClose={() => setEditModalOpen(false)}
                    propertyId={property.id}
                    />
                )
            }
            <Card
            shadow="sm" 
            padding="xl"
            radius="lg"
            withBorder
            style={{ transition: "transform 0.2s ease, box-shadow 0.2s ease" }}
            onMouseEnter={(e) => (e.currentTarget.style.transform = "translateY(-4px)")}
            onMouseLeave={(e) => (e.currentTarget.style.transform = "translateY(0)")}>
            <Card.Section 
            p="md" 
            style={{
                background: `var(--mantine-color-${propertyInfo.color}-0)`,
                borderBottom: `1px solid var(--mantine-color-${propertyInfo.color}-2)`,
                borderRadius: "var(--mantine-radius-lg) var(--mantine-radius-lg) 0 0",
            }}>
              <Group justify='space-between'>
                <Group>
                  <Avatar size="md" radius="xl" color={propertyInfo.color}>
                    {propertyInfo.icon}
                  </Avatar>
                  <div>
                    <Group gap="xs">
                      <Text fw={700} size="xl">
                        {property.number}
                      </Text>
                      <Badge color={propertyInfo.color} size="sm">
                        {propertyInfo.label.charAt(0).toUpperCase() + propertyInfo.label.slice(1)}
                      </Badge>
                    </Group>
                    <Text size="sm" c="dimmed">
                      Етаж {property.floor}
                    </Text>
                  </div>
                </Group>
                {
                    canEdit && handleDelete && (
                        <Group gap={8}>
                            <ActionIcon
                            variant='subtle'
                            color="blue"
                            disabled={isDeleting}
                            onClick={() => setEditModalOpen(true)}>
                                <IconEdit size={18} />
                            </ActionIcon>
                            <ActionIcon 
                            variant='subtle'
                            color="red"
                            disabled={isDeleting}
                            onClick={() => handleDelete(property.id)}>
                                <IconTrash size={18} />
                            </ActionIcon>
                        </Group>
                    )
                }
              </Group>
            </Card.Section>

            <Stack gap="xs" mt="md">
              <Group gap="xs">
                <IconRuler size={16} color="gray" />
                <Text size="sm">
                  <Text span fw={500}>
                    Застроена площ:
                  </Text>{" "}
                  {property.builtArea} m²
                </Text>
              </Group>

              <Group gap="xs">
                <IconChartPie size={16} color="gray" />
                <Text size="sm">
                  <Text span fw={500}>
                    Идеални части:
                  </Text>{" "}
                  {property.idealParts}
                </Text>
              </Group>

              <Divider
                my="xs"
                label={
                  <Group gap="xs">
                    <IconUsers size={16} />
                    <Text size="sm">Контакти</Text>
                  </Group>
                }
                labelPosition="left"
              />

              {/* {property.occupants.length > 0 ? (
                <Group spacing="xs">
                  {property.occupants.map((occupant) => (
                    <Avatar
                      key={occupant.id}
                      size="sm"
                      radius="xl"
                      color={stringToColor(occupant.name)}
                      title={occupant.name}
                    >
                      {getInitials(occupant.name)}
                    </Avatar>
                  ))}
                </Group>
              ) : ( */}
                <Text size="sm" c="dimmed" fs={'italic'}>
                    Няма назначени контакти
                </Text>
              {/* )} */}
            </Stack>
          </Card>
        </>
    );
}