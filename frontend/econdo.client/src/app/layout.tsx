import type { Metadata } from "next";
import "./globals.css";

import '@mantine/core/styles.css';
import { ModalsProvider } from '@mantine/modals';
import { 
  ColorSchemeScript,
  mantineHtmlProps, 
  MantineProvider 
} from "@mantine/core";

import HealthProvider from "./healthProvider";
import { Suspense } from "react";
import Loading from "@/components/loading";
import QueryProvider from "./queryProvider";

export const metadata: Metadata = {
  title: "ECondo",
  description: "A condomium managment app",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en" {...mantineHtmlProps}>
      <head>
        <ColorSchemeScript />
        <meta name="apple-mobile-web-app-title" content="ECondo" />
      </head>
      <body>
        <MantineProvider>
          <HealthProvider>
            <QueryProvider>
              <ModalsProvider>
                <Suspense fallback={<Loading height={'100vh'}/>}>
                  {children}
                </Suspense>
              </ModalsProvider>
            </QueryProvider>
          </HealthProvider>
        </MantineProvider>
      </body>
    </html>
  );
}
