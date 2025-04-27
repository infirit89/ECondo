import { UserNavbar } from "@/components/navbar/userNavbar";
import { AppShell, AppShellFooter, AppShellMain } from "@mantine/core";
import DashboardSidebar, { ActiveTab } from "./dashboardSidebar";
import { ReactNode } from "react";
import { isUserOccupant } from "@/actions/propertyOccupant";
import { redirect } from "next/navigation";
import Footer from "@/components/footer";

export default async function PropertyLayout({ children, occupants, bills, params } :
    Readonly<{
        children: ReactNode,
        occupants: ReactNode,
        bills: ReactNode,
        params: Promise<{ slug: string[] }> }>) {
    
    const {
        slug
    } = await params;

    const propertyId: string = slug[0];
    const activeTab = slug[1];

    const result = await isUserOccupant(propertyId);

    if(!result.ok)
        redirect('/condos/properties');

    const occupantType = result.value?.occupantType;

    if(activeTab === undefined)
    {
        if(occupantType === 'Собственик')
            redirect(`/properties/${propertyId}/occupants`);
        else
            redirect(`/properties/${propertyId}/bills`);
    }

    function getChild(): ReactNode {
        switch (activeTab) {
        case 'occupants':
            if(occupantType !== 'Собственик')
                return children;

            return occupants;
        case 'bills':
            return bills;
        default:
            return children;
        }
    }

    const child = getChild();

    return (
    <AppShell 
    header={{ height: 60 }}
    navbar={{ width: 300, breakpoint: 'sm', collapsed: { mobile: true }} }>
        <UserNavbar additionalLinks={occupantType === 'Собственик' ? [
            { link: `/properties/${propertyId}/occupants`, label: 'Контакти' },
            { link: `/properties/${propertyId}/bills`, label: 'Сметки' },
        ] : [
            { link: `/properties/${propertyId}/bills`, label: 'Сметки' },
        ]}/>
        <DashboardSidebar 
        activeTab={activeTab as ActiveTab}
        propertyId={propertyId}
        isOwner={occupantType === 'Собственик'}/>
        <AppShellMain>
            { child }
        </AppShellMain>
        <Footer/>
    </AppShell>
    )
}