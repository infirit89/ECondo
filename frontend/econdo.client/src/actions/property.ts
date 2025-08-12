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

export interface BriefOccupantResult {
    firstName: string,
    lastName: string,
}

export interface PropertyOccupantResult {
    property: PropertyResult,
    occupants: BriefOccupantResult[],
    remainingOccupants: number,
}

export const getPropertiesInEntrance =
    cache(async (entranceId: string, page: number, pageSize: number):
        Promise<Result<PagedList<PropertyOccupantResult>>> => {
        try {
            const properties = await authInstance
                .get<PagedList<PropertyOccupantResult>>('/api/property/getPropertiesInEntrance', {
                    params: {
                        entranceId,
                        page,
                        pageSize
                    },
                });

            return resultOk(properties.data);
        }
        catch (error) {
            if (isAxiosError<ApiError, Record<string, string[]>>(error))
                return resultFail(error.response?.data!);
        }

        throw new Error('Unexpected code flow');
    });

export const getPropertiesForUser =
    cache(async (page: number, pageSize: number): Promise<Result<PagedList<PropertyOccupantResult>>> => {
        try {
            const properties = await authInstance
                .get<PagedList<PropertyOccupantResult>>('/api/property/getForUser', {
                    params: {
                        page,
                        pageSize
                    },
                });

            return resultOk(properties.data);
        }
        catch (error) {
            if (isAxiosError<ApiError, Record<string, string[]>>(error))
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
        catch (error) {
            if (isAxiosError<ApiError, Record<string, string[]>>(error))
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
        } catch (error) {
            if (isAxiosError<ApiError, Record<string, string[]>>(error))
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
        } catch (error) {
            if (isAxiosError<ApiError, Record<string, string[]>>(error))
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
        } catch (error) {
            if (isAxiosError<ApiError, Record<string, string[]>>(error))
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
        } catch (error) {
            if (isAxiosError<ApiError, Record<string, string[]>>(error))
                return resultFail(error.response?.data!);
        }

        throw new Error('Unexpected code flow');
    });

export const getAllProperties =
    cache(async (
        page: number,
        pageSize: number,
        entranceFilter: { buildingId: string, entranceNumber: string } | undefined):
        Promise<Result<PagedList<PropertyOccupantResult>>> => {
        try {
            const res = await authInstance.get<PagedList<PropertyOccupantResult>>(
                '/api/property/getAll', {
                params: {
                    page,
                    pageSize,
                    'entranceFilter.buildingId': entranceFilter?.buildingId,
                    'entranceFilter.entranceNumber': entranceFilter?.entranceNumber,
                }
            });

            return resultOk(res.data);
        } catch (error) {
            if (isAxiosError<ApiError, Record<string, string[]>>(error))
                return resultFail(error.response?.data!);
        }

        throw new Error('Unexpected code flow');
    })

// Guid Id, decimal AmountPaid, string BillTitle, string Status
export interface PaymentResult {
    id: string,
    amountPaid: number,
    billTitle: string,
    status: string,
}

export const getPaymentsForProperty =
    cache(async (propertyId: string, page: number, pageSize: number) => {
        try {
            const res = await authInstance.get<PagedList<PaymentResult>>(
                '/api/payment/getForProperty', {
                params: {
                    propertyId,
                    page,
                    pageSize,
                }
            });

            return resultOk(res.data);
        } catch (error) {
            console.error(error);
            if (isAxiosError<ApiError, Record<string, string[]>>(error))
                return resultFail(error.response?.data!);
        }

        throw new Error('Unexpected code flow');
    });

interface StripeSecretResult {
    clientSecret: string,
}

export const createIntent =
    cache(async (paymentId: string):
        Promise<Result<StripeSecretResult>> => {
        try {
            const res = await authInstance.post<StripeSecretResult>(
                '/api/payment/createIntent', {
                paymentId
            });

            return resultOk(res.data);
        } catch (error) {
            if (isAxiosError(error))
                console.error(error.response);
            if (isAxiosError<ApiError, Record<string, string[]>>(error))
                return resultFail(error.response?.data!);
        }

        throw new Error('Unexpected code flow');
    });
