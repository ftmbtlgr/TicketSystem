export interface User {
    userId: number;
    username: string;
    email: string;
    firstName: string;
    lastName: string;
    role: 'Admin' | 'User' | 'Guest';
    createdDate: Date;
    lastLoginDate?: Date;
    isActive: boolean;

    phoneNumber?: string;
    profilePictureUrl?: string;
    
    // Relationships
    // ticketsCreated?: Ticket[];
    // ticketsAssigned?: Ticket[];
    }