import { checkHealth } from "@/actions/health";
import { ReactNode } from "react";

export default async function HealthProvider(
    { children } : Readonly<{ children: ReactNode }>
) {
    await checkHealth();
    return (
        <>
            {children}
        </>
    );
}