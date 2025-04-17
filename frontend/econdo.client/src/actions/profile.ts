'use server';

import { BriefProfileResponse, CreateProfileData, ProfileDetails } from "@/types/profileData";
import authInstance from "@/lib/axiosInstance";
import { resultFail, resultOk, Result } from "@/types/result";
import { isAxiosError } from "axios";
import { ApiError } from "@/types/apiResponses";

export async function createProfile(data: CreateProfileData): Promise<Result> {
    try {
        await authInstance.post('/api/profile/create', data);
        return resultOk();
    } catch(error) {
        if(isAxiosError<ApiError>(error))
            return resultFail(error.response?.data!);
    }

    throw new Error('Unexpected code flow');
}

export async function getBriefProfile(): Promise<Result<BriefProfileResponse>> {
    try {
        const res = await authInstance.get<BriefProfileResponse>(
            '/api/profile/getBriefProfile');
        return resultOk(res.data);
    } catch(error) {
        if(isAxiosError<ApiError>(error))
            return resultFail(error.response?.data!);
    }

    console.log('aaaaaaaaaaaaaa');
    throw new Error('Unexpected code flow');
}

export async function getProfile(): Promise<Result<ProfileDetails>> {
    try {
        const res = await authInstance.get<ProfileDetails>('/api/profile/getProfile');
        return resultOk(res.data);
    } catch(error) {
        if(isAxiosError<ApiError>(error))
            return resultFail(error.response?.data!);
    }

    throw new Error('Unexpected code flow');
}

export async function updateProfile(data: ProfileDetails): Promise<Result> {
    try {
        await authInstance.put('/api/profile/updateProfile', data);
        return resultOk();
    } catch(error) {
        if(isAxiosError<ApiError>(error))
            return resultFail(error.response?.data!);
    }
    
    throw new Error('Unexpected code flow');
}
