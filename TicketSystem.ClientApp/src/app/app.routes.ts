import { Routes } from '@angular/router';
import { TicketListComponent } from './features/tickets/components/ticket-list/ticket-list.component';
import { TicketFormComponent } from './features/tickets/components/ticket-form/ticket-form.component';

export const routes: Routes = [
  { path: '', redirectTo: 'tickets', pathMatch: 'full' },
  { path: 'tickets', component: TicketListComponent },
  { path: 'tickets/new', component: TicketFormComponent },
];
