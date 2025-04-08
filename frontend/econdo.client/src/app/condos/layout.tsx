import { AppShellMain, Center, Tabs, TabsList, TabsTab } from "@mantine/core";
import Link from "next/link";
import { ReactNode, Suspense } from "react";
import Loading from "./loading";

export default function CondoLayout({children}: {children: ReactNode}) {
    const tabs = [
        { value: '/condos', label: 'Имоти' },
        { value: '/condos/buildings', label: 'Сгради' },
    ];

    return (
        <AppShellMain>
            <Center mt={'xl'}>
                <Tabs>
                    <TabsList>
                        {tabs.map((tab) => (
                            <Link href={tab.value} key={tab.value} passHref>
                                <TabsTab value={tab.value}>{tab.label}</TabsTab>
                            </Link>
                        ))}
                    </TabsList>
                </Tabs>
            </Center>
            <Suspense fallback={<Loading/>}>
                {children}
            </Suspense>
        </AppShellMain>
    )
}