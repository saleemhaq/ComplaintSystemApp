export interface User {
    id: number;
    userName: string;
    created: Date;
    // This is optional that is why we use the 'elvies' or '?' operator
    // Optional properties should come after the required properties or else you will get
    // an error
    roles?: string[];
}