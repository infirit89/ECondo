'use client';

import { 
    Container, 
    Title, 
    Flex, 
    Button
} from "@mantine/core";
import PropertiesList from "./propertiesList";
import { useDisclosure } from "@mantine/hooks";
import { PropertyCreationModal } from "@/components/propetyCreationModal/propertyCreationModal";

export default function PropertyPageContent() {

    const [opened, { open, close }] = useDisclosure(false);

    return (
        <>
            <PropertyCreationModal
            isOpen={opened}
            onClose={close} />
            <Container size="lg" py="xl">
                <Flex justify={'space-between'} mb={'md'}>
                    <Title>Имоти</Title>
                    <Button onClick={open}>Нов имот</Button>
                </Flex>
                <PropertiesList/>
            </Container>
        </>
    );
}