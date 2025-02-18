'use server';

import authInstance from "@/lib/axiosInstance";

export default async function Home() {

  const res = await authInstance.get('/api/weatherForecast/');
  
  return (
    <>
    { res && res.status === 200 ? <p>Toma</p> : <></> }
    </>
  );
}
