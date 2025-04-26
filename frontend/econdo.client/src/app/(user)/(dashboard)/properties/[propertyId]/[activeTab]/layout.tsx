import { UserNavbar } from "@/components/navbar/userNavbar";
import { AppShell, AppShellFooter, AppShellMain } from "@mantine/core";
import DashboardSidebar, { ActiveTab } from "./dashboardSidebar";
import { ReactNode } from "react";

export default async function PropertyLayout({ children, occupants, bills, params } :
    Readonly<{
        children: ReactNode,
        occupants: ReactNode,
        bills: ReactNode,
        params: Promise<{propertyId: string, activeTab: ActiveTab}> }>) {
    
    const {
        propertyId,
        activeTab
    } = await params;

    function getChild(): ReactNode {
        switch (activeTab) {
        case 'occupants':
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
            <UserNavbar additionalLinks={[
                { link: `/properties/${propertyId}/occupants`, label: 'Контакти' },
                { link: `/properties/${propertyId}/bills`, label: 'Сметки' },
            ]}/>
            <DashboardSidebar 
            activeTab={activeTab}
            propertyId={propertyId}/>
            <AppShellMain>
                { child }
            </AppShellMain>
            <AppShellFooter>
                
            </AppShellFooter>
        </AppShell>
    )
}