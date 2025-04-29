import { Center, Loader } from "@mantine/core";

export default function Loading() {
    
    return (
        <Center h={'20vh'}>
            <Loader size={'lg'} type={'dots'}/>
        </Center>
    );
}
