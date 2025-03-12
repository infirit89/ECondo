'use client';
import { Anchor, Box, Burger, Button, Divider, Drawer, Group, ScrollArea, } from "@mantine/core";
import classes from '@/components/navbar.module.css';
import { useDisclosure } from "@mantine/hooks";
import { MantineLogo } from "@mantinex/mantine-logo";

export function Navbar() {
    const [drawerOpened, { toggle: toggleDrawer, close: closeDrawer }] = useDisclosure(false);

    return (
        <Box>
            <header className={classes.header}>
                <Group justify="space-between" h="100%">
                <Anchor href="/">
                    <MantineLogo size={30} />
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
                    <Button variant="default" component="a" href="/login">Вход</Button>
                    <Button component="a" href="/register">Регистрирай се</Button>
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
                        <MantineLogo size={40} />
                    </Anchor>
                }
                hiddenFrom="sm"
                zIndex={1000000}
            >
                <ScrollArea h="calc(100vh - 80px" mx="-md">
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

                <Group justify="center" grow pb="xl" px="md">
                    <Button variant="default" component="a" href="/login">Вход</Button>
                    <Button component="a" href="/register">Регистрирай се</Button>
                </Group>
                </ScrollArea>
            </Drawer>
        </Box>
    )
}