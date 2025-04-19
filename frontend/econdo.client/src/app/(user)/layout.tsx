import { getBriefProfile } from "@/actions/profile";
import AppProvider from "@/components/appProvider/appProvider";
import { ReactNode, Suspense } from "react";

export const experimental_ppr = true;

export default async function UserLayout({ children } : Readonly<{children: ReactNode}>) {

    const profileRes = await getBriefProfile();

    if(!profileRes.ok)
        throw new Error();


    return (
        <AppProvider profileData={profileRes.value}>
            <Suspense fallback={<>Loading</>}>
                {children}
            </Suspense>
        </AppProvider>
    )
}
