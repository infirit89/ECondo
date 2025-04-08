import authInstance from "@/lib/axiosInstance";
import { ValidationError } from "@/types/apiResponses";
import { Result, resultFail, resultOk } from "@/types/result";
import { isAxiosError } from "axios";

export interface BuildingResult {
    id: string,
    name: string,
    provinceName: string,
    municipality: string,
    settlementPlace: string,
    neighborhood: string,
    postalCode: string,
    street: string,
    streetNumber: string,
    buildingNumber: string,
    entranceNumber: string,
}

export async function getBuildingsForUser(): Promise<Result<BuildingResult[]>> {

    try {
        const buildings = await authInstance.get<BuildingResult[]>('/api/building/getBuildingsForUser');
        return resultOk(buildings.data);
    }
    catch(error) {
        if(isAxiosError<ValidationError, Record<string, unknown>>(error))
            return resultFail(error.response?.data!);
    }

    return resultFail(new Error('Unexpected code flow'));
}

export async function isUserInBuilding(buildingId: string): Promise<Result> {
    try {
        await authInstance.get(`/api/building/isInBuilding?buildingId=${buildingId}`);
        return resultOk();
    }
    catch(error) {
        if(isAxiosError<ValidationError, Record<string, unknown>>(error))
            return resultFail(error.response?.data!);
    }

    return resultFail(new Error('Unexpected code flow'));
}