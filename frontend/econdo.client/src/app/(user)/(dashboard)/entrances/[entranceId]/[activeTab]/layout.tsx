import { isUserInBuilding } from "@/actions/condo";
import DashboardSidebar, { ActiveTab } from "./dashboardSidebar";
import { UserNavbar } from "@/components/navbar/userNavbar";
import { AppShell, AppShellMain } from "@mantine/core";
import { redirect } from "next/navigation";
import { ReactNode } from "react";
import Footer from "@/components/footer";

export default async function Dashboard(
  { children, properties, bills, params }:
  Readonly<{
    children: ReactNode,
    properties: ReactNode,
    bills: ReactNode,
    params: Promise<{
      entranceId: string, activeTab: ActiveTab
    }>
  }>) {


  const {
    entranceId,
    activeTab } = await params;

  const result = await isUserInBuilding(entranceId);

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
        { link: `/buildings/${entranceId}/properties`, label: 'Имоти' },
        { link: `/buildings/${entranceId}/bills`, label: 'Сметки' },
      ]}/>
      <DashboardSidebar 
      activeTab={activeTab}
      entranceId={entranceId}/>
      <AppShellMain>
          { getChild() }
      </AppShellMain>
      <Footer/>
  </AppShell>
  );
}
