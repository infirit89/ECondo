'use client';

import { getBriefProfile } from "@/actions/profile";
import Loading from "@/components/loading";
import { ProfileCreationModal } from "@/components/profile/profileCreationModal";
import { BriefProfileResponse } from "@/types/profileData";
import { queryKeys } from "@/types/queryKeys";
import { useQuery } from "@tanstack/react-query";
import { createContext, ReactNode, useContext } from "react";

type ProfileContextType = {
    profile?: BriefProfileResponse;
}

const ProfileContext = 
    createContext<ProfileContextType | undefined>(undefined);

const useQueryBriefProfile = () => {
    return useQuery({
       queryKey: queryKeys.profiles.getBrief(),
       queryFn: () => getBriefProfile(), 
    });
}

export default function ProfileProvider(
    { children } : { children: ReactNode }) {
    const { data: profileRes, isLoading } = useQueryBriefProfile();

    if(isLoading)
        return <Loading height={'100vh'}/>;

    return (
        !profileRes?.ok ?
        <ProfileCreationModal opened={true}/>
        :
        <ProfileContext.Provider
        value={{
            profile: profileRes.value,
        }}>
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