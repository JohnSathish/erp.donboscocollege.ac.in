import { Component } from '@angular/core';
import { apiResultFormat, teacherLeaves, pageSelection, studentLeaveAttendance } from '../../../../shared/model/pages.model';
import { routes } from '../../../../shared/routes/routes';
import { MatSortModule, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router, RouterLink } from '@angular/router';
import { PaginationService, tablePageSize } from '../../../../shared/custom-pagination/pagination.service';
import { DataService } from '../../../../shared/data/data.service';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { SelectModule } from 'primeng/select';
import { CustomPaginationComponent } from '../../../../shared/custom-pagination/custom-pagination.component';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-student-leaves',
    templateUrl: './student-leaves.component.html',
    styleUrl: './student-leaves.component.scss',
    imports: [BsDatepickerModule,SelectModule,CustomPaginationComponent,FormsModule,MatSortModule,RouterLink,CommonModule]
})
export class StudentLeavesComponent {
  public routes = routes;
  // pagination variables
  public tableData: Array<teacherLeaves> = [];
  public pageSize = 10;
  public serialNumberArray: Array<number> = [];
  public totalData = 0;
  dataSource!: MatTableDataSource<teacherLeaves>;
  public searchDataValue = '';
  public tableDataCopy: Array<teacherLeaves> = [];
  public actualData: Array<teacherLeaves> = [];
  //** pagination variables
  // pagination variables
  public tableData2: Array<studentLeaveAttendance> = [];
  public pageSize2 = 10;
  public serialNumberArray2: Array<number> = [];
  public totalData2 = 0;
  dataSource2!: MatTableDataSource<studentLeaveAttendance>;
  public searchDataValue2 = '';
  public tableDataCopy2: Array<studentLeaveAttendance> = [];
  public actualData2: Array<studentLeaveAttendance> = [];
  //** pagination variables
  initChecked = false;

  constructor(
    private data: DataService,
    private pagination: PaginationService,
    private router: Router
  ) {
    this.data.getTeacherLeaves().subscribe((apiRes: apiResultFormat) => {
      this.actualData = apiRes.data;
      this.pagination.tablePageSize.subscribe((res: tablePageSize) => {
        if (this.router.url == this.routes.studentLeaves) {
          this.getTableData({ skip: res.skip, limit: res.limit });
          this.pageSize = res.pageSize;
        }
      });
    });
    this.data.getstudentLeaveAttendance().subscribe((apiRes: apiResultFormat) => {
      this.actualData2 = apiRes.data;
      this.pagination.tablePageSize.subscribe((res: tablePageSize) => {
        if (this.router.url == this.routes.studentLeaves) {
          this.getTableData2({ skip: res.skip, limit: res.limit });
          this.pageSize2 = res.pageSize;
        }
      });
    });
  }

  private getTableData(pageOption: pageSelection): void {
    this.data.getTeacherLeaves().subscribe((apiRes: apiResultFormat) => {
      this.tableData = [];
      this.tableDataCopy = [];
      this.serialNumberArray = [];
      this.totalData = apiRes.totalData;
      apiRes.data.map((res: teacherLeaves, index: number) => {
        const serialNumber = index + 1;
        if (index >= pageOption.skip && serialNumber <= pageOption.limit) {
          res.id = serialNumber;
          this.tableData.push(res);
          this.serialNumberArray.push(serialNumber);
          this.tableDataCopy.push(res);
        }
      });
      this.dataSource = new MatTableDataSource<teacherLeaves>(this.actualData);
      this.pagination.calculatePageSize.next({
        totalData: this.totalData,
        pageSize: this.pageSize,
        tableData: this.tableData,
        tableDataCopy: this.tableDataCopy,
        serialNumberArray: this.serialNumberArray,
      });
    });
  }
  private getTableData2(pageOption: pageSelection): void {
    this.data.getstudentLeaveAttendance().subscribe((apiRes: apiResultFormat) => {
      this.tableData2 = [];
      this.tableDataCopy2 = [];
      this.serialNumberArray2 = [];
      this.totalData2 = apiRes.totalData;
      apiRes.data.map((res: studentLeaveAttendance, index: number) => {
        const serialNumber = index + 1;
        if (index >= pageOption.skip && serialNumber <= pageOption.limit) {
          res.key = serialNumber;
          this.tableData2.push(res);
          this.serialNumberArray2.push(serialNumber);
          this.tableDataCopy2.push(res);
        }
      });
      this.dataSource2 = new MatTableDataSource<studentLeaveAttendance>(this.actualData2);
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
  public sortData2(sort: Sort) {
    const data = this.tableData2.slice();
    if (!sort.active || sort.direction === '') {
      this.tableData2 = data;
    } else {
      this.tableData2 = data.sort((a, b) => {
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
public searchData2(value: string): void {
    this.searchDataValue = value.trim().toLowerCase();
    this.dataSource2.filter = this.searchDataValue;
    this.tableData2 = this.dataSource2.filteredData;
    this.row = this.tableData2.length > 0;

    if (this.searchDataValue !== '') {
      // Handle filtered data
      this.pagination.calculatePageSize.next({
        totalData: this.tableData2.length,
        pageSize: this.pageSize,
        tableData: this.tableData2,
        serialNumberArray: this.tableData2.map((_, i) => i + 1), 
      });
    } else {
      // Handle reset to full data
      this.pagination.calculatePageSize.next({
        totalData: this.totalData,
        pageSize: this.pageSize,
        tableData: this.tableData2,
        serialNumberArray: this.serialNumberArray,
      });
    }
  }

}
