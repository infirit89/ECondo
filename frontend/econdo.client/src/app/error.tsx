'use client';

import { useEffect } from "react";
import { Button, Container, Group, Text, Title } from '@mantine/core';
import classes from './error.module.css';
import Router from "next/router";

export default function ErrorPage({
    error,
    reset,
} : {
    error: Error & {digest?: string},
    reset: () => void
}) {
    useEffect(() => {
        console.error(error);
    }, [error]);

    return (
        <div className={classes.root}>
            <Container>
            <div className={classes.label}>500</div>
            <Title className={classes.title}>Опа, случи се грешка...</Title>
            <Text size="lg" ta="center" className={classes.description}>
                Нашите сървъри не можаха да обработят заявката ви.
                Не се притеснявайте, нашият екип e уведомен.
                Опитайте да обновите страницата.
            </Text>
            <Group justify="center">
                <Button variant="white" size="md" onClick={() => reset()}>
                Обнови страницата
                </Button>
            </Group>
            </Container>
        </div>
    );
}