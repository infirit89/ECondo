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
        ] as const
    },
    occupants: {
        all: ['ocupants'] as const,
        inProperty: (id: string) => [...queryKeys.occupants.all, id] as const,
    },
    profiles: {
        all: ['profiles'] as const,
        getBrief: () => [...queryKeys.profiles.all, 'brief'] as const,
    }
}
