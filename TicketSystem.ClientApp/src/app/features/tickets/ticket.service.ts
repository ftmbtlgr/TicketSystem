import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Pagination } from '../../shared/models/pagination';
import { Ticket } from '../../shared/models/ticket';

import { BehaviorSubject, Observable, of, tap } from 'rxjs';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class TicketService {
  baseUrl = environment.apiUrl; 
  private http = inject(HttpClient);

  getTickets(){
    return this.http.get<Pagination<Ticket>>(this.baseUrl + 'tickets')}

  createTicket(ticket: any) {
  return this.http.post(`${this.baseUrl}tickets`, ticket);
}

}
