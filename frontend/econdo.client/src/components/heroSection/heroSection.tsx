import { BackgroundImage, Button, Container, Overlay, Title, Text } from "@mantine/core";
import { motion } from "framer-motion";
import Link from "next/link";

export function HeroSection() {

    return (
    <BackgroundImage
    src="https://images.unsplash.com/photo-1516501312919-d0cb0b7b60b8"
    radius={0}
    style={{ minHeight: '100vh', position: 'relative' }}
    >
    {/* Optional: Overlay to enhance text readability */}
    <Overlay 
    gradient="linear-gradient(180deg, rgba(0,0,0,0.7) 0%, rgba(0,0,0,0.2) 100%)"
    opacity={1}
    zIndex={0} />

        <Container size="md" pos={'relative'} ta={'center'} c={'white'} pt={180} style={{ zIndex: 1, }}>
            <Container size="md" style={{ textAlign: 'center', color: 'white' }}>
            <motion.div
                initial="hidden"
                animate="visible"
                variants={{
                hidden: {},
                visible: {
                    transition: {
                    staggerChildren: 0.2,
                    },
                },
                }}
            >
                <motion.div
                initial={{ opacity: 0, y: 20 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ duration: 0.6, ease: 'easeOut' }}
                >
                <Title order={1} size="48px" fw={900} style={{
                    textShadow: '0 2px 4px rgba(0,0,0,0.7)'
                }}>
                    Управлявай сградите си лесно
                </Title>
                </motion.div>

                <motion.div
                initial={{ opacity: 0, y: 20 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ delay: 0.3, duration: 0.6, ease: 'easeOut' }}
                >
                <Text mt="md" size="lg" style={{
                    textShadow: '0 2px 4px rgba(0,0,0,0.7)'
                }}>
                    Платформа за мениджмънт на имоти, създадена за модерни общности.
                </Text>
                </motion.div>

                <motion.div
                initial={{ opacity: 0, y: 20 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ delay: 0.5, duration: 0.6, ease: 'easeOut' }}>
                    <motion.div whileHover={{ scale: 1.03 }} whileTap={{ scale: 0.98 }}>
                        <Button
                        size="lg"
                        variant="gradient"
                        gradient={{ from: 'blue', to: 'cyan' }}
                        radius="xl"
                        mt="xl"
                        component={Link}
                        href={'/register'}>
                            Започни сега
                        </Button>
                    </motion.div>
                </motion.div>
            </motion.div>
            </Container>
        </Container>
    </BackgroundImage>
    )
}