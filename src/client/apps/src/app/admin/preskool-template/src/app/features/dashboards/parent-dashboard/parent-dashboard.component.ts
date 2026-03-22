import { Component, ViewChild } from '@angular/core';
import { ApexStroke, ApexXAxis, ChartComponent, NgApexchartsModule } from 'ng-apexcharts';
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
  xaxis: ApexXAxis | any;
  stroke?: ApexStroke | any;

};
import {
  ApexChart,
  ApexNonAxisChartSeries,
  ApexResponsive,
  ApexPlotOptions,
  ApexDataLabels,
  ApexLegend,
  ApexGrid,
} from 'ng-apexcharts';
import { routes } from '../../../shared/routes/routes';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { SelectModule } from 'primeng/select';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'app-parent-dashboard',
    templateUrl: './parent-dashboard.component.html',
    styleUrl: './parent-dashboard.component.scss',
    imports: [BsDatepickerModule,NgApexchartsModule,SelectModule,RouterLink,CommonModule,FormsModule]
})
export class ParentDashboardComponent {
  public routes = routes;
  @ViewChild('chart') chart!: ChartComponent;
  public chartOptions: Partial<ChartOptions>;
  

  constructor( ) {
    this.chartOptions = {
      chart: {
        type: 'line',
        height: 345,
      },
      series: [{
        name: 'Avg. Exam Score',
        data: [0, 32, 40, 50, 60, 52, 50,44,40,60,75,70] // Sample data
      }, {
        name: 'Avg. Attendance',
        data: [0,35,43,34,30,28,25,50,60,75,77,80] // Sample data
      }],
      xaxis: {
        categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May','Jun','Jul','Aug','Sep','Oct','Nov','Dec']
      },
		
    };
   
   
   
  }
 
}
