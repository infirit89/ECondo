import MotionSection from "@/components/motionSection";
import { Button, Container, Title } from "@mantine/core";
import { motion } from "framer-motion";
import Link from "next/link";

export function CallToActionSection() {
    return (
    <MotionSection>
        <Container size="lg" py="xl" style={{ textAlign: 'center' }}>
            <Title order={2} mb="md">Готови ли сте да опростите управлението на вашата сграда?</Title>
            <motion.div whileHover={{ scale: 1.03 }} whileTap={{ scale: 0.98 }}>
                <Button 
                size="lg" 
                variant="gradient" 
                gradient={{ from: 'blue', to: 'cyan' }}
                component={Link}
                href="/register">
                Присъединете се към ECondo днес
                </Button>
            </motion.div>
        </Container>
    </MotionSection>        
    );
}