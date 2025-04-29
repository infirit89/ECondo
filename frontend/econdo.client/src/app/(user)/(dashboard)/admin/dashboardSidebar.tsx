'use client';
import { AppShellNavbar, ScrollArea } from "@mantine/core";
import classes from './dashboardSidebar.module.css';

import { IconBuildings, IconHome, IconUsers } from '@tabler/icons-react';
import Link from "next/link";
export default function DashboardSidebar(
    { activeTab, } : 
    { activeTab: string, }) {

    const data = [
        { id: 'buildings', link: `/admin/buildings`, label: 'Сгради', icon: IconBuildings },
        { id: 'properties', link: `/admin/properties`, label: 'Имоти', icon: IconHome },
        { id: 'users', link: `/admin/users`, label: 'Потребители', icon: IconUsers },
    ];
    

    const links = data.map((item) => (
    <Link
    className={classes.link}
    data-active={activeTab.startsWith(item.link) || undefined}
    href={item.link}
    key={item.id}>
        <item.icon className={classes.linkIcon} stroke={1.5} />
        <span>{item.label}</span>
    </Link>
    ));
    return (
        <AppShellNavbar>
            <nav className={classes.navbar}>
                <div className={classes.navbarMain}>
                    <ScrollArea className={classes.links}>
                        {links}
                    </ScrollArea>
                </div>
            </nav>
        </AppShellNavbar>
    );
}