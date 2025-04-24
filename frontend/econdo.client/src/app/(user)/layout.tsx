'use client';

import { ReactNode } from "react";
import ProfileProvider from "../profileProvider";

import {
  QueryClient,
  QueryClientProvider,
} from '@tanstack/react-query'

const queryClient = new QueryClient();

export default function UserLayout(
    {children} : {children: ReactNode}) {
    
    return (
        <ProfileProvider>
            <QueryClientProvider client={queryClient}>
                {children}
            </QueryClientProvider>
        </ProfileProvider>
    );
}