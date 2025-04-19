'use server';

import { authInstance } from "@/lib/axiosInstance";
import { PagedList, ApiError } from "@/types/apiResponses";
import { Result, resultFail, resultOk } from "@/types/result";
import { isAxiosError } from "axios";
import { cache } from "react";

export interface BriefPropertyResult {
    id: string,
    floor: number,
    number: number,
    propertyType: string,
}

export const getPropertiesInEntrance = 
    cache(async (buildingId: string, entranceNumber: string, page: number, pageSize: number): 
        Promise<Result<PagedList<BriefPropertyResult>>> => {
    try {
        const properties = await authInstance
        .get<PagedList<BriefPropertyResult>>('/api/property/getPropertiesInBuilding', { 
            params: {
                buildingId,
                entranceNumber,
                page,
                pageSize
            },
        });

        return resultOk(properties.data);
    }
    catch(error) {
        if(isAxiosError<ApiError, Record<string, string[]>>(error))
            return resultFail(error.response?.data!);
    }


    throw new Error('Unexpected code flow');
});