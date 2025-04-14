import type { Metadata } from "next";
import "./globals.css";

import '@mantine/core/styles.css';
import { ColorSchemeScript, mantineHtmlProps, MantineProvider } from "@mantine/core";
import App from "@/components/app/app";
import { isAuthenticated } from "@/actions/auth";
import { getBriefProfile } from "@/actions/profile";
import { BriefProfileResponse } from "@/types/profileData";

export const metadata: Metadata = {
  title: "ECondo",
  description: "A condomium managment app",
};

export default async function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  const authenticated = await isAuthenticated();
  let profileData: BriefProfileResponse | undefined = undefined;
  if(authenticated) {
      try {
          profileData = await getBriefProfile();
      }
      catch(error) {
          console.error(error);
      }
  }

  return (
    <html lang="en" {...mantineHtmlProps}>
      <head>
        <ColorSchemeScript />
        <meta name="apple-mobile-web-app-title" content="ECondo" />
      </head>
      <body>
        <MantineProvider>
          <App isAuthenticated={authenticated} profileData={profileData}>
            {children}
          </App>
        </MantineProvider>
      </body>
    </html>
  );
}
