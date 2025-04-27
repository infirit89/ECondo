import { getAllOccupantTypes } from "@/actions/propertyOccupant";
import Loading from "@/components/loading";
import OccupantTypeProvider from "@/providers/occupantTypeProvider";
import { Suspense } from "react";
import OccupantPageContent from "./occupantPageContent";

export default function Page() {
    const occupantTypes = getAllOccupantTypes();
    
    return (
        <Suspense fallback={<Loading/>}>
            <OccupantTypeProvider occupantTypes={occupantTypes}>
                <OccupantPageContent/>
            </OccupantTypeProvider>
        </Suspense>
    );
}