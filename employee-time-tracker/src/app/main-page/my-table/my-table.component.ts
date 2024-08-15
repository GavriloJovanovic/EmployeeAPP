import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-my-table',
  templateUrl: './my-table.component.html',
  styleUrls: ['./my-table.component.css']
})
export class MyTableComponent {

  @Input() employeeData: any[] = [];

  constructor() { }

  ngOnInit(): void {
    this.employeeData.sort((a, b) => b.totalTime - a.totalTime);
  }

  getRowClass(totalTime: number): string {
    return totalTime < 100 ? 'highlight' : '';
  }
}
