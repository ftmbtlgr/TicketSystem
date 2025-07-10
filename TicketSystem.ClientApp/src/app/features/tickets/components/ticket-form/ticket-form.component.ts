import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { TicketService } from '../../ticket.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-ticket-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './ticket-form.component.html'
})
export class TicketFormComponent {
  ticketForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private ticketService: TicketService,
    private router: Router
  ) {
    this.ticketForm = this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.ticketForm.valid) {
      this.ticketService.createTicket(this.ticketForm.value).subscribe({
        next: () => {
          alert('Ticket başarıyla eklendi.');
          this.router.navigate(['/tickets']);
        },
        error: (err) => {
          console.error('Hata:', err);
          alert('Ticket eklenirken bir hata oluştu.');
        }
      });
    }
  }
}
