export function formatCurrency(amount: number): string {
    return new Intl.NumberFormat('bg-BG', {
        style: 'currency',
        currency: 'BGN',
    }).format(amount);
}
