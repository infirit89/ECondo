'use client';

import { Modal, Tabs } from "@mantine/core";
import { useState } from "react";
import PropertyForm from "./propertyForm";
import OccupantsList from "./occupanstList";

interface PropertyEditModalProps {
    opened: boolean,
    onClose: () => void,
    propertyId: string,
    buildingId: string,
    entranceNumber: string,
}

export default function PropertyEditModal(
    {
        opened,
        onClose,
        propertyId,
        buildingId,
        entranceNumber,
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
                    buildingId={buildingId}
                    entranceNumber={entranceNumber}
                    propertyId={propertyId}/>
                </Tabs.Panel>
            </Tabs>

        </Modal>
    )
}