'use client';
import { Anchor, AppShellNavbar, AppShellSection, Box, Center, ScrollArea } from "@mantine/core";
import classes from './dashboardSidebar.module.css';

import { IconArrowLeft, IconReceipt, IconUsersGroup, } from '@tabler/icons-react';
import Link from "next/link";

export type ActiveTab = 'occupants' | 'bills';

export default function DashboardSidebar(
    { propertyId, activeTab, isOwner, } : 
    { propertyId: string, activeTab: ActiveTab, isOwner: boolean }) {

    const data = isOwner ? [
        { id: 'occupants', link: `/properties/${propertyId}/occupants`, label: 'Ползватели', icon: IconUsersGroup },
        { id: 'bills', link: `/properties/${propertyId}/bills`, label: 'Сметки', icon: IconReceipt },
    ] : [
        { id: 'bills', link: `/properties/${propertyId}/bills`, label: 'Сметки', icon: IconReceipt }
    ];
    

    const links = data.map((item) => (
        <Link
          className={classes.link}
          data-active={item.id === activeTab || undefined}
          href={item.link}
          key={item.id}
        >
          <item.icon className={classes.linkIcon} stroke={1.5} />
          <span>{item.label}</span>
        </Link>
      ));
    return (
        <AppShellNavbar>
            <nav className={classes.navbar}>
                <div className={classes.navbarMain}>
                    <AppShellSection className={classes.header}>
                        <Anchor component={Link} size="sm" href="/condos/properties">
                            <Center inline>
                                <IconArrowLeft size={12}/>
                                <Box ml={5}>Към начало</Box>
                            </Center>
                        </Anchor>
                    </AppShellSection>
                    <ScrollArea className={classes.links}>
                        {links}
                    </ScrollArea>
                </div>
            </nav>
        </AppShellNavbar>
    );
}