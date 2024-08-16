import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { TimeTrackerService } from '../services/time-tracker.service';
import { Employee } from '../models/employee.model';

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.css']
})
export class MainPageComponent implements OnInit {

  employeeData: any[] = [];

  constructor(private timeTrackerService: TimeTrackerService, private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.timeTrackerService.getTimeEntries().subscribe(data => {
      console.log('Raw data from service:', data); // Log raw data
      this.employeeData = this.processData(data);
      console.log('Processed employee data:', this.employeeData); // Log processed data

      // Trigger change detection manually after data is ready
      this.cdr.detectChanges();
    });
  }

  processData(employees: Array<Employee>): any[] {
    const employeeMap: any = {};

    // Filter out employees with null values in critical fields
    const validEmployees = employees.filter(employee =>
      employee.EmployeeName !== null &&
      employee.StarTimeUtc !== null &&
      employee.EndTimeUtc !== null
    );

    validEmployees.forEach(employee => {
      const employeeName = employee.EmployeeName;
      var totalTime : Number = employee.totalTime;

      if (!employeeMap[employeeName]) {
        employeeMap[employeeName] = {
          employeeName,
          totalTime: 0
        };
      }

      employeeMap[employeeName].totalTime += totalTime;
    }
  );

      // Apply ceiling to the total time after all hours have been added
    Object.keys(employeeMap).forEach(employeeName => {
      employeeMap[employeeName].totalTime = Math.ceil(employeeMap[employeeName].totalTime);
    });
    // Convert to array and sort by totalTime in descending order
    const sortedEmployees = Object.values(employeeMap).sort((a: any, b: any) => b.totalTime - a.totalTime);
    console.log('Sorted employee data:', sortedEmployees); // Log sorted data
    return sortedEmployees;
  }
}
