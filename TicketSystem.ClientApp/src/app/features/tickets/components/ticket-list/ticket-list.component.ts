import { Component, OnDestroy, OnInit, Input } from '@angular/core';
import { Ticket } from '../../../../shared/models/ticket';
import { TicketService } from '../../ticket.service';
import { catchError, finalize, Observable, of, map } from 'rxjs';
import { Pagination } from '../../../../shared/models/pagination';
import { PageEvent } from '@angular/material/paginator';
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
    /*this.ticketService.getTickets().subscribe({
      next: (res: Pagination<Ticket>) => {
        this.tickets = res.data;
      },
    }); */
    this.loadTickets();
  }
  loadTickets(): void {
    this.isLoading = true; // Yüklemeyi başlat
    this.errorMessage = null; // Önceki hataları temizle

    this.tickets$ = this.ticketService.getTickets().pipe(
      map(response => response.data),
      catchError((error) => {
        // Hata yakalandığında
        this.errorMessage =
          error.message || 'Biletler yüklenirken bir hata oluştu.';
        this.isLoading = false; // Yüklemeyi durdur
        return of([]); // Hata durumunda boş bir Observable döndürerek akışı sonlandır
      }),
      finalize(() => {
        // İşlem tamamlandığında (başarılı veya hatalı)
        this.isLoading = false;
      })
    );
  }
}
