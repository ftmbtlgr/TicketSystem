// ticket-list.component.ts
import { Component, OnInit } from '@angular/core';
import { Ticket } from '../../../../shared/models/ticket';
import { TicketService } from '../../ticket.service';
import { catchError, finalize, Observable, of, map } from 'rxjs';
// import { Pagination } from '../../../../shared/models/pagination'; // Artık buna ihtiyacınız olmayabilir
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-ticket-list',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatTableModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatButtonModule,
    MatIconModule,
  ],
  templateUrl: './ticket-list.component.html',
  styleUrl: './ticket-list.component.scss',
})
export class TicketListComponent implements OnInit {
  tickets$: Observable<Ticket[]> | null = null;
  isLoading: boolean = true;
  errorMessage: string | null = null;

  constructor(private ticketService: TicketService) {}

  ngOnInit(): void {
    this.loadTickets();
  }

  loadTickets(): void {
    this.isLoading = true;
    this.errorMessage = null;

    this.tickets$ = this.ticketService.getAllTickets().pipe(
      map((pagination: any) => pagination && pagination.items ? pagination.items : []),
      catchError((error) => {
        console.error('Biletler yüklenirken bir hata oluştu:', error);
        this.errorMessage = error.message || 'Biletler yüklenirken bir hata oluştu.';
        this.isLoading = false;
        return of([]);
      }),
      finalize(() => {
        this.isLoading = false;
        console.log('Yükleme işlemi tamamlandı. isLoading:', this.isLoading);
      })
    );
  }
}