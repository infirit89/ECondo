'use client';
import { Anchor, AppShellSection, Box, Center, Divider, ScrollArea } from "@mantine/core";
import classes from './dashboardSidebar.module.css';

import { IconArrowLeft, IconBuildingEstate, IconReceipt, IconUsersGroup, } from '@tabler/icons-react';
import { LinksGroup } from '@/components/navbar/navbarLinksGroup';

const mockdata = [
    { label: 'Имоти', icon: IconBuildingEstate },
    { label: 'Собственост', icon: IconUsersGroup },
    { label: 'Сметки', icon: IconReceipt },
  ];


export default function DashboardSidebar() {
    const links = mockdata.map((item) => <LinksGroup {...item} key={item.label} />);
    
    return (
        <nav className={classes.navbar}>
            <AppShellSection my={'md'}>
                <Anchor size="sm" href="/condos/buildings">
                    <Center inline>
                        <IconArrowLeft size={12}/>
                        <Box ml={5}>Към начало</Box>
                    </Center>
                </Anchor>
            </AppShellSection>
            <Divider/>
            <ScrollArea className={classes.links}>
                <div className={classes.linksInner}>{links}</div>
            </ScrollArea>
    
            <div className={classes.footer}>
            </div>
        </nav>
    );
}