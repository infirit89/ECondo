import { Anchor, Box, Burger, Button, Divider, Drawer, Group, rem, ScrollArea, Text } from "@mantine/core";
import styles from '@/components/Navbar.module.css';
import { useDisclosure } from "@mantine/hooks";

export function Navbar() {
    const [drawerOpened, { toggle: toggleDrawer, close: closeDrawer }] = useDisclosure(false);

    return (
        <>
            <Box>
                <Group justify={'space-between'}>
                    <Text component="a" href="/">ECondo</Text>
                </Group>
                <Group visibleFrom="sm">
                    <a href='/login' className={styles.link}>Вход</a>
                    <a href='/register' className={styles.link}>Регистрация</a>
                    <a href='/about' className={styles.link}>За Errandix</a>
                </Group>
                <Burger opened={drawerOpened} onClick={toggleDrawer} hiddenFrom="sm" mb={rem(15)} />
            </Box>
            <Drawer
                opened={drawerOpened}
                onClose={closeDrawer}
                size={'100%'}
                padding={'md'}
                title={
                <Text component="a" href="/">ECondo</Text>
                // <Anchor href='/'>
                //     <Image component={NextImage} src={LogoErrandixTransparent} className={classes.logo} alt="errandix" mt={'0'}/>
                // </Anchor>
                }
                className={styles.drawer}
                hiddenFrom='sm'
                zIndex={1000000}
                closeButtonProps={{
                mt: '15px',
                }}>
                <ScrollArea h={`calc(100vh -${rem(80)})`} mx={'-md'}>
                    <Divider my={'sm'}/>
                    <a href="/" className={styles.drawerlink}>
                    Начало
                    </a>
                    {/* <a href="/about" className={styles.drawerlink}>
                    За Errandix
                    </a> */}
                    <Divider my={'sm'}/>
                    <Group justify={'center'} grow pb={'xl'} px={'md'}>
                    <Button component='a' href='/login' variant={'default'}>Вход</Button>
                    <Button component='a' href='/register'>Регистрация</Button>
                    </Group>
                </ScrollArea>
            </Drawer>
        </>
    )
}