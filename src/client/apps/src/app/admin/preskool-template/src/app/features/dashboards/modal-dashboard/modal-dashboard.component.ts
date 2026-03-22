import { Component, Renderer2, ViewChild } from '@angular/core';

import {
  ApexAxisChartSeries,
  ApexChart,
  ApexDataLabels,
  ApexGrid,
  ApexPlotOptions,
  ApexStroke,
  ApexXAxis,
  ChartComponent,
  NgApexchartsModule,
} from 'ng-apexcharts';
import { Router, RouterLink } from '@angular/router';
import { routes } from '../../../shared/routes/routes';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { SelectModule } from 'primeng/select';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DatePickerModule } from 'primeng/datepicker';
import { CountUpModule } from 'ngx-countup';
import { CommonService } from '../../../shared/common/common.service';
import { SettingsService } from '../../../shared/settings/settings.service';
import { SidebarService } from '../../../shared/sidebar/sidebar.service';
export type ChartOptions = {
  series: ApexAxisChartSeries | any;
  chart: ApexChart | any;
  dataLabels: ApexDataLabels | any;
  plotOptions: ApexPlotOptions | any;
  xaxis: ApexXAxis | any;
  stroke: ApexStroke | any;
  grid: ApexGrid | any;
  colors:  any;
};
interface Task {
  title: string;
  time: string;
  status: string;
  completed: boolean;
}


@Component({
    selector: 'app-modal-dashboard',
    templateUrl: './modal-dashboard.component.html',
    styleUrl: './modal-dashboard.component.scss',
    imports: [BsDatepickerModule,NgApexchartsModule,SelectModule,RouterLink,CommonModule,FormsModule,DatePickerModule,CountUpModule]
})
export class ModalDashboardComponent  {
  routes =  routes
  bsValue = new Date();
  bsRangeValue: Date[];
  maxDate = new Date();
 
  base = '';
  page = '';
  last = '';
value!: Date;
 
  @ViewChild('chart') chart!: ChartComponent;
  public chartOptions: Partial<ChartOptions>;
  public chartOptions2: Partial<ChartOptions> | any;
  public chartOptions3: Partial<ChartOptions> | any;
  public chartOptions4: Partial<ChartOptions> | any;
  public chartOptions5: Partial<ChartOptions> | any;
  public chartOptions6: Partial<ChartOptions> | any;
  public chartOptions7: Partial<ChartOptions> | any;

  constructor( private common: CommonService, private renderer: Renderer2,public layout:SettingsService,private router: Router,private sidebar:SidebarService) {
    this.chartOptions = {
      chart: {
        height: 90,
        type: 'area',
        toolbar: {
          show: false,
        },
        sparkline: {
          enabled: true,
        },
      },

      dataLabels: {
        enabled: false,
      },
      stroke: {
        curve: 'straight',
      },

      series: [
        {
          name: 'Earnings',
          colors: ['#3D5EE1'],
          data: [50, 60, 40, 50, 45, 55, 50],
        },
      ],
    };
    this.chartOptions2 = {
      chart: {
        height: 90,
        type: 'area',
        toolbar: {
          show: false,
        },
        sparkline: {
          enabled: true,
        },
      },
      colors: ['#E82646'],
      dataLabels: {
        enabled: false,
      },
      stroke: {
        curve: 'straight',
      },
      series: [
        {
          name: 'Earnings',
          colors: ['#FFC38F'],
          data: [40, 20, 60, 55, 50, 55, 40],
        },
      ],
    };
    this.chartOptions3 = {
      chart: {
        height: 275,
        type: 'bar',
        stacked: true,
        toolbar: {
          show: false,
        },
      },
      legend: {
        show: true,
        horizontalAlign: 'left',
        position: 'top',
        fontSize: '14px',
        labels: {
          colors: '#5D6369',
        },
      },
      plotOptions: {
        bar: {
          horizontal: false,
          columnWidth: '50%',
          endingShape: 'rounded',
        },
      },
      colors: ['#3D5EE1', '#E9EDF4'],
      dataLabels: {
        enabled: false,
      },
      stroke: {
        show: true,
        width: 2,
        colors: ['transparent'],
      },
      grid: {
        padding: {
          left: -8,
        },
      },
      series: [
        {
          name: 'Collected Fee',
          data: [30, 40, 38, 40, 38, 30, 35, 38, 40],
        },
        {
          name: 'Total Fee',
          data: [45, 50, 48, 50, 48, 40, 40, 50, 55],
        },
      ],
      xaxis: {
        categories: [
          'Q1: 2023',
          'Q1: 2023',
          'Q1: 2023',
          'Q1: 2023',
          'Q1: 2023',
          'uQ1: 2023l',
          'Q1: 2023',
          'Q1: 2023',
          'Q1: 2023',
        ],
      },
      yaxis: {
        min: 0,
        max: 150,
        tickAmount: 3,
        labels: {
          formatter: function (value: number) {
            return value.toFixed(0); // Format labels to be integers
          },
        },
      },
    };
    this.chartOptions4 = {
      chart: {
        height: 260,
        type: 'donut',
        toolbar: {
          show: false,
        },
      },
      colors: ['#3D5EE1', '#6FCCD8'],
      series: [620, 80],
      labels: ['Present', 'Absent'],
      legend: { show: false },
      responsive: [
        {
          breakpoint: 480,
          options: {
            chart: {
              height: 150,
            },
            legend: {
              position: 'bottom',
            },
          },
        },
      ],
    };
    this.chartOptions5 = {
      chart: {
        height: 260,
        type: 'donut',
        toolbar: {
          show: false,
        },
      },
      colors: ['#3D5EE1', '#6FCCD8'],
      series: [346, 54],
      labels: ['Present', 'Absent'],
      legend: { show: false },
      responsive: [
        {
          breakpoint: 480,
          options: {
            chart: {
              height: 150,
            },
            legend: {
              position: 'bottom',
            },
          },
        },
      ],
    };
    this.chartOptions6 = {
      chart: {
        height: 260,
       
        type: 'donut',
        toolbar: {
          show: false,
        },
      },
      colors: ['#3D5EE1', '#6FCCD8'],
      series: [3610, 44],
      labels: ['Present', 'Absent'],
      legend: { show: false },
      responsive: [
        {
          breakpoint: 480,
          options: {
            chart: {
              height: 120,
            },
            legend: {
              position: 'bottom',
            },
          },
        },
      ],
    };
    this.chartOptions7 = {
      chart: {
        height: 130,
        type: 'donut',
        toolbar: {
          show: false,
        },
        sparkline: {
          enabled: true,
        },
      },
      colors: ['#3D5EE1', '#EAB300', '#E82646'],
      series: [45, 11, 2],
      labels: ['Good', 'Average', 'Below Average'],
      legend: { show: false },
      dataLabels: {
        enabled: false,
      },
      yaxis: {
        tickAmount: 3,
        labels: {
          offsetX: -15,
        },
      },
      grid: {
        padding: {
          left: -8,
        },
      },
      responsive: [
        {
          breakpoint: 480,
          options: {
            chart: {
              width: 200,
            },
            legend: {
              position: 'bottom',
            },
          },
        },
      ],
    };
    this.common.base.subscribe((res: string) => {
        this.base = res;
        if (this.base === 'layout-mini') {
          layout.changeLayoutMode('mini');
        }else if (this.base === 'layout-default'){
          layout.changeLayoutMode('default');
        }else if (this.base === 'layout-box'){
          layout.changeLayoutMode('box');
        }else if (this.base === 'layout-rtl'){
          layout.changeLayoutMode('rtl');
        }else if (this.base === 'layout-dark'){
          sidebar.themeColor();
        }else {
          layout.changeLayoutMode('default');
         if (localStorage.getItem('isDarkTheme')) {
            localStorage.removeItem('isDarkTheme');
          } 
          // layout.changeLayoutMode('1');
          // layout.changeLayoutWidth('1');
          // layout.changeThemeColor('1');
        }
      });
    this.chartOptions = {
          series: [{
          data: [400, 220, 448,]
      }],
          chart: {
          type: 'bar',
          height: 180,
      toolbar: {
        show: false
      }
      },
    
      plotOptions: {
          bar: {
          horizontal: true,
          }
      },
      dataLabels: {
          enabled: false
      },
     colors: ['#EF1E1ED9'],
    grid: {
      borderColor: '#E8E8E8',  
      strokeDashArray: 4 ,
    },
      xaxis: {
          categories: ['Conversation', 'Follow Up', 'Inpipeline'
          ],
      }
      };
    this.chartOptions2 = {
        series: [{
        data: [400, 122, 250]
    }],
        chart: {
        type: 'bar',
        height: 180,
      toolbar: {
        show: false
      }
    },
    plotOptions: {
        bar: {
        horizontal: true,
        }
    },
    dataLabels: {
        enabled: false
    },
    colors: ['#27AE60'],
    grid: {
      borderColor: '#E8E8E8',  
      strokeDashArray: 4 ,     
    },
    xaxis: {
        categories: ['Conversation', 'Follow Up', 'Inpipeline'
        ],
    }
    };
    this.chartOptions3 = {
      series: [{
      name: "sales",
      colors: ['#FFC38F'],
      data: [{
        x: 'Inpipeline',
        y: 400,
        
      }, {
        x: 'Follow Up',
        y: 130
      }, {
        x: 'Schedule',
        y: 248
      }, {
        x: 'Conversation',
        y: 470
      }, {
        x: 'Won',
        y: 470
      }, {
        x: 'Lost',
        y: 180
      }]
    }],
      chart: {
      type: 'bar',
      height: 385,
      toolbar: {
        show: false
      }
    },
    dataLabels: {
      enabled: false
    },
    grid: {
      borderColor: '#E8E8E8',  
      strokeDashArray: 4 ,     
      padding: {
        right: -20 // ✅ Remove extra right padding
      }
    },
    plotOptions: {
      bar: {
          borderRadiusApplication: 'around',
          columnWidth: '50%',
      }
    },
    colors: ['#0E9384'],
    xaxis: {
      type: 'category',
      group: {
        style: {
          fontSize: '7px',
          fontWeight: 700,
        },
      }
    },
    yaxis: {
        labels: {
          offsetX: -13,
        }
    }
    
    };
    this.chartOptions4 = {
      series: [{
        name: "Deals",
        data: [1, 2, 3, 1.5, 2.2, 4, 3.0, 2.0, 3.0, 1.8, 3.0, 6.0]
    }],
      chart: {
      height: 273,
      type: 'area',
      zoom: {
        enabled: false
      },
      toolbar: {
        show: false
      }
    },
    colors: ['#FFA201'],
    dataLabels: {
      enabled: false
    },
    stroke: {
      curve: 'straight'
    },
    fill: {
      type: 'solid',
      opacity: 0 // ✅ this removes area bg color
    },
     markers: {
      size: 5,         
      shape: 'circle', 
      strokeWidth: 2,  
      strokeColors: '#FFA201', 
      hover: {
        size: 7
      }
    },
    grid: {
      borderColor: '#E8E8E8',  
      strokeDashArray: 4 ,      // Dashed lines
    },
    xaxis: {
      categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
    }, 
    yaxis: {
      min: 1,
      max: 6,
      tickAmount: 5,
          labels: {
          offsetX: -15,
            formatter: (val:any) => {
              return val / 1 + 'K'
            }
          }
        },
        legend: {
          position: 'top',
          horizontalAlign: 'left'
        }
    };
    this.maxDate.setDate(this.maxDate.getDate() + 7);
    this.bsRangeValue = [this.bsValue, this.maxDate];
    this.common.base.subscribe((base: string) => {
      this.base = base;
    });
    this.common.page.subscribe((page: string) => {
      this.page = page;
    });
    this.common.last.subscribe((last: string) => {
      this.last = last;
    });
    if (this.page == 'deals-dashboard') {
      this.renderer.addClass(document.body, 'date-picker-dashboard');
    }
  }
   time: Date | undefined;
  time2: Date | undefined;
  addtime: Date | undefined;
  addtime2: Date | undefined;
ngOnInit():void{
   this.time= new Date();
    this.time2 = new Date();
    this.time2.setHours(6);
    this.time2.setMinutes(30);
    const isRtl = this.router.url.includes('dashboard/layout-rtl');
 
  
 
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

  toggleCompletion(task: Task) {
    task.completed = !task.completed;
  }
  customFormattingFn(value: number): string {
    return value.toString();
  }
}
