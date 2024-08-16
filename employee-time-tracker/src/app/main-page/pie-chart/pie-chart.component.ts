import { Component, Input, OnInit, OnDestroy, SimpleChanges, OnChanges } from '@angular/core';
import { Chart, registerables } from 'chart.js';

@Component({
  selector: 'app-pie-chart',
  templateUrl: './pie-chart.component.html',
  styleUrls: ['./pie-chart.component.css']
})
export class PieChartComponent implements OnInit, OnDestroy, OnChanges {

  @Input() employeeData: any[] = [];
  private chart: any;

  constructor() { }

  ngOnInit(): void {
    // Register Chart.js components
    Chart.register(...registerables);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['employeeData'] && this.employeeData.length > 0) {
      this.initializeChart();
    }
  }

  ngOnDestroy(): void {
    // Destroy the chart to prevent memory leaks
    if (this.chart) {
      this.chart.destroy();
    }
  }

  initializeChart(): void {
    const ctx = document.getElementById('pieChart') as HTMLCanvasElement;
    
    if (this.chart) {
      this.chart.destroy();  // Destroy previous chart instance if it exists
    }

    const total = this.employeeData.reduce((sum, emp) => sum + emp.totalTime, 0);

    this.chart = new Chart(ctx, {
      type: 'pie',
      data: {
        labels: this.employeeData.map(emp => emp.employeeName),
        datasets: [{
          data: this.employeeData.map(emp => emp.totalTime),
          backgroundColor: this.employeeData.map(() => this.getRandomColor())
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          tooltip: {
            callbacks: {
              label: (tooltipItem: any) => {
                const value = tooltipItem.raw;
                const percentage = ((value / total) * 100).toFixed(2);
                return `${tooltipItem.label}: ${percentage}% (${value} hours)`;
              }
            }
          },
        }
      }
    });
  }

  getRandomColor(): string {
    const letters = '0123456789ABCDEF';
    let color = '#';
    for (let i = 0; i < 6; i++) {
      color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
  }
}