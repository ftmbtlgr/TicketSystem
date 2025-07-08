export type Ticket = {
  ticketId: number;
  title: string;
  description: string;
  status: 'Open' | 'In Progress' | 'Closed'| 'Reopened';
  createdDate: Date;
  lastUpdatedDate: Date;
  dueDate: Date;

  createdByUserId: number;
  assignedToUserId?: number;

   /*createdBy?: User;
   assignedTo?: User;
   comments?: Comment[];
   attachments?: Attachment[];*/
};
