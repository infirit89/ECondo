'use client';

import { Modal, Tabs } from "@mantine/core";
import { useState } from "react";
import PropertyForm from "@/components/propertyForm";
import OccupantsList from "@/components/occupantsList";

interface PropertyEditModalProps {
    opened: boolean,
    onClose: () => void,
    propertyId: string,
}

export function PropertyEditModal(
    {
        opened,
        onClose,
        propertyId,
    } : PropertyEditModalProps) {

    const [activeTab, setActiveTab] = useState<string | null>("property")
    
    return (
        <Modal
            opened={opened}
            onClose={onClose} size={'lg'}>
            <Tabs value={activeTab} onChange={setActiveTab}>
                <Tabs.List mb="md">
                    <Tabs.Tab value="property">Детайли</Tabs.Tab>
                    <Tabs.Tab value="occupants">Контакти</Tabs.Tab>
                </Tabs.List>

                <Tabs.Panel value="property">
                    <PropertyForm
                    onCancel={onClose}
                    propertyId={propertyId}
                    />
                </Tabs.Panel>

                <Tabs.Panel value="occupants">
                    <OccupantsList
                    onClose={onClose}
                    propertyId={propertyId}/>
                </Tabs.Panel>
            </Tabs>

        </Modal>
    )
}