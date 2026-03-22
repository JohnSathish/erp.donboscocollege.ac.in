import { Component, ElementRef, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { ApexAnnotations, ApexFill, ApexLegend, ApexResponsive, ApexYAxis, ChartComponent, NgApexchartsModule } from 'ng-apexcharts';

import {
  ApexAxisChartSeries,
  ApexChart,
  ApexDataLabels,
  ApexGrid,
  ApexPlotOptions,
  ApexStroke,
  ApexXAxis,
 
} from 'ng-apexcharts';


import { routes } from '../../../shared/routes/routes';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { SelectModule } from 'primeng/select';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DatePickerModule } from 'primeng/datepicker';
interface data {
  name: string | undefined;
  code: string | undefined;
}
interface Task {
  title: string;
  time: string;
  status: string;
  completed: boolean;
}
export type ChartOptions = {
  series: ApexAxisChartSeries | any;
  chart: ApexChart | any;
  dataLabels: ApexDataLabels | any;
  plotOptions: ApexPlotOptions | any;
  xaxis: ApexXAxis | any;
  stroke: ApexStroke | any;
  grid: ApexGrid | any;
  annotations:ApexAnnotations | any;
  fill: ApexFill | any
  yaxis:ApexYAxis | any;
  labels: string[] | any;
  colors: string[] | any;
  responsive: ApexResponsive[] | any;
  legend: ApexLegend | any;
 
};
@Component({
    selector: 'app-student-dashboard',
    templateUrl: './student-dashboard.component.html',
    styleUrl: './student-dashboard.component.scss',
    imports: [BsDatepickerModule,NgApexchartsModule,SelectModule,RouterLink,CommonModule,FormsModule,DatePickerModule]
})
export class StudentDashboardComponent {
  addNewRow() {
    this.formData.push({ selectedClass: {name: undefined,code:undefined}});
  }
  removeRow(index: number) {
    this.formData.splice(index, 1);
  }
  public routes = routes;
  value!: Date;
  date: Date | undefined;
  @ViewChild('chart') chart!: ChartComponent;
  @ViewChildren('circleProgress') circleProgressElements!: QueryList<ElementRef>;
  public chartOptions: Partial<ChartOptions>;
  public attendanceChart: Partial<ChartOptions>;
  public chartOptions2: Partial<ChartOptions>;
  formData: { selectedClass: data | undefined }[] = [];
  constructor() {
    this.date = new Date();
    this.chartOptions = {
      chart: {
        type: 'bar',
        height: 310
      },
      series: [{
        name: 'Marks',
        data: [100, 92, 90, 82, 90] // Corresponding scores for Maths, Physics, Chemistry, English, Spanish
      }],
      xaxis: {
        categories: ['Mat', 'Phy', 'Che', 'Eng', 'Sci']
      },
      colors: ['#E9EDF4', '#3D5EE1', '#E9EDF4', '#E9EDF4', '#E9EDF4'], // Set specific colors for each bar
      plotOptions: {
        bar: {
          distributed: true,
          columnWidth: '50%',
          colors: {
            backgroundBarColors: ['#E9EDF4', '#fff'],
            backgroundBarOpacity: 1,
            backgroundBarRadius: 5,
          },
          dataLabels: {
            position: 'top'
          },
        }
      },
    
     
     
      grid: {
        yaxis: {
          lines: {
            show: false
          }
        },
      },
      
     
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
    this.chartOptions2 = {
      chart: {
        type: 'area',
        height: 355
      },
      series: [{
        name: 'Avg. Exam Score',
        data: [75, 68, 65, 68, 75] // Sample data
      }, {
        name: 'Avg. Attendance',
        data: [85, 78, 75, 78, 85] // Sample data
      }],
      xaxis: {
        categories: ['Quarter 1', 'Quarter 2', 'Half yearly', 'Model', 'Final']
      },
    };
   
  }
 

  toggleCompletion(task: Task) {
    task.completed = !task.completed;
  }
  tasks: Task[] = [
    {
      title: 'Send Reminder to Students',
      time: '01:00 PM',
      status: 'Completed',
      completed: true,
    },
    {
      title: 'Create Routine to new staff',
      time: '04:50 PM',
      status: 'Inprogress',
      completed: false,
    },
    {
      title: 'Extra Class Info to Students',
      time: '04:55 PM',
      status: 'Yet to Start',
      completed: false,
    },
    {
      title: 'Fees for Upcoming Academics',
      time: '04:55 PM',
      status: 'Yet to Start',
      completed: false,
    },
    {
      title: 'English - Essay on Visit',
      time: '05:55 PM',
      status: 'Yet to Start',
      completed: false,
    },
  ];
  ngAfterViewInit() {
    this.updateProgress();
  }
  updateProgress() {
    this.circleProgressElements.forEach(elementRef => {
      const element = elementRef.nativeElement;
      const value = parseInt(element.getAttribute('data-value'), 10);
      const left = element.querySelector('.progress-left .progress-bar');
      const right = element.querySelector('.progress-right .progress-bar');

      if (value > 0) {
        if (value <= 50) {
          right.style.transform = 'rotate(' + this.percentageToDegrees(value) + 'deg)';
        } else {
          right.style.transform = 'rotate(180deg)';
          left.style.transform = 'rotate(' + this.percentageToDegrees(value - 50) + 'deg)';
        }
      }
    });
  }

  percentageToDegrees(percentage: number): number {
    return (percentage / 100) * 360;
  }

}
