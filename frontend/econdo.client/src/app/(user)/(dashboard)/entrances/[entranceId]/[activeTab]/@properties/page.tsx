import { getAllPropertyTypes } from "@/actions/property";
import PropertyPageContent from "./propertyPageContent";
import { Suspense } from "react";
import Loading from "@/components/loading";
import { getAllOccupantTypes } from "@/actions/propertyOccupant";
import PropertyTypeProvider from "@/providers/propertyTypeProvider";
import OccupantTypeProvider from "@/providers/occupantTypeProvider";

export default function PropertiesPage() {
    const propertyTypes = getAllPropertyTypes();
    const occupantTypes = getAllOccupantTypes();

    return (
        <Suspense fallback={<Loading/>}>
            <PropertyTypeProvider propertyTypes={propertyTypes}>
                <OccupantTypeProvider occupantTypes={occupantTypes}>
                    <PropertyPageContent/>
                </OccupantTypeProvider>
            </PropertyTypeProvider>
        </Suspense>
    );
}
