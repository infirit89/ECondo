'use client';

import { Button, Card, Center, Container, Flex, Group, Pagination, SimpleGrid, Title, Text } from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import { useParams } from "next/navigation";
import BillModal from "./billModal";
import { checkStripeStatus, connectToStripe, getBillsForEntrance, getStripeLoginLink } from "@/actions/condo";
import { useQuery } from "@tanstack/react-query";
import { queryKeys } from "@/types/queryKeys";
import Loading from "@/components/loading";
import { useState } from "react";
import { IconMoodPuzzled } from "@tabler/icons-react";

// hard coded for now
const pageSize = 9;

const useCheckStripeStatusQuery = (buildingId: string, entranceNumber: string) => {
    return useQuery({
        queryKey: queryKeys.stripe.checkStatus(buildingId, entranceNumber),
        queryFn: () => checkStripeStatus(buildingId, entranceNumber),
    });
}

const useBillsForEntranceQuery = (buildingId: string, entranceNumber: string, page: number) => {
    return useQuery({
        queryKey: queryKeys.bills.pagedForEntrance(buildingId, entranceNumber, page, pageSize),
        queryFn: () => getBillsForEntrance(buildingId, entranceNumber, page, pageSize),
    });
}

export default function BillsPage() {
    const { buildingId, entranceNumber } = useParams<{
        buildingId: string,
        entranceNumber: string }>();

    const [opened, { open, close }] = useDisclosure(false);
    const [isConnectingToStripe, setConnectingToStripe] = useState(false);

    const { data: stripeStatus, isLoading } = useCheckStripeStatusQuery(
        buildingId, 
        entranceNumber);

    const [page, setPage] = useState(0);
    const { data: bills, isLoading: isLoadingBills } = useBillsForEntranceQuery(
        buildingId,
        entranceNumber,
        page,
    );

    const handleConnectToStripe = async () => {
        setConnectingToStripe(true);
        const res = await connectToStripe(buildingId, entranceNumber);

        if(!res.ok) {
            console.log(res.error);
            setConnectingToStripe(false);
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

    if(isLoading || !stripeStatus?.ok || isLoadingBills || !bills?.ok)
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
                        disabled={isConnectingToStripe}
                        onClick={stripeStatus.value?.chargesEnabled ? 
                        handleLoginToStripe : 
                        handleConnectToStripe}>
                            {stripeStatus.value?.chargesEnabled ? 'Виж плащания' : 'Свържи плащания'}
                        </Button>
                        <Button onClick={open}>Нов разход</Button>
                    </Group>
                </Flex>

                {
                    bills.value && bills.value.items.length > 0 ?
                    <>
                        <Flex justify={'center'} mt={'lg'} mb={'lg'} hiddenFrom='lg'>
                            <Pagination total={bills.value.totalPages}
                            value={page + 1}
                            onChange={(value) => setPage(value - 1)}/>
                        </Flex>
                        <SimpleGrid
                        cols={{ base: 1, md: 2, lg: 3 }}
                        spacing={{ base: 'sm', md: 'md', lg: 'lg' }}>
                            {
                                bills
                                .value
                                .items.map((value, index) => (
                                <Card key={index} shadow="sm" padding="lg">
                                    <Text fw={500}>{value.title}</Text>
                                    {
                                        value.description && (
                                            <Text>{value.description}</Text>        
                                        )
                                    }
                                    <Text>${value.amount.toFixed(2)}</Text>
                                </Card>
                                ))    
                            }
                        </SimpleGrid>
                        <Flex justify={'center'} mt={'xl'}>
                            <Pagination total={bills.value.totalPages}
                            value={page + 1}
                            onChange={(value) => setPage(value - 1)}/>
                        </Flex>
                    </>
                    : 
                    <>
                        <Center mt={90} mb={20}>
                            <IconMoodPuzzled size={100} color="#868e96"/>
                        </Center>
                        <Center>
                            <Title c={'dimmed'}>Този вход няма регистрирани сметки</Title>
                        </Center>
                    </> 
                }
            </Container>
        </>
    );
}