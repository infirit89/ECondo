'use client';

import { PropertyTypeNameResult } from "@/actions/property"
import { Result } from "@/types/result";
import { createContext, ReactNode, use, useContext } from "react"

type PropertyTypeContextType = {
    propertyTypes: PropertyTypeNameResult,
}

const PropertyTypeContext =
    createContext<PropertyTypeContextType | undefined>(undefined);

export default function PropertyTypeProvider(
{children, propertyTypes } : 
{ children: ReactNode, propertyTypes: Promise<Result<PropertyTypeNameResult>> }) {
    
    const allPropertyTypes = use(propertyTypes);
    if(!allPropertyTypes.ok || !allPropertyTypes.value)
        throw new Error('Unexpected result');

    return (
        <PropertyTypeContext.Provider 
        value={{
            propertyTypes: allPropertyTypes.value,
        }}>
            {children}
        </PropertyTypeContext.Provider>
    )
}

export function usePropertyTypes() {
    const context = useContext(PropertyTypeContext);
    if (context === undefined)
        throw new Error("usePropertyTypes must be used within a PropertyTypeProvider");

    return context;
}

