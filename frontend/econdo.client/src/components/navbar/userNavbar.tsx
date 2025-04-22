'use client';
import { 
    Anchor,
    AppShellHeader,
    Box,
    Burger,
    Button,
    Divider,
    Drawer,
    Group,
    Menu,
    MenuDropdown,
    MenuItem,
    MenuTarget,
    rem,
    ScrollArea,
    Stack
} from "@mantine/core";
import classes from './navbar.module.css';
import { useDisclosure } from "@mantine/hooks";
import { ECondoLogo } from '@/components/logo/econdoLogo';
import UserAvatar from "./userAvatar";
import Link from "next/link";

export function UserNavbar() {
    const [drawerOpened, { toggle: toggleDrawer, close: closeDrawer }] = useDisclosure(false);
    
    return (
        <AppShellHeader>
            <Box>
                <header className={classes.header}>
                    <Group justify="space-between" h="100%">
                    <Anchor component={Link} href="/" pt={4} visibleFrom='sm'>
                        <ECondoLogo size={30}/>
                    </Anchor>

                    {/* <Group h="100%" gap={0} visibleFrom="sm">
                        <a href="#" className={classes.link}>
                        Начало
                        </a>
                        <a href="#" className={classes.link}>
                        За нас
                        </a>
                        <a href="#" className={classes.link}>
                        Контакти
                        </a>
                    </Group> */}

                    <Group visibleFrom="sm">
                        
                        <Menu
                        shadow="md"
                        width={200}
                        key={'profile'}
                        withinPortal
                        position="bottom-end"
                        transitionProps={{ transition: 'pop-top-right' }}
                        offset={5}>
                            <MenuTarget>
                                <UserAvatar/>
                            </MenuTarget>
                        
                            <MenuDropdown>
                                <MenuItem component={Link} href='/profile'>Профил</MenuItem>
                                <MenuItem component={Link} href='/logout'>Изход</MenuItem>
                            </MenuDropdown>
                        </Menu>
                    </Group>

                    <Burger opened={drawerOpened} onClick={toggleDrawer} hiddenFrom="sm" />
                    </Group>
                </header>

                <Drawer
                    opened={drawerOpened}
                    onClose={closeDrawer}
                    size="100%"
                    padding="md"
                    title={
                        <Anchor href="/">
                            <ECondoLogo size={40} />
                        </Anchor>
                    }
                    hiddenFrom="sm"
                    zIndex={1000000}
                >
                    <ScrollArea h={`calc(100vh - ${rem(600)})`} mx="-md">
                    {/* <Divider my="sm" /> */}

                    {/* <a href="#" className={classes.link}>
                        Начало
                    </a>
                    <a href="#" className={classes.link}>
                        За нас
                    </a>
                    <a href="#" className={classes.link}>
                        Контакти
                    </a> */}

                    <Divider my="sm" />

                    <Link href="/" className={classes.drawerlink}>
                    Начало
                    </Link>
                    <Link href="/profile" className={classes.drawerlink}>
                    Профил
                    </Link>
                    </ScrollArea>
                    <Stack justify={'flex-end'} h={`calc(100vh - ${rem(300)})`} mx={'-md'}>
                    <Group justify={'center'} grow pb={'xl'} px={'md'}>
                        <Button component={Link} href='/logout' variant='filled' color={'red'}>Изход</Button>
                    </Group>
                    </Stack>
                </Drawer>
            </Box>
        </AppShellHeader>
    )
}