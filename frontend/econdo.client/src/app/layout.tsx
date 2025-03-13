import type { Metadata } from "next";
// import { Geist, Geist_Mono } from "next/font/google";
// import "./globals.css";

import '@mantine/core/styles.css';
import { ColorSchemeScript, mantineHtmlProps, MantineProvider } from "@mantine/core";
import App from "@/components/app";

// const geistSans = Geist({
//   variable: "--font-geist-sans",
//   subsets: ["latin"],
// });

// const geistMono = Geist_Mono({
//   variable: "--font-geist-mono",
//   subsets: ["latin"],
// });

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
          <App>
            {children}
          </App>
        </MantineProvider>
      </body>
    </html>
  );
}
