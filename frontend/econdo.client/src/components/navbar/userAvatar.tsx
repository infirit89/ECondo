'use client';

import { useProfile } from "@/app/profileProvider";
import { Avatar, Skeleton } from "@mantine/core";

import classes from './userAvatar.module.css';
import React, { forwardRef, useEffect, useState } from "react";

const UserAvatar = forwardRef<HTMLDivElement, React.ComponentPropsWithoutRef<'div'>>((props, ref) => {
    const { profile, loading } = useProfile();
    const [ mounted, setMounted ] = useState(false);

    useEffect(() => {
        setMounted(true);
    })

    if(!mounted || loading || !profile)
        return (<Skeleton ref={ref} {...props} height={40} circle />);

    return (
        <Avatar
        ref={ref}
        {...props}
        radius="xl" 
        color={'blue'} 
        className={classes.profileLink}>
            {profile?.firstName.at(0)}{profile?.lastName.at(0)}
        </Avatar>
    );
}); 

export default UserAvatar;
