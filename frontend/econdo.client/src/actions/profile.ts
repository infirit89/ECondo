'use server';

import { BriefProfileResponse, CreateProfileData, ProfileDetails } from "@/types/profileData";
import { authInstance } from "@/lib/axiosInstance";
import { resultFail, resultOk, Result } from "@/types/result";
import { isAxiosError } from "axios";
import { ApiError, PagedList } from "@/types/apiResponses";
import { cache } from "react";
import { userProfileCookieKey } from "@/utils/constants";
import { cookies } from "next/headers";

export async function createProfile(data: CreateProfileData): Promise<Result> {
    try {
        console.log('aaaaaaaa');
        await authInstance.post('/api/profile/create', data);
        return resultOk();
    } catch(error) {
        // console.log();
        if(isAxiosError(error))
            console.log(error.response?.data);
        if(isAxiosError<ApiError>(error))
            return resultFail(error.response?.data!);
    }

    throw new Error('Unexpected code flow');
}

export const getBriefProfile = cache(async (): Promise<Result<BriefProfileResponse>> => {
    try {
        const res = 
        await authInstance.get<BriefProfileResponse>(
            '/api/profile/getBriefProfile');
        return resultOk(res.data);
    } catch(error) {
        if(isAxiosError<ApiError>(error))
            return resultFail(error.response?.data!);
    }

    throw new Error('Unexpected code flow');
});

export async function setProfileCookie(profile: BriefProfileResponse | undefined) {
    const cookieStore = await cookies();
    
    cookieStore.set(userProfileCookieKey, 
        JSON.stringify(profile));
}

export async function getProfileFromCookie(): Promise<BriefProfileResponse | undefined> {
    const cookieStore = await cookies();
    const profile = cookieStore.get(userProfileCookieKey);

    return profile?.value ? JSON.parse(profile.value) : undefined;
}

export const getProfile = cache(async (): Promise<Result<ProfileDetails>> => {
    try {
        const res = await authInstance.get<ProfileDetails>('/api/profile/getProfile');
        return resultOk(res.data);
    } catch(error) {
        if(isAxiosError<ApiError>(error))
            return resultFail(error.response?.data!);
    }

    throw new Error('Unexpected code flow');
});

export interface UserProfileResult {
    firstName: string,
    middleName: string,
    lastName: string,
    email: string,
}

export const getAllProfiles = 
cache(async (page: number, pageSize: number): 
Promise<Result<PagedList<UserProfileResult>>> => {
    try {
        const res = await authInstance.get<PagedList<UserProfileResult>>(
            '/api/profile/getAll', {
                params: {
                    page,
                    pageSize,
                },
            });
            
        return resultOk(res.data);
    } catch(error) {
        if(isAxiosError<ApiError>(error))
            return resultFail(error.response?.data!);
    }

    throw new Error('Unexpected code flow');
});

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
