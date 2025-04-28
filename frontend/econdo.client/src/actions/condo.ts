'use server';

import normalInstance, { authInstance } from "@/lib/axiosInstance";
import { ApiError, PagedList } from "@/types/apiResponses";
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

export const getBuildingsForUser = 
cache(async (page: number, pageSize: number, buildingName?: string): 
Promise<Result<PagedList<BuildingResult>>> => {
    try {
        const buildings = await authInstance.get<PagedList<BuildingResult>>(
            '/api/building/getBuildingsForUser', {
                params: {
                    page,
                    pageSize,
                    buildingName: buildingName,
                }
            });
        return resultOk(buildings.data);
    }
    catch(error) {
        if(isAxiosError<ApiError>(error))
            return resultFail(error.response?.data!);
    }

    throw new Error('Unexpected code flow');
});

export const isUserInBuilding = cache(async (buildingId: string, entranceNumber: string): Promise<Result> => {
    try {
        await authInstance
            .get(`/api/building/isEntranceManager?buildingId=${buildingId}&entranceNumber=${entranceNumber}`);
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

export const getAllBuildings =
cache(async (page: number, pageSize: number):
Promise<Result<PagedList<BuildingResult>>> => {
    try {
        const res = await authInstance.get<PagedList<BuildingResult>>(
            '/api/building/getAll', {
            params: {
                page,
                pageSize,
            }
        });
        return resultOk(res.data);
    } catch(error) {
        if(isAxiosError<ApiError>(error))
            return resultFail(error.response?.data!);
    }

    throw new Error('Unexpected code flow');
});

export const deleteEntrance =
cache(async (buildingId: string, entranceNumber: string) => {
    try {
        await authInstance.delete(
            '/api/building/delete', {
            params: {
                buildingId,
                entranceNumber,
            },
        });

        return resultOk();
    } catch(error) {
        if(isAxiosError<ApiError>(error))
            return resultFail(error.response?.data!);
    }

    throw new Error('Unexpected code flow');
});
