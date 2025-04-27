import { Container, SimpleGrid, Title } from "@mantine/core";
import MotionSection from "../motionSection";
import { IconKey, IconShieldCheck, IconUser } from "@tabler/icons-react";
import { motion } from "framer-motion";
import UserRoleCard from "@/components/userRoleCard";

export function UserRoleSection() {
    return (
    <MotionSection>
        <Container size="lg" py="xl">
            <Title
            order={2}
            ta="center"
            mb="md"
            style={{
                fontSize: '2.5rem',
                fontWeight: 800,
                background: 'linear-gradient(to right, #228be6, #15cfd0)',
                WebkitBackgroundClip: 'text',
                color: 'transparent',
            }}
            >
            За кого е ECondo?
            </Title>

            <SimpleGrid cols={{ base: 1, md: 3 }} spacing="xl" mt="xl">
            {[ 
                {icon: IconShieldCheck, role: "Домоуправители", description: "Управлявай регистрациите на сгради, общи части и дейности."},
                {icon: IconKey, role: "Собственици на имоти", description: "Регистрирай имоти, добавяй или актуализирай наематели и управлявай плащания."},
                {icon: IconUser, role: "Наематели", description: "Лесно плащай наем и такси за поддръжка онлайн."}
            ].map((item, idx) => (
                <motion.div
                    whileHover={{ scale: 1.05 }}
                    whileTap={{ scale: 0.98 }}
                    transition={{ type: 'spring', stiffness: 300 }}
                    key={idx}>
                    <UserRoleCard
                    key={idx}
                    icon={item.icon}
                    role={item.role}
                    description={item.description}
                    />
                </motion.div>
            ))}
            </SimpleGrid>
        </Container>
    </MotionSection>
    );
}