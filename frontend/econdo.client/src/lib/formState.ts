export type FormState = 'idle' | 'loading' | 'error' | 'success';

export type FormAction =
    | { type: 'SUBMIT' }
    | { type: 'SUCCESS' }
    | { type: 'ERROR' };

export const initialFormState: FormState = 'idle';

export default function formReducer(state: FormState, action: FormAction): FormState {
    switch (action.type) {
        case 'SUBMIT':
            return 'loading';
        case 'SUCCESS':
            return 'success';
        case 'ERROR':
            return 'error';
        default:
            return state;
    }
}
