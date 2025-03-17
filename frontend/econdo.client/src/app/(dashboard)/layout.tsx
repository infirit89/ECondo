import DashboardNavbar from "@/components/dashboardNavbar";
import { AppShellMain, AppShellNavbar } from "@mantine/core";

export default function Dashboard({children }:  Readonly<{
  children: React.ReactNode;
}>) {

    return (
        <>
          <AppShellNavbar>
              <DashboardNavbar/>
          </AppShellNavbar>
          <AppShellMain>
            {children}
          </AppShellMain>
        </>
    );
}