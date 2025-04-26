'use server';

import { authInstance } from "@/lib/axiosInstance";
import { PagedList, ApiError } from "@/types/apiResponses";
import { Result, resultFail, resultOk } from "@/types/result";
import { isAxiosError } from "axios";
import { cache } from "react";

export interface PropertyResult {
    id: string,
    floor: string,
    number: string,
    propertyType: string,
    builtArea: number,
    idealParts: number,
}

export const getPropertiesInEntrance = 
    cache(async (buildingId: string, entranceNumber: string, page: number, pageSize: number): 
        Promise<Result<PagedList<PropertyResult>>> => {
    try {
        const properties = await authInstance
        .get<PagedList<PropertyResult>>('/api/property/getPropertiesInBuilding', { 
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

export const getPropertiesForUser =
cache(async(page: number, pageSize: number): Promise<Result<PagedList<PropertyResult>>> => {
    try {
        const properties = await authInstance
        .get<PagedList<PropertyResult>>('/api/property/getForUser', { 
            params: {
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
})

export interface CreatePropertyData {
    buildingId: string,
    entranceNumber: string,
    propertyType: string,
    floor: string,
    number: string,
    builtArea: number,
    idealParts: number,
}

export const createProperty =
    cache(async (data: CreatePropertyData):
    Promise<Result> => {
        try {
            await authInstance.post('/api/property/create', data);
            return resultOk();
        }
        catch(error) {
            if(isAxiosError<ApiError, Record<string, string[]>>(error))
                return resultFail(error.response?.data!);
        }

        throw new Error('Unexpected code flow');
    });

export interface PropertyTypeNameResult {
    propertyTypes: string[],
}

export const getAllPropertyTypes = 
    cache(async ():
    Promise<Result<PropertyTypeNameResult>> => {
        try {
            const result = await authInstance
                .get<PropertyTypeNameResult>('/api/propertyType/getAll');

            return resultOk(result.data);
        } catch(error) {
            if(isAxiosError<ApiError, Record<string, string[]>>(error))
                return resultFail(error.response?.data!);
        }

        throw new Error('Unexpected code flow');
    });

export const deleteProperty =
    cache(async (propertyId: string):
    Promise<Result> => {
        try {
            await authInstance.delete('/api/property/delete', {
                params: {
                    propertyId,
                },
            });

            return resultOk();
        } catch(error) {
            if(isAxiosError<ApiError, Record<string, string[]>>(error))
                return resultFail(error.response?.data!);
        }

        throw new Error('Unexpected code flow');
    });

export interface Property {
    id: string,
    floor: string,
    number: string,
    propertyType: string,
    builtArea: number,
    idealParts: number,
}

export const getPropertyById =
cache(async (propertyId: string):
Promise<Result<Property>> => {
    try {
        const res = await authInstance.get<Property>('/api/property/getById', {
            params: {
                propertyId,
            }
        });

        return resultOk(res.data);
    } catch(error) {
        if(isAxiosError<ApiError, Record<string, string[]>>(error))
            return resultFail(error.response?.data!);
    }

    throw new Error('Unexpected code flow');
});

export const updateProperty =
cache(async (data: Property):
Promise<Result> => {
    try {
        await authInstance.put('/api/property/update', {
            propertyId: data.id,
            floor: data.floor,
            number: data.number,
            propertyType: data.propertyType,
            builtArea: data.builtArea,
            idealParts: data.idealParts,
        });
        return resultOk();
    } catch(error) {
        if(isAxiosError<ApiError, Record<string, string[]>>(error))
            return resultFail(error.response?.data!);
    }

    throw new Error('Unexpected code flow');
});
