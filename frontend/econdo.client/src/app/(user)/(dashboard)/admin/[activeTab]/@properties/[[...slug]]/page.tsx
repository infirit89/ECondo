import { getAllPropertyTypes } from "@/actions/property";
import { getAllOccupantTypes } from "@/actions/propertyOccupant";
import Loading from "@/components/loading";
import { Suspense } from "react";
import PropertiesPageContent from "./propertiesPageContent";
import OccupantTypeProvider from "@/providers/occupantTypeProvider";
import PropertyTypeProvider from "@/providers/propertyTypeProvider";

export default function PropertiesPage() {

    const propertyTypes = getAllPropertyTypes();
    const occupantTypes = getAllOccupantTypes();
    return (
        <Suspense fallback={<Loading/>}>
            <PropertyTypeProvider propertyTypes={propertyTypes}>
                <OccupantTypeProvider occupantTypes={occupantTypes}>
                    <PropertiesPageContent/>
                </OccupantTypeProvider>
            </PropertyTypeProvider>
        </Suspense>
    );
}