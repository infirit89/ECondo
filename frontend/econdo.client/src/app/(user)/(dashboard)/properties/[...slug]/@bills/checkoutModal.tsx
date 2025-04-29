'use client';

import { createIntent } from "@/actions/property";
import Loading from "@/components/loading";
import { queryKeys } from "@/types/queryKeys";
import { Button, Modal, Stack, Text } from "@mantine/core";
import { useQuery } from "@tanstack/react-query";
import { CardElement, Elements, useElements, useStripe } from '@stripe/react-stripe-js';
import { stripePromise } from "@/lib/stripe";

const usePaymentIntentQuery = (paymentId: string) => {
    return useQuery({
        queryKey: queryKeys.payments.createIntent(paymentId),
        queryFn: () => createIntent(paymentId),
    });
}

export default function CheckoutModal(
    { opened, onClose, id, amountPaid }: 
    { opened: boolean, onClose: () => void, id: string, amountPaid: number }) {

    return (
        <Modal opened={opened} onClose={onClose}>
            {
                opened && (
                    <CheckoutData id={id} amountPaid={amountPaid}/>
                )
            }
        </Modal>
    )
}

function CheckoutData({ id, amountPaid }: {id: string, amountPaid: number }) {
    const {data: clientSecretRes, isLoading} = usePaymentIntentQuery(id);
        
    if(isLoading || !clientSecretRes?.ok)
        return <Loading/>;

    const clientSecret = clientSecretRes.value!.clientSecret;

    return (
        <Elements stripe={stripePromise} options={{clientSecret}}>
            <CheckoutForm amount={amountPaid} clientSecret={clientSecret}/>
        </Elements>
    );
}

function CheckoutForm({ amount, clientSecret }: { amount: number, clientSecret: string }) {
    const stripe = useStripe();
    const elements = useElements();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
    
        if (!stripe || !elements) return;
    
        const result = await stripe.confirmCardPayment(clientSecret, {
            payment_method: {
                card: elements.getElement(CardElement)!,
                billing_details: {
                    address: {
                        postal_code: '1000',
                        country: 'BG',
                    }
                }
            },
        });
    
        if (result.error) {
          console.error(result.error.message);
          alert(result.error.message);
        } else {
          if (result.paymentIntent?.status === "succeeded") {
            alert("Payment successful!");
          }
        }
    };

    return (
    <form onSubmit={handleSubmit}>
        <Stack>
        <Text>Amount: ${amount.toFixed(2)}</Text>
        <CardElement options={{ hidePostalCode: true }} />
        <Button type="submit" mt="md" disabled={!stripe}>
            Pay Now
        </Button>
        </Stack>
    </form>
    );
}