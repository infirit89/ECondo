'use server';

import authInstance, { normalInstance } from "@/lib/axiosInstance";
import { ApiError } from "@/types/apiResponses";
import { Result, resultFail, resultOk } from "@/types/result";
import { isAxiosError } from "axios";
import { unstable_cache } from "next/cache";
import { cache } from "react";

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

export const getBuildingsForUser = cache(async (): Promise<Result<BuildingResult[]>> => {
    try {
        const buildings = await authInstance.get<BuildingResult[]>('/api/building/getBuildingsForUser');
        return resultOk(buildings.data);
    }
    catch(error) {
        if(isAxiosError<ApiError>(error))
            return resultFail(error.response?.data!);
    }

    throw new Error('Unexpected code flow');
});

export const isUserInBuilding = cache(async (buildingId: string): Promise<Result> => {
    try {
        await authInstance.get(`/api/building/isInBuilding?buildingId=${buildingId}`);
        return resultOk();
    }
    catch(error) {
        if(isAxiosError<ApiError>(error))
            return resultFail(error.response?.data!);
    }

    throw new Error('Unexpected code flow');
});

export interface RegisterBuilding {
    buildingName: string,
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

export async function registerBuildingEntrance(building: RegisterBuilding): Promise<Result> {
    try {
        await authInstance.post('/api/building/registerBuildingEntrance', building);
        return resultOk();
    } catch(error) {
        if(isAxiosError<ApiError>(error))
            return resultFail(error.response?.data!);
    }

    throw new Error('Unexpected code flow');
}

export interface ProvinceNameResult {
    provinces: string[],
}

export const getProvinces = unstable_cache(async (): Promise<ProvinceNameResult> => {
    return  (await normalInstance
        .get<ProvinceNameResult>('/api/province/getProvinces')).data;
});
