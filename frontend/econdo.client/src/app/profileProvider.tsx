'use client';

import { getBriefProfile } from "@/actions/profile";
import Loading from "@/components/loading";
import { ProfileCreationModal } from "@/components/profile/profileCreationModal";
import { ApiError } from "@/types/apiResponses";
import { BriefProfileResponse } from "@/types/profileData";
import { userProfileSessionKey } from "@/utils/constants";
import { createContext, ReactNode, useContext, useEffect, useState } from "react";

type ProfileContextType = {
    profile?: BriefProfileResponse;
    loading: boolean;
    error?: ApiError;
    refetchProfile: () => Promise<void>;
}

const ProfileContext = 
    createContext<ProfileContextType | undefined>(undefined);

const getStoredProfile = (): BriefProfileResponse | undefined => {
    if (typeof window === 'undefined') return undefined;
    
    try {
      const storedProfile = sessionStorage.getItem(userProfileSessionKey);
      return storedProfile ? JSON.parse(storedProfile) : undefined;
    } catch (e) {
      return undefined;
    }
};

export default function ProfileProvider(
    { children } : { children: ReactNode }) {
    const storedProfile = getStoredProfile();
    const [profile, setProfile] = 
    useState<BriefProfileResponse | undefined>(storedProfile);
    const [loading, setLoading] = useState(!storedProfile);
    const [error, setError] = 
        useState<ApiError | undefined>(undefined);

    const fetchProfile = async () => {
        setLoading(true);
        const response = await getBriefProfile();
        
        if (!response.ok) {
            setError(response.error);
            setLoading(false);
            return;
        }
        
        sessionStorage
            .setItem(userProfileSessionKey,
                JSON.stringify(response.value));
        setProfile(response.value);
        setError(undefined);
        setLoading(false);
    };

    // fetch profile data on component mount
    useEffect(() => {
        if(!profile)
            fetchProfile();
    }, []);

    return (
        error ?
        <ProfileCreationModal opened={true}/>
        :
        <ProfileContext.Provider
        value={{
            profile,
            loading,
            error,
            refetchProfile: fetchProfile,
        }}
        >
        {children}
        </ProfileContext.Provider>
    );
}

export function useProfile() {
    const context = useContext(ProfileContext);
    if (context === undefined) {
        throw new Error('useProfile must be used within a ProfileProvider');
    }
    return context;
}