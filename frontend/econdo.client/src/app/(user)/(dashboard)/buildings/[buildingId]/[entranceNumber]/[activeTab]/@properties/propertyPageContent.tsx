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
import { Result } from "@/types/result";
import { PropertyTypeNameResult } from "@/actions/property";
import { use } from "react";

export default function PropertyPageContent(
    {propertyTypes} : 
    { propertyTypes: Promise<Result<PropertyTypeNameResult>> } 
) {
    const allPropertyTypes = use(propertyTypes);
    if(!allPropertyTypes.ok)
        throw new Error('Unexpected result');
    
    const [opened, { open, close }] = useDisclosure(false);

    return (
        <>
            <PropertyCreationModal
            isOpen={opened}
            onClose={close}
            propertyTypes={allPropertyTypes.value!} />
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