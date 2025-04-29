'use client';

import { OccupantTypeNameResult } from "@/actions/propertyOccupant";
import { Result } from "@/types/result";
import { createContext, ReactNode, use, useContext } from "react"

type OccupantTypeContextType = {
    occupantTypes: OccupantTypeNameResult,
}

const OccupantTypeContext =
    createContext<OccupantTypeContextType | undefined>(undefined);

export default function OccupantTypeProvider(
{children, occupantTypes } : 
{ children: ReactNode, occupantTypes: Promise<Result<OccupantTypeNameResult>> }) {
    
    const allOccupantTypes = use(occupantTypes);
    if(!allOccupantTypes.ok || !allOccupantTypes.value)
        throw new Error('Unexpected result');

    return (
        <OccupantTypeContext.Provider 
        value={{
            occupantTypes: allOccupantTypes.value,
        }}>
            {children}
        </OccupantTypeContext.Provider>
    )
}

export function useOccupantTypes() {
    const context = useContext(OccupantTypeContext);
    if (context === undefined)
        throw new Error("useOccupantTypes must be used within a OccupantTypeProvider");

    return context;
}