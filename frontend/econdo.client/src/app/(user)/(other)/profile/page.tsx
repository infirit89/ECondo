import { AppShellMain, Container } from "@mantine/core";
import ProfileContainer from "@/components/profile/profileContainer";

export default function Profile() {

    return (
        <AppShellMain>
            <Container size={820} mb={100} mt={50}>
                <ProfileContainer/>
            </Container>
        </AppShellMain>
    );
}
