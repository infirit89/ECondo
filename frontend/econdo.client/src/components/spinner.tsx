import { Center, Loader } from "@mantine/core";

interface SpinnerProps {
    h?: string;
}

export default function Spinner({h = '100vh'}: SpinnerProps) {

    return (
        <Center h={h}>
            <Loader size={'lg'} type={'dots'}/>
        </Center>
    );
}