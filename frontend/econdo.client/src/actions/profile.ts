'use server';

import { ValidationError } from "@/app/_data/apiResponses";
import { BriefProfileResponse, CreateProfileData } from "@/app/_data/profileData";
import authInstance from "@/lib/axiosInstance";
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