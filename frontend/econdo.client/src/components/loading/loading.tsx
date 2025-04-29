import { Center, Loader, StyleProp } from "@mantine/core";

export function Loading({ height } : {height?: StyleProp<React.CSSProperties['height']>}) {
    
    return (
        <Center h={height}>
            <Loader size={'lg'} type={'dots'}/>
        </Center>
    );
}
