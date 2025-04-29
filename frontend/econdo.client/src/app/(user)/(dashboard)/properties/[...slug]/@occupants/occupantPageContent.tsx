'use client';

import OccupantForm from "@/components/occupantForm";
import { useOccupantTypes } from "@/providers/occupantTypeProvider";
import { Button, Container, Flex, Modal, Title } from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import { useParams } from "next/navigation";
import OccupantsList from "./occupantsList";

export default function OccupantPageContent() {
    const { slug } = useParams<{ slug: string[] }>();
    const [opened, { open, close }] = useDisclosure(false);
    const propertyId = slug[0];
    const occupantTypes = useOccupantTypes()
        .occupantTypes
        .occupantTypes
        .filter(ot => ot !== 'Собственик');

    return (
        <>
            <Modal opened={opened} onClose={close}>
                <OccupantForm 
                onCancel={close} 
                propertyId={propertyId}
                occupantTypes={{occupantTypes}}
                onSucess={close}/>
            </Modal>
            <Container size="lg" py="xl">
                <Flex justify={'space-between'} mb={'md'}>
                    <Title>Ползватели</Title>
                    <Button onClick={open}>Нов ползвател</Button>
                </Flex>
                <OccupantsList propertyId={propertyId} occupantTypes={{occupantTypes}}/>
            </Container>
        </>
    );
}