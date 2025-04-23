'use client';

import { BriefPropertyResult, deleteProperty, getPropertiesInEntrance } from "@/actions/property";
import Loading from "@/components/loading";
import { ApiError, PagedList } from "@/types/apiResponses";
import { Center, Grid, GridCol, Title, Pagination, Alert } from "@mantine/core";
import { IconExclamationCircle, IconMoodPuzzled } from "@tabler/icons-react";
import { useParams } from "next/navigation";
import { useCallback, useEffect, useReducer, useState } from "react";
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

function propertyPageReducer(state: PropertiesPageState, action: ProperiesPageAction): 
    PropertiesPageState {
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
const pageSize = 8;

export default function PropertiesList() {
    const { buildingId, entranceNumber } = useParams<{
        buildingId: string,
        entranceNumber: string }>();

    const [isMounted, setIsMounted] = useState(false);
    const [state, dispatch] = useReducer(propertyPageReducer, { state: 'idle' });
    const [isDeleteError, setDeleteError] = useState(false);

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
        setIsMounted(true);
    }, []);
    
    const handlePropertyDelete = async (id: string) => {
        const res = await deleteProperty({
            buildingId: buildingId,
            entranceNumber: entranceNumber,
            propertyId: id,
        });

        fetchProperties(state.properties?.currentPage!);
        
        if(!res.ok) {
            setDeleteError(true);
            return;
        }

        setDeleteError(false);
    };

    if(!isMounted || state.state === 'loading')
        return <Loading/>;

    return (
        <> 
            { 
                isDeleteError ?
                <Alert
                variant="light"
                color="red"
                title="Грешка"
                icon={<IconExclamationCircle/>} mb={'md'}>
                    Грешка при изтриването на имот!
                </Alert>
                :
                <></>
            }
            {
                state.properties && state.properties.items.length > 0 ?
                <>
                    <Grid>
                        {
                            state
                            .properties
                            .items.map((value, index) => (
                            <GridCol key={index} span={{ base: 2, xs: 3 }}>
                                <PropertyCard
                                key={index}
                                property={value}
                                handleDelete={handlePropertyDelete} />
                            </GridCol>
                            ))    
                        }
                    </Grid>
                    <Pagination total={state.properties.totalPages}
                    value={state.properties.currentPage + 1}
                    onChange={(value) => fetchProperties(value - 1)}
                    mt={'md'}/>
                </>
                : 
                <>
                    <Center mt={90} mb={20}>
                        <IconMoodPuzzled size={100} color="#868e96"/>
                    </Center>
                    <Center>
                        <Title c={'dimmed'}>Този вход няма регистрирани имоти</Title>
                    </Center>
                </> 
            }
        </>
    );
}