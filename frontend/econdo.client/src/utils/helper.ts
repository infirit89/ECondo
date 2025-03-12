import { ValidationError } from "@/app/_data/apiResponses";
import { AxiosError, isAxiosError } from "axios";

export const axiosToApiErrorConverter = (ex: AxiosError) : ValidationError | AxiosError => {
    if(isAxiosError<ValidationError>(ex))
        return ex;
    
    return ex;
}