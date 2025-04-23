import { getAllPropertyTypes } from "@/actions/property";
import PropertyPageContent from "./propertyPageContent";
import { Suspense } from "react";
import Loading from "@/components/loading";

export default function PropertiesPage() {
    const propertyTypes = getAllPropertyTypes();

    return (
        <Suspense fallback={<Loading/>}>
            <PropertyPageContent propertyTypes={propertyTypes}/>
        </Suspense>
    );
}
