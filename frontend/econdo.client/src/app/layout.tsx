import type { Metadata } from "next";
import "./globals.css";

import '@mantine/core/styles.css';
import { 
  ColorSchemeScript,
  mantineHtmlProps, 
  MantineProvider 
} from "@mantine/core";

import App from "@/components/app/app";
import { isAuthenticated } from "@/actions/auth";
import { getBriefProfile } from "@/actions/profile";
import { BriefProfileResponse } from "@/types/profileData";
import { checkHealth } from "@/actions/health";

export const metadata: Metadata = {
  title: "ECondo",
  description: "A condomium managment app",
};

export default async function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  const isHealthy = await checkHealth();
  const authenticated = await isAuthenticated();
  let profileData: BriefProfileResponse | undefined = undefined;
  if(authenticated) {
      const profileRes = await getBriefProfile();
      if(profileRes.ok)
        profileData = profileRes.value;
  }

  return (
    <html lang="en" {...mantineHtmlProps}>
      <head>
        <ColorSchemeScript />
        <meta name="apple-mobile-web-app-title" content="ECondo" />
      </head>
      <body>
        <MantineProvider>
          <App 
          isAuthenticated={authenticated} 
          profileData={profileData}>
            {children}
          </App>
        </MantineProvider>
      </body>
    </html>
  );
}
