import { Component } from '@angular/core';
import {
  ApexChart,
  ApexNonAxisChartSeries,
  ApexResponsive,
  ApexPlotOptions,
  ApexDataLabels,
  ApexLegend,
  ApexGrid,
  NgApexchartsModule,
} from 'ng-apexcharts';
import { routes } from '../../../shared/routes/routes';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { SelectModule } from 'primeng/select';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DatePickerModule } from 'primeng/datepicker';
export type ChartOptions = {
  series: ApexNonAxisChartSeries | any;
  chart: ApexChart | any;
  plotOptions: ApexPlotOptions | any;
  dataLabels: ApexDataLabels | any;
  labels: string[] | any;
  legend: ApexLegend | any;
  colors: string[] | any;
  responsive: ApexResponsive[] | any;
  grid?: ApexGrid | any;
};
@Component({
    selector: 'app-teacher-dashboard',
    templateUrl: './teacher-dashboard.component.html',
    styleUrl: './teacher-dashboard.component.scss',
    imports: [BsDatepickerModule,NgApexchartsModule,SelectModule,RouterLink,CommonModule,FormsModule,DatePickerModule]
})
export class TeacherDashboardComponent {
  public routes = routes;
  public studentDonutChart: Partial<ChartOptions>;
  public attendanceChart: Partial<ChartOptions>;
  date!: Date;

  constructor() {
    this.date = new Date();

    this.studentDonutChart = {
      series: [95, 5],
      chart: {
        height: 90,
        type: 'donut',
        toolbar: {
          show: false,
        },
        sparkline: {
          enabled: true,
        },
      },
      grid: {
        show: false,
        padding: {
          left: 0,
          right: 0,
        },
      },
      plotOptions: {
        bar: {
          horizontal: false,
          columnWidth: '50%',
        },
      },
      dataLabels: {
        enabled: false,
      },
      labels: ['Completed', 'Pending'],
      legend: {
        show: false,
      },
      colors: ['#1ABE17', '#E82646'],
      responsive: [
        {
          breakpoint: 480,
          options: {
            chart: {
              width: 100,
            },
            legend: {
              position: 'bottom',
            },
          },
        },
      ],
    };

    this.attendanceChart = {
      series: [60, 5, 15, 20],
      chart: {
        height: 290,
        type: 'donut',
        toolbar: {
          show: false,
        },
      },
      plotOptions: {
        bar: {
          horizontal: false,
          columnWidth: '50%',
        },
      },
      dataLabels: {
        enabled: false,
      },
      labels: ['Present', 'Late', 'Half Day', 'Absent'],
      colors: ['#1ABE17', '#1170E4', '#E9EDF4', '#E82646'],
      responsive: [
        {
          breakpoint: 480,
          options: {
            chart: {
              width: 200,
            },
            legend: {
              position: 'left',
            },
          },
        },
      ],
      legend: {
        position: 'bottom',
      },
    };
  }

}
