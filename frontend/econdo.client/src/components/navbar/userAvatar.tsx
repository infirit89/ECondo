'use client';

import { useProfile } from "@/providers/profileProvider";
import { Avatar, Skeleton } from "@mantine/core";

import classes from './userAvatar.module.css';
import React, { forwardRef, useEffect, useState } from "react";

const UserAvatar = forwardRef<HTMLDivElement, React.ComponentPropsWithoutRef<'div'>>((props, ref) => {
    const { profile } = useProfile();
    const [ mounted, setMounted ] = useState(false);

    useEffect(() => {
        setMounted(true);
    })

    if(!mounted || !profile)
        return (<Skeleton ref={ref} {...props} height={40} circle />);

    return (
        <Avatar
        ref={ref}
        {...props}
        radius="xl" 
        name={`${profile.firstName} ${profile.lastName}`}
        color={'initials'} 
        className={classes.profileLink}/>
    );
}); 

export default UserAvatar;
