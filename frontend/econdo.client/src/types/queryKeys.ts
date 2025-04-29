export const queryKeys = {
    properties: {
        all: ['properties'] as const,
        details: (id: string) => [...queryKeys.properties.all, id] as const,
        pagedInEntrance: (
            buildingId: string, 
            entranceNumber: string, 
            page: number, 
            pageSize: number) => [
                ...queryKeys.properties.all,
                buildingId,
                entranceNumber,
                page,
                pageSize] as const,
        pagedForUser: (
            page: number,
            pageSize: number
        ) => [
            ...queryKeys.properties.all,
            page,
            pageSize,
        ] as const,
        allPaged: (
            page: number,
            pageSize: number,
            buildingId?: string,
            entranceNumber?: string,
        ) => [
            ...queryKeys.properties.all,
            page,
            pageSize,
            buildingId,
            entranceNumber,
        ] as const,
    },
    occupants: {
        all: ['ocupants'] as const,
        inProperty: (id: string) => [...queryKeys.occupants.all, id] as const,
        tenantsInPropertyPaged: (
            id: string,
            page: number,
            pageSize: number,
        ) => [
            ...queryKeys.occupants.all,
            id,
            page,
            pageSize
        ] as const,
    },
    profiles: {
        all: ['profiles'] as const,
        getBrief: () => [...queryKeys.profiles.all, 'brief'] as const,
        allPaged: (
            page: number, 
            pageSize: number
        ) => [
            ...queryKeys.profiles.all,
            page,
            pageSize
        ] as const,
    },
    buildings: {
        all: ['buildings'] as const,
        pagedForUser: (
            page: number,
            pageSize: number,
            buildingName?: string,
        ) => [
            ...queryKeys.buildings.all,
            page,
            pageSize,
            buildingName,
        ] as const,
        allPaged: (
            page: number,
            pageSize: number,
        ) => [
            ...queryKeys.buildings.all,
            page,
            pageSize,
        ] as const,
    },
    stripe: {
        checkStatus: (
            buildingId: string,
            entranceNumber: string,
        ) => [
            buildingId,
            entranceNumber,
        ] as const,
    },
    payments: {
        all: ['payments'] as const,
        pagedForProperty: (
            propertyId: string,
            page: number,
            pageSize: number,
        ) => [
            ...queryKeys.payments.all,
            propertyId,
            page,
            pageSize,
        ] as const,
        createIntent: (
            propertyId: string,
        ) => [
            ...queryKeys.payments.all,
            propertyId,
        ] as const,
    },
    bills: {
        all: ['bills'] as const,
        pagedForEntrance: (
            buildingId: string,
            entranceNumber: string,
            page: number,
            pageSize: number,
        ) => [
            ...queryKeys.bills.all,
            buildingId,
            entranceNumber,
            page,
            pageSize,
        ]
    }
}
