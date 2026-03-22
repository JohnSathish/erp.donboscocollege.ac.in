import { Component } from '@angular/core';
import { MatSortModule, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router, RouterLink } from '@angular/router';
import { PaginationService, tablePageSize } from '../../../../shared/custom-pagination/pagination.service';
import { DataService } from '../../../../shared/data/data.service';
import { teacherSalary, apiResultFormat, pageSelection } from '../../../../shared/model/pages.model';
import { routes } from '../../../../shared/routes/routes';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { SelectModule } from 'primeng/select';
import { CustomPaginationComponent } from '../../../../shared/custom-pagination/custom-pagination.component';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'app-teacher-salary',
    templateUrl: './teacher-salary.component.html',
    styleUrl: './teacher-salary.component.scss',
    imports: [BsDatepickerModule,SelectModule,CustomPaginationComponent,FormsModule,MatSortModule,RouterLink]
})
export class TeacherSalaryComponent {
  public routes = routes;
  // pagination variables
  public tableData: Array<teacherSalary> = [];
  public pageSize = 10;
  public serialNumberArray: Array<number> = [];
  public totalData = 0;
  showFilter = false;
  dataSource!: MatTableDataSource<teacherSalary>;
  public searchDataValue = '';
  public tableDataCopy: Array<teacherSalary> = [];
  public actualData: Array<teacherSalary> = [];
  //** pagination variables
  initChecked = false;

  constructor(
    private data: DataService,
    private pagination: PaginationService,
    private router: Router
  ) {
    this.data.getTeacherSalary().subscribe((apiRes: apiResultFormat) => {
      this.actualData = apiRes.data;
      this.pagination.tablePageSize.subscribe((res: tablePageSize) => {
        if (this.router.url == this.routes.teacherSalary) {
          this.getTableData({ skip: res.skip, limit: res.limit });
          this.pageSize = res.pageSize;
        }
      });
    });
  }

  private getTableData(pageOption: pageSelection): void {
    this.data.getTeacherSalary().subscribe((apiRes: apiResultFormat) => {
      this.tableData = [];
      this.tableDataCopy = [];
      this.serialNumberArray = [];
      this.totalData = apiRes.totalData;
      apiRes.data.map((res: teacherSalary, index: number) => {
        const serialNumber = index + 1;
        if (index >= pageOption.skip && serialNumber <= pageOption.limit) {
          res.id = serialNumber;
          this.tableData.push(res);
          this.serialNumberArray.push(serialNumber);
          this.tableDataCopy.push(res);
        }
      });
      this.dataSource = new MatTableDataSource<teacherSalary>(this.actualData);
      this.pagination.calculatePageSize.next({
        totalData: this.totalData,
        pageSize: this.pageSize,
        tableData: this.tableData,
        tableDataCopy: this.tableDataCopy,
        serialNumberArray: this.serialNumberArray,
      });
    });
  }

  public sortData(sort: Sort) {
    const data = this.tableData.slice();
    if (!sort.active || sort.direction === '') {
      this.tableData = data;
    } else {
      this.tableData = data.sort((a, b) => {
        const aValue = (a as never)[sort.active];
        const bValue = (b as never)[sort.active];
        return (aValue < bValue ? -1 : 1) * (sort.direction === 'asc' ? 1 : -1);
      });
    }
  }
  public row=true;
  public searchData(value: string): void {
    this.searchDataValue = value.trim().toLowerCase();
    this.dataSource.filter = this.searchDataValue;
    this.tableData = this.dataSource.filteredData;
    this.row = this.tableData.length > 0;

    if (this.searchDataValue !== '') {
      // Handle filtered data
      this.pagination.calculatePageSize.next({
        totalData: this.tableData.length,
        pageSize: this.pageSize,
        tableData: this.tableData,
        serialNumberArray: this.tableData.map((_, i) => i + 1), 
      });
    } else {
      // Handle reset to full data
      this.pagination.calculatePageSize.next({
        totalData: this.totalData,
        pageSize: this.pageSize,
        tableData: this.tableData,
        serialNumberArray: this.serialNumberArray,
      });
    }
  }
  selectAll(initChecked: boolean) {
    if (!initChecked) {
      this.tableData.forEach((f) => {
        f.isSelected = true;
      });
    } else {
      this.tableData.forEach((f) => {
        f.isSelected = false;
      });
    }
  }
}
