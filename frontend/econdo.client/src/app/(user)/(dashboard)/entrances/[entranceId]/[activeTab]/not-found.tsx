import { Button, Container, Group, Text, Title } from '@mantine/core';
import classes from './notfound.module.css';
import Link from 'next/link';

export default function NotFound() {
  return (
    <Container className={classes.root}>
      <div className={classes.label}>404</div>
      <Title className={classes.title}>Намерихте тайно място</Title>
      <Text c="dimmed" size="lg" ta="center" className={classes.description}>
        За съжаление страницата, която търсите, не е намерена. Може да сте въвели грешен адрес или страница е била преместена.
      </Text>
      <Group justify="center">
        <Button component={Link} href="/" variant="subtle" size="md">
          Начална страница
        </Button>
      </Group>
    </Container>
  );
}