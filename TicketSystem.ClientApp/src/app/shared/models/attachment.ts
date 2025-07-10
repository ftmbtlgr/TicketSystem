export interface Attachment {
  attachmentId: number;
  fileName: string;
  filePath: string;
  uploadedDate: Date;

  ticketId: number;
  uploadedByUserId: number;
}
