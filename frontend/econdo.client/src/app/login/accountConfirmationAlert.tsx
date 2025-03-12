import { Alert, Anchor, List, ListItem, Text } from "@mantine/core";

export default function AccountConfirmationAlert({ hidden }: {hidden: boolean}) {
    return (
        <Alert variant="light" mb={'md'} hidden={hidden}>
            <Text fz={'sm'}>Акаунтът Ви беше създаден успешно, проверете имейла си за потвърждение.</Text>
            <Text fz={'sm'} mt={'xs'}>Не го намирате?</Text>
            <List type={'unordered'}>
                <ListItem fz={'sm'}>Проверете папка спам.</ListItem>
                <ListItem fz={'sm'}><Anchor href="#" fz={'sm'}>Повторно изпращане</Anchor></ListItem>
            </List>
        </Alert>
    );
}