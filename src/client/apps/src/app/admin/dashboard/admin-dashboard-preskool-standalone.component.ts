import { Component, ViewChild, OnInit } from '@angular/core';

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
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { SelectModule } from 'primeng/select';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DatePickerModule } from 'primeng/datepicker';
import { CountUpModule } from 'ngx-countup';

export type ChartOptions = {
  series: ApexAxisChartSeries | any;
  chart: ApexChart | any;
  dataLabels: ApexDataLabels | any;
  plotOptions: ApexPlotOptions | any;
  xaxis: ApexXAxis | any;
  stroke: ApexStroke | any;
  grid: ApexGrid | any;
};

interface Task {
  title: string;
  time: string;
  status: string;
  completed: boolean;
}

// Preskool template routes - mapped to existing ERP routes where applicable
const routes = {
  // Dashboard
  adminDashboard: '/admin/dashboard-preskool',
  
  // Students
  addStudent: '/admin/students/list',
  editStudent: '/admin/students/list',
  studentDetails: '/admin/students/list',
  studentFees: '/admin/fees/collection',
  studentLeaves: '/admin/students/list',
  studentsList: '/admin/students/list',
  studentGrid: '/admin/students/list',
  studentPromotion: '/admin/students/list',
  studentLibrary: '/admin/library/list',
  studentTimeTable: '/admin/students/list',
  studentResult: '/admin/students/list',
  
  // Teachers
  addTeacher: '/admin/staff/list',
  editTeacher: '/admin/staff/list',
  teacherRoutine: '/admin/staff/list',
  teacherLeaves: '/admin/staff/list',
  teacherSalary: '/admin/staff/list',
  teachersList: '/admin/staff/list',
  teacherGrid: '/admin/staff/list',
  teacherDetails: '/admin/staff/list',
  teacherLibrary: '/admin/library/list',
  
  // Attendance
  teacherAttendance: '/admin/staff/attendance',
  staffAttendance: '/admin/staff/attendance',
  studentAttendance: '/admin/students/attendance',
  
  // Fees
  collectFees: '/admin/fees/collection',
  feesGroup: '/admin/fees/collection',
  feesMaster: '/admin/fees/collection',
  feesType: '/admin/fees/collection',
  feesAssign: '/admin/fees/collection',
  
  // Events & Communication
  events: '/admin/events',
  noticeBoard: '/admin/communication/notice-board',
  
  // Profile
  profile: '/admin/profile',
  
  // Membership (placeholder)
  membershipPlans: '/admin/membership',
  
  // Other routes (placeholders - update when modules are available)
  classRoom: '/admin/academics',
  classTimeTable: '/admin/academics',
  classSyllabus: '/admin/academics',
  classSubject: '/admin/academics',
  classSection: '/admin/academics',
  classRoutine: '/admin/academics',
  classHomeWork: '/admin/academics',
  
  // Examinations
  examResults: '/admin/examination',
  examSchedule: '/admin/examination',
  examList: '/admin/examination',
  
  // Library
  issueBook: '/admin/library/issue-return',
  returnBook: '/admin/library/issue-return',
  books: '/admin/library/list',
  libraryMembers: '/admin/library/list',
  
  // Hostel
  hostelRooms: '/admin/hostel/rooms',
  hostelRoomType: '/admin/hostel/rooms',
  hostelList: '/admin/hostel/rooms',
  
  // Transport
  transportVehicle: '/admin/transport/list',
  transportVehicleDrivers: '/admin/transport/list',
  transportAssignVehicle: '/admin/transport/list',
  transportPickupPoints: '/admin/transport/list',
  transportRoutes: '/admin/transport/list',
};

@Component({
  selector: 'app-admin-dashboard-preskool-standalone',
  standalone: true,
  templateUrl: './admin-dashboard-preskool-standalone.component.html',
  styleUrl: './admin-dashboard-preskool-standalone.component.scss',
  imports: [
    BsDatepickerModule,
    NgApexchartsModule,
    SelectModule,
    RouterLink,
    CommonModule,
    FormsModule,
    DatePickerModule,
    CountUpModule,
  ],
})
export class AdminDashboardPreskoolStandaloneComponent implements OnInit {
  public routes = routes;
  value!: Date;

  @ViewChild('chart') chart!: ChartComponent;
  public chartOptions: Partial<ChartOptions>;
  public chartOptions2: Partial<ChartOptions> | any;
  public chartOptions3: Partial<ChartOptions> | any;
  public chartOptions4: Partial<ChartOptions> | any;
  public chartOptions5: Partial<ChartOptions> | any;
  public chartOptions6: Partial<ChartOptions> | any;
  public chartOptions7: Partial<ChartOptions> | any;

  constructor(private router: Router) {
    // Chart 1: Earnings (Area chart)
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

    // Chart 2: Expenses (Area chart)
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

    // Chart 3: Fees Collection (Bar chart)
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
          'Q2: 2023',
          'Q3: 2023',
          'Q4: 2023',
          'Q1: 2024',
          'Q2: 2024',
          'Q3: 2024',
          'Q4: 2024',
          'Q1: 2025',
        ],
      },
      yaxis: {
        min: 0,
        max: 150,
        tickAmount: 3,
        labels: {
          formatter: function (value: number) {
            return value.toFixed(0);
          },
        },
      },
    };

    // Chart 4: Staff Attendance (Donut chart)
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

    // Chart 5: Teacher Attendance (Donut chart)
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

    // Chart 6: Student Attendance (Donut chart)
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

    // Chart 7: Performance (Donut chart)
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
  }

  time: Date | undefined;
  time2: Date | undefined;
  addtime: Date | undefined;
  addtime2: Date | undefined;

  ngOnInit() {
    this.time = new Date();
    this.time.setHours(8);
    this.time.setMinutes(30);
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

