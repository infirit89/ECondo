import { isUserInBuilding } from "@/actions/condo";
import DashboardSidebar, { ActiveTab } from "@/components/dashboardSidebar/dashboardSidebar";
import { UserNavbar } from "@/components/navbar/userNavbar";
import { AppShell, AppShellFooter, AppShellMain } from "@mantine/core";
import { notFound, redirect } from "next/navigation";
import { ReactNode, Suspense } from "react";

export default async function Dashboard(
  { properties, ownership, bills, params }:
  Readonly<{
    properties: ReactNode,
    ownership: ReactNode,
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
      case 'ownership':
        return ownership;
      case 'bills':
        return bills;
      default:
        notFound();
    }
  }

  return (
    <AppShell 
    header={{ height: 60 }}
    navbar={{ width: 300, breakpoint: 'sm', collapsed: { mobile: true }} }>
        <UserNavbar />
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
