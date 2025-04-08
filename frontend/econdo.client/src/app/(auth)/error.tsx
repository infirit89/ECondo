'use client';

import { useEffect } from "react";
import { Button, Container, Group, Text, Title } from '@mantine/core';
import classes from './error.module.css';

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
            <Title className={classes.title}>Something bad just happened...</Title>
            <Text size="lg" ta="center" className={classes.description}>
                Our servers could not handle your request. Don&apos;t worry, our development team was
                already notified. Try refreshing the page.
            </Text>
            <Group justify="center">
                <Button variant="white" size="md" onClick={() => reset()}>
                Refresh the page
                </Button>
            </Group>
            </Container>
        </div>
    );
}