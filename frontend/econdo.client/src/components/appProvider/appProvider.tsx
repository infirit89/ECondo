'use client';

import { AppShell } from "@mantine/core";
import { usePathname } from "next/navigation";
import { ReactNode } from "react";

export default function AppProvider(
    { children } : 
    { children: ReactNode }) {

    const pathname = usePathname();
    pathname.startsWith('/buildings');

    return (
        <AppShell>

        </AppShell>
    );
}
