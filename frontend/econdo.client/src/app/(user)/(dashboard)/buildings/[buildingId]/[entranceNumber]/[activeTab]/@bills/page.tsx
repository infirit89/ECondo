'use client';

import { Button, Container, Flex, Group, Title } from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import { useParams } from "next/navigation";
import BillModal from "./billModal";
import { checkStripeStatus, connectToStripe, getStripeLoginLink } from "@/actions/condo";
import { useQuery } from "@tanstack/react-query";
import { queryKeys } from "@/types/queryKeys";
import Loading from "@/components/loading";

const useCheckStripeStatusQuery = (buildingId: string, entranceNumber: string) => {
    return useQuery({
        queryKey: queryKeys.stripe.checkStatus(buildingId, entranceNumber),
        queryFn: () => checkStripeStatus(buildingId, entranceNumber),
    });
}

export default function BillsPage() {
    const { buildingId, entranceNumber } = useParams<{
        buildingId: string,
        entranceNumber: string }>();

    const [opened, { open, close }] = useDisclosure(false);
    
    const { data: stripeStatus, isLoading } = useCheckStripeStatusQuery(
        buildingId, 
        entranceNumber);
    const handleConnectToStripe = async () => {
        const res = await connectToStripe(buildingId, entranceNumber);

        if(!res.ok) {
            console.log(res.error);
            return;
        }

        console.log(res);

        if(res.value)
            window.location.href = res.value.url;
    }

    const handleLoginToStripe = async () => {
        const res = await getStripeLoginLink(buildingId, entranceNumber);

        if(!res.ok) {
            console.log(res.error);
            return;
        }

        console.log(res);

        if(res.value) {
            const newTab = window.open(res.value.url, '_blank');
            if(newTab)
                newTab.focus();
        }
    }

    if(isLoading || !stripeStatus?.ok)
        return <Loading/>;

    return (
        <>
            <BillModal 
            isOpen={opened} 
            onClose={close} 
            buildingId={buildingId} 
            entranceNumber={entranceNumber}/>
            <Container size="lg" py="xl">
                <Flex justify={'space-between'} mb={'md'}>
                    <Title>Разходи</Title>
                    <Group gap={'xl'}>
                        <Button 
                        onClick={stripeStatus.value?.chargesEnabled ? 
                        handleLoginToStripe : 
                        handleConnectToStripe}>
                            {stripeStatus.value?.chargesEnabled ? 'Виж плащания' : 'Свържи плащания'}
                        </Button>
                        <Button onClick={open}>Нов разход</Button>
                    </Group>
                </Flex>
            </Container>
        </>
    );
}