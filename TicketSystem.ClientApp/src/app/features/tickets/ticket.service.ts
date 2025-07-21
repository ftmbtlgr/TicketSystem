import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Pagination } from '../../shared/models/pagination';
import { Ticket } from '../../shared/models/ticket';
import { Observable, forkJoin, throwError } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';

import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class TicketService {
  baseUrl = environment.apiUrl + 'tickets/';
  private http = inject(HttpClient);

  getTickets() {
    return this.http.get<Pagination<Ticket>>(this.baseUrl + 'tickets');
  }

  createTicket(ticket: any) {
    return this.http.post(`${this.baseUrl}tickets`, ticket);
  }
  getAllTickets(): Observable<Ticket[]> {
    // API endpoint: GET /api/tickets
    return this.http.get<Ticket[]>(this.baseUrl);
  }

  getTicketsByUserId(userId: number): Observable<Ticket[]> {
    // API endpoint: GET /api/tickets/user/{userId}
    // veya GET /api/users/{userId}/tickets

    return this.http.get<Ticket[]>(`${this.baseUrl}/tickets/user/${userId}`);
  }
  // --- Hata Yönetimi ---

  private handleError(error: HttpErrorResponse) {
    console.error('API Hatası:', error);
    let errorMessage = 'Bilinmeyen bir hata oluştu.';

    if (error.error instanceof ErrorEvent) {
      // İstemci tarafı veya ağ hatası
      errorMessage = `İstemci Hatası: ${error.error.message}`;
    } else if (error.status) {
      // Sunucu tarafı hatası (HTTP Durum Kodu olan hatalar)
      errorMessage = `Sunucu Hatası Kodu: ${error.status} - ${error.statusText || ''}`;
      if (error.error && typeof error.error === 'object' && error.error.errors) {
        // Validation hataları gibi daha detaylı hata mesajları
        const validationErrors = Object.values(error.error.errors).flat();
        errorMessage += `\nDetaylar: ${validationErrors.join(', ')}`;
      } else if (error.error && typeof error.error === 'string') {
        errorMessage += `\nMesaj: ${error.error}`;
      } else if (error.message) {
        errorMessage += `\nMesaj: ${error.message}`;
      }
    } else {
      // API'ye ulaşılamaması veya diğer düşük seviyeli ağ hataları
      errorMessage = `Ağ Bağlantı Hatası: ${error.message}`;
    }

    return throwError(() => new Error(errorMessage));
  }
}
