export enum InvitationStatus {
    NotInvited = 0,
    Pending,
    Accepted,
    Expired,
    Declined
}

export type OccupantType = 'tennant' | 'owner' | 'user' | 'representative';
