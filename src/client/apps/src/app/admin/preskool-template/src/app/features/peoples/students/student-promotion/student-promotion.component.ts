import { Component } from '@angular/core';
import { apiResultFormat, studentlist, pageSelection } from '../../../../shared/model/pages.model';
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
import { DateRangePickerComponent } from '../../../common/date-range-picker/date-range-picker.component';
import { TooltipContentComponent } from '../../../common/tooltip-content/tooltip-content.component';
interface data {
  name: string;
  code: string;
}
@Component({
    selector: 'app-student-promotion',
    templateUrl: './student-promotion.component.html',
    styleUrl: './student-promotion.component.scss',
    imports: [BsDatepickerModule,SelectModule,CustomPaginationComponent,FormsModule,MatSortModule,RouterLink,CommonModule,DateRangePickerComponent,TooltipContentComponent]
})
export class StudentPromotionComponent {
  public routes = routes;
  // pagination variables
  public tableData: Array<studentlist> = [];
  public pageSize = 10;
  public serialNumberArray: Array<number> = [];
  public totalData = 0;
  showFilter = false;
  dataSource!: MatTableDataSource<studentlist>;
  public searchDataValue = '';
  public tableDataCopy: Array<studentlist> = [];
  public actualData: Array<studentlist> = [];
  //** pagination variables
  initChecked = false;
  show = false
  role: data[] | undefined;
  selectedRole1: data | undefined;
  selectedRole2: data | undefined;
  constructor(
    private data: DataService,
    private pagination: PaginationService,
    private router: Router
  ) {
    this.data.getStudent().subscribe((apiRes: apiResultFormat) => {
      this.actualData = apiRes.data;
      this.pagination.tablePageSize.subscribe((res: tablePageSize) => {
        if (this.router.url == this.routes.studentPromotion) {
          this.getTableData({ skip: res.skip, limit: res.limit });
          this.pageSize = res.pageSize;
        }
      });
    });
  }
  ngOnInit() {

    this.role = [
      { name: 'Promote to IV', code: '1' },
      { name: 'No Promotion', code: '2' },
    ];
    this.selectedRole1 = this.role[0];
    this.selectedRole2 = this.role[1];
  }
  private getTableData(pageOption: pageSelection): void {
    this.data.getStudent().subscribe((apiRes: apiResultFormat) => {
      this.tableData = [];
      this.tableDataCopy = [];
      this.serialNumberArray = [];
      this.totalData = apiRes.totalData;
      apiRes.data.map((res: studentlist, index: number) => {
        const serialNumber = index + 1;
        if (index >= pageOption.skip && serialNumber <= pageOption.limit) {
          res.id = serialNumber;
          this.tableData.push(res);
          this.serialNumberArray.push(serialNumber);
          this.tableDataCopy.push(res);
        }
      });
      this.dataSource = new MatTableDataSource<studentlist>(this.actualData);
      this.pagination.calculatePageSize.next({
        totalData: this.totalData,
        pageSize: this.pageSize,
        tableData: this.tableData,
        tableDataCopy: this.tableDataCopy,
        serialNumberArray: this.serialNumberArray,
      });
    });
  }
  public showOption() {
    this.show = !this.show
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
