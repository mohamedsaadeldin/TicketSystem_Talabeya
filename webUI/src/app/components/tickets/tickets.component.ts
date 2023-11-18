import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { TicketService } from 'src/app/services/ticket.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Ticket } from 'src/app/shared/models/ticket/ticket';
import { OnInit } from '@angular/core';
import { PagingConfig } from 'src/app/shared/models/paging/pagingConfig';

@Component({
  selector: 'app-products',
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.css']
})
export class TicketsComponent implements OnInit,PagingConfig {
  currentPage: number = this.getCurrPage();
  itemsPerPage: number = 5;
  totalItems: number = 0;
  pagingConfig: PagingConfig = {} as PagingConfig;

  constructor(private formbuilder: FormBuilder,
    private router: Router, private ticketService: TicketService,
    private FormsModule: FormsModule){


      this.pagingConfig = {
        itemsPerPage: this.itemsPerPage,
        currentPage: this.getCurrPage(),
        totalItems: this.totalItems
      }
      sessionStorage.setItem('currPage', this.currentPage.toString());
      this.loadTickets();
    }

  Tickets: any[] = [];
  errorMessages: string[] = [];
  public createTicketForm!: FormGroup

  selectedGovernorate: string = '';
  governorates: string[] = [
    'Governorate1',
    'Governorate2',
    'Governorate3',
  ];

  selectedCity: string = '';
  cities: string[] = [
    'City1',
    'City2',
    'City3',
  ];

  selectedDistrict: string = '';
  districts: string[] = [
    'District1',
    'District2',
    'District3',
  ];

  ngOnInit(): void {
    this.createTicketForm = this.formbuilder.group({
      phoneNumber: ['', Validators.required],
      governorate: [this.governorates[0], Validators.required],
      city: [this.cities[0], Validators.required],
      district: [this.districts[0], Validators.required],
      isHandled: [false, Validators.required],
    })
  }

  loadTickets(): void{
    this.ticketService.getTickets(this.getCurrPage(),this.pagingConfig.itemsPerPage)
    .subscribe((ticket: Ticket[])=> {
      this.Tickets.push(ticket);

      this.ticketService.getTicketsCount()
      .subscribe(ticketsCount => {
        this.pagingConfig.totalItems = parseInt(ticketsCount.toString());
      });
    });
  }

  // Handle Pagination
  pageChanged(event: any): void {
    this.pagingConfig.currentPage  = event;
    sessionStorage.setItem('currPage', event.toString());
    this.loadTickets();
    window.location.reload();
  }

  getCurrPage(): number {
    const storedValue = sessionStorage.getItem('currPage');
  
    if (storedValue !== null && storedValue !== undefined) {
      const parsedValue = parseInt(storedValue, 10);
  
      if (!isNaN(parsedValue) && isFinite(parsedValue)) {
        return parsedValue;
      } else {
        console.error('Error parsing stored value as integer:', storedValue);
      }
    } else {
      console.error('Stored value is null or undefined');
    }
  
    return 1; // Default
  }

  // Date Convertion
  convertDate(dateString: string) {
    const dateObject = new Date(dateString);
    
    // Check if the dateObject is valid before formatting
    if (!isNaN(dateObject.getTime())) {
      const formattedDate = dateObject.toLocaleString('en-US', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit',
        second: '2-digit'
      });
  
      return formattedDate.replace(',', '');
    } else {
      return 'Invalid Date';
    }
  }

  // update colors 
  getColor(ticket: any): string {
    const now = new Date();
    const creationTime = new Date(ticket.creationDateTime);
    const timeDifference = Math.abs(now.getTime() - creationTime.getTime()) / (1000 * 60);

    if (timeDifference <= 15) {
      return 'yellow';
    } else if (timeDifference <= 30) {
      return 'green';
    } else if (timeDifference <= 45) {
      return 'blue';
    } else if (timeDifference <= 60) {
      return 'red';
    }
    // default white
    return 'white';
  }

  // Api Functions

  createTicket(){
    this.errorMessages = [];
    if (this.createTicketForm.valid) {
      this.ticketService.createTicket(this.createTicketForm.value).subscribe({
        next: _ => {
          window.location.reload();
        },
        error: error => {
          if (error.error.errors) {
            this.errorMessages = error.error.errors;
          } else {
            this.errorMessages.push(error.error);
          }
        }
      })
    }
  }

  markHandled(id: number){
    this.errorMessages = [];
    this.ticketService.isHandled(id).subscribe({
      next: _ => {
        window.location.reload();
      },
      error: error => {
        if (error.error.errors) {
          this.errorMessages = error.error.errors;
        } else {
          this.errorMessages.push(error.error);
        }
      }
    })
  }
}
