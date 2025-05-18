
'use client';

import { useState } from 'react';
import {
    Container,
    Title,
    Text,
    Paper,
    Stack,
    Divider,
    Accordion,
    TextInput,
    Textarea,
    Button,
    Grid,
    Group,
    Box,
    Card,
    ThemeIcon,
    SimpleGrid,
    rem
} from '@mantine/core';
import { IconMail, IconPhone, IconBrandWhatsapp, IconQuestionMark, IconHome, IconCreditCard, IconUsers, IconCalendarTime } from '@tabler/icons-react';
import { NextPage } from 'next';
import { useForm } from 'react-hook-form';

const ICON_SIZE = rem(24);

interface ContactFormValues {
    name: string;
    email: string;
    subject: string;
    message: string;
}

export default function SupportPage() {
    const [isSubmitting, setIsSubmitting] = useState(false);

    const form = useForm<ContactFormValues>({
        defaultValues: {
            name: '',
            email: '',
            subject: '',
            message: ''
        },
        // validate: {
        //     name: (value) => value.trim().length < 2 ? 'Името трябва да съдържа поне 2 символа' : null,
        //     email: (value) => /^\S+@\S+\.\S+$/.test(value) ? null : 'Невалиден имейл адрес',
        //     subject: (value) => value.trim().length < 5 ? 'Заглавието трябва да съдържа поне 5 символа' : null,
        //     message: (value) => value.trim().length < 20 ? 'Съобщението трябва да съдържа поне 20 символа' : null,
        // }
    });

    const handleSubmit = (values: ContactFormValues) => {
        setIsSubmitting(true);

        // Simulating API call
        setTimeout(() => {
            setIsSubmitting(false);
            form.reset();
            // notifications.show({
            //     title: 'Съобщението е изпратено',
            //     message: 'Ще се свържем с вас възможно най-скоро.',
            //     color: 'green',
            // });
        }, 1000);
    };

    return (
        <Container size="lg" py="xl">
            <Paper shadow="xs" p="md" withBorder>
                <Stack gap="xl">
                    <Box>
                        <Title order={1} ta="center">Поддръжка и помощ</Title>
                        <Text ta="center" c="dimmed" mt="sm">
                            Имате въпроси или проблеми? Тук сме, за да помогнем.
                        </Text>
                    </Box>

                    <Divider />

                    {/* Contact Information */}
                    <SimpleGrid cols={{ base: 1, sm: 2 }} spacing="lg">
                        <Card shadow="sm" padding="lg" radius="md" withBorder>
                            <Card.Section p="md">
                                <Group justify="center">
                                    <ThemeIcon size={50} radius="md" variant="light" color="blue">
                                        <IconMail size={30} />
                                    </ThemeIcon>
                                </Group>
                            </Card.Section>

                            <Stack align="center" mt="md" gap="xs">
                                <Text fw={700}>Имейл</Text>
                                <Text component="a" href="mailto:support@econdo.online" c="blue">
                                    support@econdo.online
                                </Text>
                                <Text size="sm" color="dimmed">
                                    Отговаряме в рамките на 24 часа
                                </Text>
                            </Stack>
                        </Card>

                        <Card shadow="sm" padding="lg" radius="md" withBorder>
                            <Card.Section p="md">
                                <Group justify="center">
                                    <ThemeIcon size={50} radius="md" variant="light" color="green">
                                        <IconPhone size={30} />
                                    </ThemeIcon>
                                </Group>
                            </Card.Section>

                            <Stack align="center" mt="md" gap="xs">
                                <Text fw={700}>Телефон</Text>
                                <Text component="a" href="tel:+35912345678" c="green">
                                    +359 1 234 5678
                                </Text>
                                <Text size="sm" c="dimmed">
                                    Пон-Пет, 9:00-18:00
                                </Text>
                            </Stack>
                        </Card>

                        {/* <Card shadow="sm" padding="lg" radius="md" withBorder> */}
                        {/*     <Card.Section p="md"> */}
                        {/*         <Group justify='center'> */}
                        {/*             <ThemeIcon size={50} radius="md" variant="light" color="grape"> */}
                        {/*                 <IconBrandWhatsapp size={30} /> */}
                        {/*             </ThemeIcon> */}
                        {/*         </Group> */}
                        {/*     </Card.Section> */}
                        {/**/}
                        {/*     <Stack align="center" mt="md" gap="xs"> */}
                        {/*         <Text fw={700}>WhatsApp</Text> */}
                        {/*         <Text component="a" href="https://wa.me/359889876543" c="grape"> */}
                        {/*             +359 88 987 6543 */}
                        {/*         </Text> */}
                        {/*         <Text size="sm" c="dimmed"> */}
                        {/*             Бързи отговори в работно време */}
                        {/*         </Text> */}
                        {/*     </Stack> */}
                        {/* </Card> */}
                    </SimpleGrid>

                    <Divider label="Често задавани въпроси" labelPosition="center" />

                    {/* FAQ Section */}
                    <Accordion variant="separated">
                        <Accordion.Item value="account">
                            <Accordion.Control icon={<IconQuestionMark size={ICON_SIZE} />}>
                                Как да създам акаунт и да започна работа с ECondo?
                            </Accordion.Control>
                            <Accordion.Panel>
                                <Text>
                                    За да създадете акаунт в ECondo, посетете нашата начална страница и натиснете бутона "Регистрация".
                                    Въведете вашия имейл, създайте парола и попълнете необходимата информация за вас.
                                    След като потвърдите имейла си, можете да влезете в системата и да започнете да я използвате.
                                </Text>
                                <Text mt="sm">
                                    Ако сте управител на вход, можете да създадете профил на сградата и да въведете данни за входа.
                                    Ако сте собственик или наемател, ще получите покана от вашия управител на входа с връзка за присъединяване.
                                </Text>
                            </Accordion.Panel>
                        </Accordion.Item>

                        <Accordion.Item value="properties">
                            <Accordion.Control icon={<IconHome size={ICON_SIZE} />}>
                                Как да добавя и управлявам имоти в системата?
                            </Accordion.Control>
                            <Accordion.Panel>
                                <Text>
                                    Като управител на вход, можете да добавяте имоти чрез раздела "Имоти" във вашия профил.
                                    Натиснете бутона "Добави имот" и въведете подробности като номер на апартамент, етаж, площ и тип на имота.
                                </Text>
                                <Text mt="sm">
                                    След като имотът е добавен, можете да го свържете със собственици или наематели, като добавите техните данни
                                    и изпратите им покана по имейл. Когато получат поканата, те ще могат да се присъединят към системата със собствен акаунт.
                                </Text>
                                <Text mt="sm">
                                    За да редактирате детайли за имота, отворете страницата на имота и изберете опцията "Редактиране".
                                    За да изтриете имот, използвайте опцията "Изтриване", но имайте предвид, че това ще премахне и всички исторически данни за този имот.
                                </Text>
                            </Accordion.Panel>
                        </Accordion.Item>

                        <Accordion.Item value="billing">
                            <Accordion.Control icon={<IconCreditCard size={ICON_SIZE} />}>
                                Как работи системата за такси и плащания?
                            </Accordion.Control>
                            <Accordion.Panel>
                                <Text>
                                    ECondo ви позволява да създавате и управлявате различни видове такси - еднократни или повтарящи се.
                                    Като управител, можете да създадете такса от раздела "Сметки", като посочите сума, описание, начална дата и опционална крайна дата.
                                </Text>
                                <Text mt="sm">
                                    Можете да създадете повтарящи се такси като избирате интервал - месечно, тримесечно или годишно.
                                    Системата автоматично ще генерира нови сметки за всеки имот според зададения график.
                                </Text>
                                <Text mt="sm">
                                    Собствениците и наемателите ще виждат сметките си в техния профил и могат да платят онлайн чрез интегрираната ни система за плащания,
                                    поддържаща кредитни/дебитни карти. Системата автоматично отбелязва платените сметки и поддържа история на всички плащания.
                                </Text>
                            </Accordion.Panel>
                        </Accordion.Item>

                        <Accordion.Item value="occupants">
                            <Accordion.Control icon={<IconUsers size={ICON_SIZE} />}>
                                Как да управлявам собственици и наематели?
                            </Accordion.Control>
                            <Accordion.Panel>
                                <Text>
                                    За да добавите собственик или наемател към имот, отидете в детайлите на съответния имот и изберете опцията "Добави обитател".
                                    Въведете техните имена и имейл, и изберете типа (собственик или наемател).
                                </Text>
                                <Text mt="sm">
                                    След като добавите обитател, системата ще изпрати автоматична покана на посочения имейл.
                                    Когато обитателят приеме поканата, неговият акаунт ще бъде свързан с имота и ще получи съответните права за достъп.
                                </Text>
                                <Text mt="sm">
                                    Собствениците имат права да видят информацията за имота, сметките и плащанията, както и да добавят или премахват наематели.
                                    Наемателите могат да виждат информацията за имота и сметките си, но не могат да променят детайлите на имота.
                                </Text>
                                <Text mt="sm">
                                    Когато обитател се премести, можете да го премахнете от имота, като използвате опцията "Премахни обитател".
                                    Това ще премахне техния достъп до имота, но ще запази историята на плащанията.
                                </Text>
                            </Accordion.Panel>
                        </Accordion.Item>

                        {/* <Accordion.Item value="maintenance"> */}
                        {/*     <Accordion.Control icon={<IconCalendarTime size={ICON_SIZE} />}> */}
                        {/*         Как да организирам поддръжка и периодични задачи? */}
                        {/*     </Accordion.Control> */}
                        {/*     <Accordion.Panel> */}
                        {/*         <Text> */}
                        {/*             ECondo предлага инструменти за планиране и проследяване на редовна поддръжка и ремонти. */}
                        {/*             Като управител, можете да създавате задачи от раздела "Поддръжка", с описание, приоритет и краен срок. */}
                        {/*         </Text> */}
                        {/*         <Text mt="sm"> */}
                        {/*             Можете да задавате повтарящи се задачи, като почистване, градинарство или технически прегледи, и системата */}
                        {/*             ще ви напомня, когато наближи времето за следващото изпълнение. */}
                        {/*         </Text> */}
                        {/*         <Text mt="sm"> */}
                        {/*             За всяка задача можете да прикачвате документи, снимки и бележки, което ви помага да поддържате подробна */}
                        {/*             история на всички дейности по поддръжката. */}
                        {/*         </Text> */}
                        {/*         <Text mt="sm"> */}
                        {/*             Системата също ви позволява да свързвате разходи с конкретни задачи за поддръжка, */}
                        {/*             което улеснява финансовото проследяване и отчитане. */}
                        {/*         </Text> */}
                        {/*     </Accordion.Panel> */}
                        {/* </Accordion.Item> */}
                    </Accordion>

                    {/* <Divider label="Свържете се с нас" labelPosition="center" /> */}

                    {/* Contact Form */}
                    {/* <Paper p="lg" withBorder> */}
                    {/*     <form onSubmit={form.onSubmit(handleSubmit)}> */}
                    {/*         <Grid gutter="md"> */}
                    {/*             <Grid.Col span={12} md={6}> */}
                    {/*                 <TextInput */}
                    {/*                     required */}
                    {/*                     label="Име" */}
                    {/*                     placeholder="Вашето име" */}
                    {/*                     {...form.getInputProps('name')} */}
                    {/*                 /> */}
                    {/*             </Grid.Col> */}
                    {/*             <Grid.Col span={12} md={6}> */}
                    {/*                 <TextInput */}
                    {/*                     required */}
                    {/*                     label="Имейл" */}
                    {/*                     placeholder="вашия@имейл.com" */}
                    {/*                     {...form.getInputProps('email')} */}
                    {/*                 /> */}
                    {/*             </Grid.Col> */}
                    {/*             <Grid.Col span={12}> */}
                    {/*                 <TextInput */}
                    {/*                     required */}
                    {/*                     label="Тема" */}
                    {/*                     placeholder="Тема на запитването" */}
                    {/*                     {...form.getInputProps('subject')} */}
                    {/*                 /> */}
                    {/*             </Grid.Col> */}
                    {/*             <Grid.Col span={12}> */}
                    {/*                 <Textarea */}
                    {/*                     required */}
                    {/*                     label="Съобщение" */}
                    {/*                     placeholder="Вашето запитване или проблем" */}
                    {/*                     minRows={4} */}
                    {/*                     {...form.getInputProps('message')} */}
                    {/*                 /> */}
                    {/*             </Grid.Col> */}
                    {/*             <Grid.Col span={12}> */}
                    {/*                 <Group position="right"> */}
                    {/*                     <Button type="submit" loading={isSubmitting}> */}
                    {/*                         Изпрати съобщение */}
                    {/*                     </Button> */}
                    {/*                 </Group> */}
                    {/*             </Grid.Col> */}
                    {/*         </Grid> */}
                    {/*     </form> */}
                    {/* </Paper> */}
                </Stack>
            </Paper>
        </Container>
    );
};
