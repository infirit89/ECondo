import { Container } from "@mantine/core";
import ProfileContainer from "@/components/profileContainer";

export default function Profile() {

    return (
        <Container size={820} my={100} mb={100}>
            <ProfileContainer/>
        </Container>
    );
}
