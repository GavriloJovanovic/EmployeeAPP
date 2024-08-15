import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Employee } from '../models/employee.model';

@Injectable({
  providedIn: 'root'
})
export class TimeTrackerService {

  private apiUrl = 'https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code=vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==';

  constructor(private http: HttpClient) { }

  getTimeEntries(): Observable<Array<Employee>> {
    return this.http.get<Array<Employee>>(this.apiUrl).pipe(
      map(data => data.map(entry => new Employee(
        entry.Id,
        entry.EmployeeName,
        new Date(entry.StarTimeUtc),
        new Date(entry.EndTimeUtc),
        entry.EntryNotes
      )))
    );
  }
}