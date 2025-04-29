'use client';

import { getPaymentsForProperty } from "@/actions/property";
import Loading from "@/components/loading";
import { queryKeys } from "@/types/queryKeys";
import { Button, Card, Center, Flex, Pagination, SimpleGrid, Title, Text, Container } from "@mantine/core";
import { IconMoodPuzzled } from "@tabler/icons-react";
import { useQuery } from "@tanstack/react-query";
import { useParams } from "next/navigation";
import { useState } from "react";
import CheckoutModal from "./checkoutModal";

// hard coded for now
const pageSize = 9;

const usePaymentsPagedQuery = (propertyId: string, page: number) => {
    return useQuery({
        queryKey: queryKeys.payments.pagedForProperty(
            propertyId,
            page,
            pageSize),
        queryFn: () => getPaymentsForProperty(
            propertyId,
            page,
            pageSize),
    })
}

export default function BillsPage() {
    const { slug } = useParams<{ slug: string[] }>();
    const propertyId = slug[0];
    const [page, setPage] = useState(0);

    const [paymentId, setPaymentId] = useState<string | undefined>(undefined);
    const [amountPaid, setAmountPaid] = useState(0);
    const [ isCheckoutOpen, setCheckoutOpen ] = useState(false);
    const { data: payments, isLoading } = usePaymentsPagedQuery(propertyId, page);
    
    if(isLoading || !payments?.ok)
        return <Loading/>;

    return (
        <Container size="lg" py="xl">
            <CheckoutModal 
            id={paymentId!} 
            opened={isCheckoutOpen} 
            onClose={() => setCheckoutOpen(false)}
            amountPaid={amountPaid}/>
            {
            payments.value && payments.value.items.length > 0 ?
            <>
                <Flex justify={'center'} mt={'lg'} mb={'lg'} hiddenFrom='lg'>
                    <Pagination total={payments.value.totalPages}
                    value={page + 1}
                    onChange={(value) => setPage(value - 1)}/>
                </Flex>
                <SimpleGrid
                cols={{ base: 1, md: 2, lg: 3 }}
                spacing={{ base: 'sm', md: 'md', lg: 'lg' }}>
                    {
                        payments
                        .value
                        .items.map((value, index) => (
                        <Card key={index} shadow="sm" padding="lg">
                            <Text fw={500}>{value.billTitle}</Text>
                            <Text>${value.amountPaid.toFixed(2)}</Text>
                            <Button
                                mt="md"
                                onClick={() => {
                                    setPaymentId(value.id)
                                    setCheckoutOpen(true);
                                    setAmountPaid(value.amountPaid);
                                }}
                            >
                                Плати
                            </Button>
                        </Card>
                        ))    
                    }
                </SimpleGrid>
                <Flex justify={'center'} mt={'xl'}>
                    <Pagination total={payments.value.totalPages}
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
                    <Title c={'dimmed'}>Този имот няма регистрирани разходи</Title>
                </Center>
            </> 
        }
        </Container>
    );
}