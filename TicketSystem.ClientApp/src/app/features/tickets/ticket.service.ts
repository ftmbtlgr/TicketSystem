import {
  HttpClient,
  HttpErrorResponse,
  HttpParams,
} from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Pagination } from '../../shared/models/pagination';
import { Ticket } from '../../shared/models/ticket';
import { Observable, catchError, forkJoin, throwError } from 'rxjs';

import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class TicketService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  createTicket(ticket: any) {
    return this.http.post(`${this.baseUrl}tickets`, ticket);
  }
  // GET /api/tickets
  getAllTickets(): Observable<Ticket[]> {
    return this.http.get<Ticket[]>(this.baseUrl + 'tickets').pipe(
      catchError(this.handleError) 
    );
  }

  getTicketsByUserId(userId: number): Observable<Ticket[]> {
    // GET /api/tickets/user/{userId}

    return this.http.get<Ticket[]>(`${this.baseUrl}/tickets/user/${userId}`);
  }
  private handleError(error: HttpErrorResponse) {
    console.error('API Hatası:', error);
    let errorMessage = 'Bilinmeyen bir hata oluştu.';

    if (error.error instanceof ErrorEvent) {
      errorMessage = `İstemci Hatası: ${error.error.message}`;
    } else if (error.status) {
      errorMessage = `Sunucu Hatası Kodu: ${error.status} - ${error.statusText || ''}`;
      if (error.error && typeof error.error === 'object' && error.error.errors) {
        const validationErrors = Object.values(error.error.errors).flat();
        errorMessage += `\nDetaylar: ${validationErrors.join(', ')}`;
      } else if (error.error && typeof error.error === 'string') {
        errorMessage += `\nMesaj: ${error.error}`;
      } else if (error.message) {
        errorMessage += `\nMesaj: ${error.message}`;
      }
    } else {
      errorMessage = `Ağ Bağlantı Hatası: ${error.message}`;
    }

    // throwError bir fonksiyon olduğu için arrow function ile çağrılmalı
    return throwError(() => new Error(errorMessage)); 
  }
}
