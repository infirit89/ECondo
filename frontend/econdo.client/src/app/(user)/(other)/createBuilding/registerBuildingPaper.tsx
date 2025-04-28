'use client';

import { ProvinceNameResult } from "@/actions/condo";
import RegisterBuildingEntranceForm from "@/components/registerBuildingEntranceForm/registerBuildingEntranceForm";
import { Paper } from "@mantine/core";
import { redirect } from "next/navigation";

export default function RegisterBuildingPaper({ provinces } : { provinces: ProvinceNameResult }) {
    const onSuccess = () => {
        redirect('/condos/buildings');
    }

    
    return (
    <Paper withBorder shadow="md" p={30} mt={10} radius="md">
        <RegisterBuildingEntranceForm provinces={provinces} onSuccess={onSuccess}/>
    </Paper>
    );
}