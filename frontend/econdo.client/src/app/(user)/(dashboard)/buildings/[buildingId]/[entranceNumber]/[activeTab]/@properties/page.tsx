'use client';

import { Container, Title, Flex, Button } from "@mantine/core";;
import PropertiesList from "./propertiesList";

export default function PropertiesPage() {

    return (
        <Container size="lg" py="xl">
            <Flex justify={'space-between'} mb={'md'}>
                <Title>Имоти</Title>
                <Button>Нов имот</Button>
            </Flex>
            
            <PropertiesList/>
            
        </Container>
    );
}
