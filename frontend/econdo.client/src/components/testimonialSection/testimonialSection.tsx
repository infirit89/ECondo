import { Container, SimpleGrid, Title } from "@mantine/core";
import { motion } from "framer-motion";
import TestimonialCard from "@/components/testimonialCard";

export function TestimonialSection() {

    return (
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
            Доверено от общности
        </Title>

        <SimpleGrid cols={{ base: 1, md: 2 }} spacing="xl" mt="xl">
            {[ 
            {name: "Йордан Станков", role: "Домоуправител, Skyline Towers", testimony: "ECondo промени начина, по който управляваме нашите наематели — плащанията, управлението на наематели, всичко е толкова лесно сега!"},
            {name: "Тома Йорданов", role: "Собственик, Palm Residences", testimony: "Най-накрая модерно средство за управление на собствеността. Управлението на наематели и плащането на такси никога не е било по-лесно."}
            ].map((item, idx) => (
            <motion.div
                key={idx}
                initial={{ opacity: 0, y: 30 }}
                whileInView={{ opacity: 1, y: 0 }}
                transition={{ duration: 0.6, delay: idx * 0.2 }}
            >
                <TestimonialCard
                name={item.name}
                role={item.role}
                testimony={item.testimony}
                />
            </motion.div>
            ))}
        </SimpleGrid>
    </Container>  
    );
}