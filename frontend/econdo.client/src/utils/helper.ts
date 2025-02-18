import { ApiError } from "@/app/_data/apiResponses";
import { AxiosError } from "axios";

export const axiosToApiErrorConverter = (ex: AxiosError) : ApiError => {
    let apiError = ex.response?.data as ApiError;
    apiError.errors = new Map(Object.entries(apiError.errors));
    return apiError;
}