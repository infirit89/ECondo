import { Tabs, TabsList, TabsTab } from "@mantine/core";
import Link from "next/link";
import { ReactNode } from "react";

export default async function CondoLayout({ buildings, properties, params }: Readonly<{
    buildings: ReactNode,
    properties: ReactNode,
    params: Promise<{ activeTab: 'buildings' | 'properties' }>
}>) {
    const { activeTab } = await params;
    const tabs = [
        { value: '/condos/properties', label: 'Имоти' },
        { value: '/condos/buildings', label: 'Сгради' },
    ];

    return (
        <>
            <Tabs defaultValue={`/condos/${activeTab}`} mt={'xl'} variant='outline'>
                <TabsList justify={'center'}>
                    {tabs.map((tab) => (
                        <Link href={tab.value} key={tab.value}>
                            <TabsTab value={tab.value}>{tab.label}</TabsTab>
                        </Link>
                    ))}
                </TabsList>
            </Tabs>
            { activeTab === 'buildings' ?
                buildings :
                activeTab === 'properties' ?
                properties :
                <></> }
        </>
    )
}