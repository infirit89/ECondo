import MotionSection from "@/components/motionSection";
import { Container, SimpleGrid, Title } from "@mantine/core";
import { IconBuilding, IconCash, IconHome, IconUsers } from "@tabler/icons-react";
import { motion } from "framer-motion";
import FeatureCard from "@/components/featureCard";

export function ComponentsSection() {

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
          Всичко, от което се нуждаете, за да управлявате имота си
        </Title>

        <SimpleGrid cols={{ base: 1, sm: 2, md: 4 }} spacing="xl" mt="xl">
          {[ 
            {icon: IconBuilding, title: "Управление на сгради", description: "Регистрирайте и следете множество сгради лесно."},
            {icon: IconHome, title: "Регистриране на имоти", description: "Добавете единици и възлагайте собственици или наематели без усилия."},
            {icon: IconUsers, title: "Управление на обитатели", description: "Собствениците могат да управляват наемателите си по всяко време."},
            {icon: IconCash, title: "Плащане на сметки онлайн", description: "Плащайте такси за поддръжка, ремонти и почистване сигурно."}
          ].map((item, idx) => (
            <motion.div
              key={idx}
              whileHover={{ scale: 1.05 }}
              whileTap={{ scale: 0.98 }}
              transition={{ type: 'spring', stiffness: 300 }}
            >
              <FeatureCard
                key={idx}
                icon={item.icon}
                title={item.title}
                description={item.description}
              />
            </motion.div>
          ))}
        </SimpleGrid>
      </Container>
    </MotionSection>
    );
}