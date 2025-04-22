'use client';

import { BriefPropertyResult, getPropertiesInEntrance } from "@/actions/property";
import Loading from "@/components/loading";
import { ApiError, PagedList } from "@/types/apiResponses";
import { Card, CardSection, Center, Grid, GridCol, Title, Text, Image } from "@mantine/core";
import { IconMoodPuzzled } from "@tabler/icons-react";
import { useParams } from "next/navigation";
import { useCallback, useEffect, useReducer } from "react";
import PropertyCard from "./propertyCard";

interface PropertiesPageState {
    state: 'idle' | 'loading' | 'error' | 'success',
    properties?: PagedList<BriefPropertyResult>,
    error?: ApiError,
}

export type ProperiesPageAction = 
    | { type: 'request_page' }
    | { type: 'request_page_success', properties?: PagedList<BriefPropertyResult> }
    | { type: 'request_page_error', error: ApiError };

function propertyPageReducer(state: PropertiesPageState, action: ProperiesPageAction): PropertiesPageState {
    switch(action.type) {
        case 'request_page':
            return { ...state, state: 'loading' };
        case 'request_page_success':
            return { state: 'success', properties: action.properties };
        case 'request_page_error':
            return { state: 'error', error: action.error };
    }
}

// hard coded for now
const pageSize = 5;


export default function PropertiesList() {
    const {buildingId, entranceNumber} = useParams<{ buildingId: string, entranceNumber: string }>();

    const [state, dispatch] = useReducer(propertyPageReducer, { state: 'idle' });

    const fetchProperties = useCallback(async (page: number) => {
        dispatch({ type: 'request_page' });

        const response = await 
            getPropertiesInEntrance(buildingId, entranceNumber, page, pageSize);
        
        if(!response.ok) {
            dispatch({ type: 'request_page_error', error: response.error });
            return;
        }
        
        dispatch({ type: 'request_page_success', properties: response.value });
    }, [buildingId, entranceNumber, pageSize]);

    useEffect(() => {
        fetchProperties(0);
    }, []);
    

    if(state.state === 'loading')
        return <Loading/>;

    return (
        state.properties ?
        <Grid>
            {
                state
                .properties
                .items.map((value, index) => (
                <GridCol key={index} span={{ base: 2, xs: 3 }}>
                    <PropertyCard key={index} property={value} />
                </GridCol>
                ))    
            }
        </Grid>
        : 
        <>
            <Center mt={90} mb={20}>
                <IconMoodPuzzled size={100} color="#868e96"/>
            </Center>
            <Center>
                <Title c={'dimmed'}>Този вход няма регистрирани имоти</Title>
            </Center>
        </> 
    );
}