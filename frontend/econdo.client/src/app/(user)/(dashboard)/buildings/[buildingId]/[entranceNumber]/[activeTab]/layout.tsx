import { isUserInBuilding } from "@/actions/condo";
import DashboardSidebar, { ActiveTab } from "./dashboardSidebar";
import { UserNavbar } from "@/components/navbar/userNavbar";
import { AppShell, AppShellFooter, AppShellMain } from "@mantine/core";
import { notFound, redirect } from "next/navigation";
import { ReactNode } from "react";

export default async function Dashboard(
  { children, properties, bills, params }:
  Readonly<{
    children: ReactNode,
    properties: ReactNode,
    bills: ReactNode,
    params: Promise<{
      buildingId: string, entranceNumber: string, activeTab: ActiveTab
    }>
  }>) {


  const {
    buildingId,
    entranceNumber,
    activeTab } = await params;

  const result = await isUserInBuilding(
    buildingId,
    entranceNumber);

  if(!result.ok)
    redirect('/condos/buildings');

  function getChild(): ReactNode {
    switch (activeTab) {
      case 'properties':
        return properties;
      case 'bills':
        return bills;
      default:
        return children;
    }
  }

  return (
    <AppShell 
    header={{ height: 60 }}
    navbar={{ width: 300, breakpoint: 'sm', collapsed: { mobile: true }} }>
        <UserNavbar additionalLinks={[
          { link: `/buildings/${buildingId}/${entranceNumber}/properties`, label: 'Имоти' },
          { link: `/buildings/${buildingId}/${entranceNumber}/bills`, label: 'Сметки' },
        ]}/>
        <DashboardSidebar 
        activeTab={activeTab}
        buildingId={buildingId}
        entranceNumber={entranceNumber}/>
        <AppShellMain>
            { getChild() }
        </AppShellMain>
        <AppShellFooter>
            
        </AppShellFooter>
    </AppShell>
  );
}
