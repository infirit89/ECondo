import { updateAccessToken } from "@/actions/auth";
import { isUserInBuilding } from "@/actions/condo";
import { getBriefProfile } from "@/actions/profile";
import AppProvider from "@/components/appProvider/appProvider";
import DashboardSidebar from "@/components/dashboardSidebar/dashboardSidebar";
import { UserNavbar } from "@/components/navbar/userNavbar";
import { AppShell, AppShellFooter, AppShellMain, AppShellNavbar } from "@mantine/core";
import { redirect } from "next/navigation";
import { Suspense } from "react";

export default async function Dashboard({ children, params }:  Readonly<{
  children: React.ReactNode;
  params: Promise<{ buildingId: string }>
}>) {

  const { buildingId } = await params;

  await updateAccessToken();
  const profileRes = await getBriefProfile();

  if(!profileRes.ok)
      throw new Error();
  const result = await isUserInBuilding(buildingId);

  if(!result.ok)
    redirect('/condos');

  return (
      <AppProvider profileData={profileRes.value}>
          <AppShell 
          header={{ height: 60 }}
          navbar={{ width: 300, breakpoint: 'sm', collapsed: { mobile: true }} }>
              <UserNavbar 
              {...profileRes.value} />
              <AppShellNavbar>
              <DashboardSidebar/>
          </AppShellNavbar>
              <AppShellMain>
                  <Suspense>
                      {children}
                  </Suspense>
              </AppShellMain>
              <AppShellFooter>
                  
              </AppShellFooter>
          </AppShell>  
      </AppProvider>
  );
}
