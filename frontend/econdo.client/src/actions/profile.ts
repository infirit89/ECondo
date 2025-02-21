'use server';

import { ApiError, ApiSucess, isApiError } from "@/app/_data/apiResponses";
import { CreateProfileData } from "@/app/_data/profileData";
import { normalInstance } from "@/lib/axiosInstance";
import { axiosToApiErrorConverter } from "@/utils/helper";
export async function createProfile(data: CreateProfileData): Promise<ApiError | ApiSucess> {
    const res = await normalInstance.post('/api/profile/create', data)
    .catch(axiosToApiErrorConverter);

    if(isApiError(res))
        return res;

    return {
        status: res.status,
        statusText: res.statusText,
    };
}