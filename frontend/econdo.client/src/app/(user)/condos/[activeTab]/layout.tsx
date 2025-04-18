import { AppShellMain, Center, Tabs, TabsList, TabsTab } from "@mantine/core";
import Link from "next/link";
import { ReactNode, Suspense } from "react";
import Loading from "./loading";

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
            <Tabs defaultValue={`/condos/${activeTab}`} mt={'xl'}>
                <TabsList justify={'center'}>
                    {tabs.map((tab) => (
                        <Link href={tab.value} key={tab.value} passHref>
                            <TabsTab value={tab.value}>{tab.label}</TabsTab>
                        </Link>
                    ))}
                </TabsList>
            </Tabs>
            <Suspense fallback={<Loading/>}>
                { activeTab === 'buildings' ?
                    buildings :
                    activeTab === 'properties' ?
                    properties :
                    <></> }
            </Suspense>
        </>
    )
}