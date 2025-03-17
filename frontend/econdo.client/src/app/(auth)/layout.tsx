import { AppShellMain } from "@mantine/core";

export default function AuthLayout({children }:  Readonly<{
    children: React.ReactNode;
  }>) {
    return (
        <AppShellMain>
            {children}
        </AppShellMain>
    );
}