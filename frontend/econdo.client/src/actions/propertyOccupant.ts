'use server';

import { authInstance } from "@/lib/axiosInstance";
import { ApiError } from "@/types/apiResponses";
import { InvitationStatus } from "@/types/propertyOccupant";
import { Result, resultFail, resultOk } from "@/types/result";
import { isAxiosError } from "axios";
import { cache } from "react";

export const acceptPropertyInvitation =
    cache(async (token: string, email: string):
    Promise<Result> => {
        try {
            await authInstance
            .post('/api/propertyOccupant/acceptInvitation', {
                token,
                email,
            });
            return resultOk();
        } catch(error) {
            if(isAxiosError<ApiError, Record<string, string[]>>(error))
                return resultFail(error.response?.data!);
        }

        throw new Error('Unexpected code flow');
    });

interface AddOccupantToPropertyData {
    buildingId: string,
    entranceNumber: string,
    propertyId: string,
    firstName: string,
    middleName: string,
    lastName: string,
    occupantType: string,
    email: string | null,
}

const baseUrl = process.env.NEXT_PRIVATE_BASE_URL;

export const addOccupantToProperty =
cache(async(data: AddOccupantToPropertyData):
Promise<Result> => {
    try {
        await authInstance.post('/api/propertyOccupant/create', {
            ...data,
            returnUri: `${baseUrl}/acceptInvitation`,
        });
        return resultOk();
    } catch(error) {
        if(isAxiosError<ApiError, Record<string, string[]>>(error))
            return resultFail(error.response?.data!);
    }

    throw new Error('Unexpected code flow');
});

interface UpdateOccupantData {
    occupantId: string,
    firstName: string,
    middleName: string,
    lastName: string,
    type: string,
    email: string | null,
}

export const updatePropertyOccupant =
cache(async (data: UpdateOccupantData): Promise<Result> => {
    try {
        await authInstance.put('/api/propertyOccupant/update', {
            ...data,
            returnUri: `${baseUrl}/acceptInvitation`,
        });
        return resultOk();
    } catch(error) {
        if(isAxiosError<ApiError, Record<string, string[]>>(error))
            return resultFail(error.response?.data!);
    }

    throw new Error('Unexpected code flow');
});

export interface OccupantTypeNameResult {
    occupantTypes: string[],
}
    
export const getAllOccupantTypes =
cache(async (): 
Promise<Result<OccupantTypeNameResult>> => {
    try {
        const res = await authInstance
                .get<OccupantTypeNameResult>(
                    '/api/occupantType/getAll');
        return resultOk(res.data);
    } catch(error) {
        if(isAxiosError<ApiError, Record<string, string[]>>(error))
            return resultFail(error.response?.data!);
    }

    throw new Error('Unexpected code flow');
});

export interface Occupant {
    id: string,
    firstName: string,
    middleName: string,
    lastName: string,
    type: string,
    email?: string,
    invitationStatus: InvitationStatus,
}

export const getOccupantsInProperty =
cache(async (propertyId: string):
Promise<Result<Occupant[]>> => {
    try {
        const res = await authInstance
                .get<Occupant[]>(
                    '/api/propertyOccupant/getInProperty', {
                        params: {
                            propertyId,
                        },
                    });
        return resultOk(res.data);
    } catch(error) {
        if(isAxiosError<ApiError, Record<string, string[]>>(error))
            return resultFail(error.response?.data!);
    }

    throw new Error('Unexpected code flow');
});
