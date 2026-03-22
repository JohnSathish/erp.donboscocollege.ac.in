import { Component, inject, OnInit, signal, CUSTOM_ELEMENTS_SCHEMA, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AdmissionsAdminApiService } from '@client/shared/data';
import { AdminDashboardDto } from '@client/shared/data';
import { KpiCardComponent } from '../shared/components/kpi-card.component';
import { NgApexchartsModule } from 'ng-apexcharts';
import {
  ApexAxisChartSeries,
  ApexChart,
  ApexXAxis,
  ApexYAxis,
  ApexStroke,
  ApexTooltip,
  ApexLegend,
  ApexDataLabels,
  ApexNonAxisChartSeries,
  ApexResponsive,
} from 'ng-apexcharts';

export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  yaxis: ApexYAxis;
  stroke: ApexStroke;
  tooltip: ApexTooltip;
  legend: ApexLegend;
  colors: string[];
};

export type BarChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  colors: string[];
};

export type DonutChartOptions = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  labels: string[];
  colors: string[];
  legend: ApexLegend;
};

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, KpiCardComponent, NgApexchartsModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss'],
})
export class AdminDashboardComponent implements OnInit {
  private readonly api = inject(AdmissionsAdminApiService);

  protected readonly dashboard = signal<AdminDashboardDto | null>(null);
  protected readonly loading = signal(false);
  protected readonly error = signal<string | null>(null);
  protected activeTab: 'registered' | 'subscribed' = 'registered';

  // Chart Options
  chartOptions: ChartOptions = {
    series: [
      {
        name: 'Applications',
        data: [10, 15, 12, 18, 20, 25, 22],
      },
    ],
    chart: {
      type: 'line',
      height: 300,
      toolbar: { show: false },
    },
    xaxis: {
      categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul'],
    },
    yaxis: {
      labels: {
        formatter: (val: number) => `$${val}k`,
      },
    },
    stroke: {
      curve: 'smooth',
      width: 3,
    },
    tooltip: {
      theme: 'dark',
    },
    legend: {
      show: false,
    },
    colors: ['#487FFF'],
  };

  barChartOptions: BarChartOptions = {
    series: [
      {
        name: 'Applications',
        data: [20, 25, 30, 35, 40, 35, 30],
      },
    ],
    chart: {
      type: 'bar',
      height: 200,
      toolbar: { show: false },
    },
    xaxis: {
      categories: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
    },
    colors: ['#487FFF'],
  };

  donutChartOptions: DonutChartOptions = {
    series: [400, 300],
    chart: {
      type: 'donut',
      height: 200,
    },
    labels: ['Submitted', 'Approved'],
    colors: ['#487FFF', '#FF9F29'],
    legend: {
      show: false,
    },
  };

  // Computed data for tables
  protected readonly recentApplications = computed(() => {
    const data = this.dashboard();
    if (!data) return [];
    // Mock data - replace with actual API data when available
    return [
      { id: 1, name: 'John Doe', email: 'john@example.com', submittedDate: new Date(), status: 'Submitted' },
      { id: 2, name: 'Jane Smith', email: 'jane@example.com', submittedDate: new Date(), status: 'Pending' },
    ];
  });

  protected readonly approvedApplications = computed(() => {
    const data = this.dashboard();
    if (!data) return [];
    // Mock data - replace with actual API data when available
    return [
      { id: 1, name: 'John Doe', email: 'john@example.com', approvedDate: new Date() },
      { id: 2, name: 'Jane Smith', email: 'jane@example.com', approvedDate: new Date() },
    ];
  });

  ngOnInit(): void {
    this.loadDashboard();
  }

  async loadDashboard(): Promise<void> {
    this.loading.set(true);
    this.error.set(null);
    try {
      const data = await this.api.getAdminDashboard().toPromise();
      if (data) {
        this.dashboard.set(data);
        this.updateCharts(data);
      }
    } catch (err) {
      console.error('Failed to load dashboard', err);
      this.error.set('Unable to load dashboard data. Please retry.');
    } finally {
      this.loading.set(false);
    }
  }

  private updateCharts(data: AdminDashboardDto): void {
    // Update donut chart with actual data
    this.donutChartOptions = {
      ...this.donutChartOptions,
      series: [
        data.statisticsByStatus.submitted,
        data.statisticsByStatus.approved,
      ],
    };
  }
}
