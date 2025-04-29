import type { Metadata } from "next";
import "./globals.css";

import '@mantine/core/styles.css';
import '@mantine/dates/styles.css';
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
import { DatesProvider } from '@mantine/dates';
import 'dayjs/locale/bg';


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
              <DatesProvider settings={{ locale: 'bg', timezone: 'UTC' }}>
                <ModalsProvider>
                  <Suspense fallback={<Loading height={'100vh'}/>}>
                    {children}
                  </Suspense>
                </ModalsProvider>
              </DatesProvider>
            </QueryProvider>
          </HealthProvider>
        </MantineProvider>
      </body>
    </html>
  );
}
