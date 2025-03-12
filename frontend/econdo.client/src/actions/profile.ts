'use server';

import { ValidationError } from "@/app/_data/apiResponses";
import { BriefProfileResponse, CreateProfileData, ProfileDetails } from "@/app/_data/profileData";
import authInstance from "@/lib/axiosInstance";
import { resultFail, resultOk, Result } from "@/utils/result";
import { isAxiosError } from "axios";

export async function createProfile(data: CreateProfileData): Promise<ValidationError | null> {
    try {
        await authInstance.post('/api/profile/create', data);
        return null;
    } catch(error) {
        console.error(error); 
        if(isAxiosError<ValidationError, Record<string, unknown>>(error)) {
            return error.response?.data!;
        }

        console.error(error);
    }

    throw new Error("Unexpected code flow");
}

export async function getBriefProfile(): Promise<BriefProfileResponse> {
    const res = await authInstance.get<BriefProfileResponse>(
        '/api/profile/getBriefProfile');
    return res.data;
}

export async function getProfile(): Promise<Result<ProfileDetails, ValidationError | Error>> {
    try {
        const res = await authInstance.get<ProfileDetails>('/api/profile/getProfile');
        return resultOk(res.data);
    } catch(error) {
        if(isAxiosError<ValidationError, Record<string, unknown>>(error))
            return resultFail(error.response?.data!);

        return resultFail(error as Error);
    }
}

export async function updateProfile(data: ProfileDetails): Promise<Result<null, ValidationError | Error>> {
    try {
        await authInstance.put('/api/profile/updateProfile', data);
        return resultOk(null);
    } catch(error) {
        if(isAxiosError<ValidationError, Record<string, unknown>>(error))
            return resultFail(error.response?.data!);
        
        return resultFail(error as Error);
    }
}
