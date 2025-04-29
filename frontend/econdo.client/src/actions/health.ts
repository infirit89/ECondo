import normalInstance from "@/lib/axiosInstance";

export async function checkHealth() {
    await normalInstance.get('/health');
}