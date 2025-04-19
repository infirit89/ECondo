import { getPropertiesInEntrance } from "@/actions/property";

export default async function PropertiesPage({ params } : 
    Readonly<{ 
        params: Promise<{
            buildingId: string,
            entranceNumber: string
        }> 
    }>) {
    const {buildingId, entranceNumber} = await params;

    const propertiesRes = await getPropertiesInEntrance(buildingId, entranceNumber, 0, 5);
    console.log(propertiesRes);
    
    
    return (
        <>
            Properties
        </>
    );
}