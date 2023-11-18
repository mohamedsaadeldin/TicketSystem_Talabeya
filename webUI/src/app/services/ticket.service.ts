import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { Ticket } from '../shared/models/ticket/ticket';

@Injectable({
    providedIn: 'root'
})
export class TicketService {
    constructor(private http: HttpClient) { }

    getTickets(page: number, pageSize: number) {
        return this.http.get<Ticket[]>(`${environment.appUrl}Tickets?page=${page}&pageSize=${pageSize}`)
    }

    getTicketsCount() {
        return this.http.get(`${environment.appUrl}Tickets/GetTicketsCount`)
    }

    getTicketById(id: number) {
        return this.http.get<Ticket[]>(`${environment.appUrl}Tickets/GetTicketById/${id}`, {})
    }

    createTicket(model: Ticket) {
        return this.http.post(`${environment.appUrl}Tickets/CreateTicket`, model);
    }

    isHandled(id: number) {
        return this.http.put(`${environment.appUrl}Tickets/IsHandled?id=${id}`, {});
    }
}