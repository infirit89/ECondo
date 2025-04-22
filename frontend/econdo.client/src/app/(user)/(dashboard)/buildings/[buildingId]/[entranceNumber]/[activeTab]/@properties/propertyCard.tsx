import { BriefPropertyResult } from "@/actions/property";
import { 
    Card, 
    CardSection, 
    Center, 
    Text, 
    Image, 
    Group, 
    ActionIcon, 
    useMantineTheme,
} from "@mantine/core";
import { IconPencil, IconTrash } from "@tabler/icons-react";

const propertyImages = new Map([
    ['офис', {
        src: 'https://images.unsplash.com/photo-1497215728101-856f4ea42174?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D',
        alt: 'Офис',
    }],
    ['апартамент', {
        src: 'https://images.pexels.com/photos/1918291/pexels-photo-1918291.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2',
        alt: 'Апартамент',
    }],
    ['ателие', {
        src: 'https://images.unsplash.com/photo-1598016677484-ad34c3fd766e?w=600&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8MTV8fHN0dWRpb3xlbnwwfHwwfHx8MA%3D%3D',
        alt: 'Ателие',
    }],
    ['гараж', {
        src: 'https://images.unsplash.com/photo-1562631320-c487e97ada24?q=80&w=1971&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D',
        alt: 'Гараж',
    }],
    ['магазин', {
        src: 'https://images.unsplash.com/photo-1472851294608-062f824d29cc?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D',
        alt: 'Магазин',
    }],
    ['мазе', {
        src: 'https://images.unsplash.com/photo-1610306013733-473dbbdf9cfc?q=80&w=1974&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D',
        alt: 'Мазе',
    }],
    ['склад', {
        src: 'https://images.unsplash.com/photo-1622030411594-c282a63aa1bc?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D',
        alt: 'Склад',
    }],
    ['таванско помещение', {
        src: 'https://images.pexels.com/photos/3568792/pexels-photo-3568792.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2',
        alt: 'Таванско Помещение',
    }],
])

export default function PropertyCard(
    {property} : { property: BriefPropertyResult }
) {
    const theme = useMantineTheme();
    let propertyImage = 
        propertyImages.get(
            property.propertyType.toLowerCase());

    propertyImage = propertyImage !== undefined ? 
    propertyImage :
    {
        src: 'https://images.pexels.com/photos/1918291/pexels-photo-1918291.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2',
        alt: 'Грешка',
    };

    return (
        <Card 
        padding="lg"
        radius="md"
        withBorder>
            <CardSection>
                <Image
                className={`h-[180]`}
                src={propertyImage.src}
                alt={propertyImage.alt}
                />
            </CardSection>

            <Center mb={'xs'} mt={'xs'}>
                <Text fw={500}>№ {property.number}</Text>
            </Center>

            <Text size="sm" c="dimmed" ta={'center'}>
                Етаж: {property.floor}
            </Text>
            <Text size="sm" c="dimmed" ta={'center'}>
                Тип: {property.propertyType}
            </Text>

            <CardSection mt={'md'} pr={'lg'} pb={'xs'} pt={'xs'}>
                <Group justify='end' gap={8} mr={0}>
                    <ActionIcon variant="subtle" color="gray">
                        <IconPencil size={20} stroke={1.5} />
                    </ActionIcon>
                    <ActionIcon variant="subtle" color="red">
                        <IconTrash size={20} color={theme.colors.red[6]} stroke={1.5} />
                    </ActionIcon>
                </Group>
            </CardSection>
        </Card>
    );
}