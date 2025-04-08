import { isUserInBuilding } from "@/actions/condo";
import DashboardSidebar from "@/components/dashboardSidebar/dashboardSidebar";
import { AppShellMain, AppShellNavbar } from "@mantine/core";
import { redirect } from "next/navigation";

export default async function Dashboard({ children, params }:  Readonly<{
  children: React.ReactNode;
  params: Promise<{ buildingId: string }>
}>) {

  const { buildingId } = await params;
  const result = await isUserInBuilding(buildingId);

  if(!result.ok)
    redirect('/condos');

  return (
      <>
        <AppShellNavbar>
            <DashboardSidebar/>
        </AppShellNavbar>
        <AppShellMain>
          {children}
        </AppShellMain>
      </>
  );
}
