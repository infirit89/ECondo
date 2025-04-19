import type { Metadata } from "next";
import "./globals.css";

import '@mantine/core/styles.css';
import { 
  ColorSchemeScript,
  mantineHtmlProps, 
  MantineProvider 
} from "@mantine/core";

import HealthProvider from "./healthProvider";

export const metadata: Metadata = {
  title: "ECondo",
  description: "A condomium managment app",
};

export default async function RootLayout({
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
              {children}
          </HealthProvider>
        </MantineProvider>
      </body>
    </html>
  );
}
