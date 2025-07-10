import { Component, OnDestroy, OnInit } from '@angular/core';
import { Ticket } from '../../../../shared/models/ticket';
import { TicketService } from '../../ticket.service';
import { Observable, Subscription } from 'rxjs';
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
  tickets: Ticket[] = [];

  constructor(private ticketService: TicketService) {}

  ngOnInit(): void {
    this.ticketService.getTickets().subscribe({
      next: (res: Pagination<Ticket>) => {
        this.tickets = res.data;
      },
    });
  }
}
