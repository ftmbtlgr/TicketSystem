import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from "./layout/header/header.component";
import { HttpClient } from '@angular/common/http';
import { Ticket } from './shared/models/ticket';
import { Pagination } from './shared/models/pagination';
import { environment } from '../../environments/environment';
import { TicketListComponent } from "./features/tickets/components/ticket-list/ticket-list.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent, TicketListComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  title = 'ticket-system';
  tickets: Ticket[] = [];

  ngOnInit(): void {
    this.http.get<Pagination<Ticket>>(this.baseUrl + 'tickets').subscribe({
      next: response => this.tickets = response.data,
      error: error => console.log(error),
      complete: () => console.log('complete')
  })
  }
}