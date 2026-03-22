import { Component, OnInit, Renderer2 } from '@angular/core';
import { routes } from '../../../shared/routes/routes';
import { MatSortModule, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router, RouterLink } from '@angular/router';
import { DataService } from '../../../shared/data/data.service';
import {
  pageSelection,
  PaginationService,
  tablePageSize,
} from '../../../shared/custom-pagination/pagination.service';
import { classRoutineData, apiResultFormat } from '../../../shared/model/pages.model';
import { SelectModule } from 'primeng/select';
import { CustomPaginationComponent } from '../../../shared/custom-pagination/custom-pagination.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DateRangePickerComponent } from '../../common/date-range-picker/date-range-picker.component';
import { TooltipContentComponent } from '../../common/tooltip-content/tooltip-content.component';
import { DatePickerModule } from 'primeng/datepicker';
interface data {
  name: string;
  code: string;
}

@Component({
    selector: 'app-class-routine',
    templateUrl: './class-routine.component.html',
    styleUrl: './class-routine.component.scss',
    imports: [SelectModule,CustomPaginationComponent,CommonModule,FormsModule,MatSortModule,DateRangePickerComponent,TooltipContentComponent,DatePickerModule,RouterLink]
})
export class ClassRoutineComponent {
  public routes = routes;
  class: data[] | undefined;
  roomNo: data[] | undefined;
  section: data[] | undefined;
  teacher: data[] | undefined;
  day: data[] | undefined;
  selectedClass: data | undefined;
  selectedRoom: data | undefined;
  selectedSection: data | undefined;
  selectedTeacher: data | undefined;
  selectedDay: data | undefined;
  selectedClass2: data | undefined;
  selectedRoom2: data | undefined;
  selectedSection2: data | undefined;
  selectedTeacher2: data | undefined;
  selectedDay2: data | undefined;
  bsValue = new Date();
  initChecked = false;
  time: Date | undefined;
  time2: Date | undefined;
  addtime: Date | undefined;
  addtime2: Date | undefined;
  public pageSize = 10;
  public tableData: Array<classRoutineData> = [];
  public tableDataCopy: Array<classRoutineData> = [];
  public actualData: Array<classRoutineData> = [];
  public currentPage = 1;
  // pagination variables
  public skip = 0;
  public limit: number = this.pageSize;

  public serialNumberArray: Array<number> = [];
  public totalData = 0;
  showFilter = false;
  public pageSelection: pageSelection[] = [];
  dataSource!: MatTableDataSource<classRoutineData>;
  public searchDataValue = '';
  constructor(
    private renderer: Renderer2,
    private data: DataService,
    private pagination: PaginationService,
    private router: Router
  ) {
    this.data.getClassRoutine().subscribe((apiRes: apiResultFormat) => {
      this.actualData = apiRes.data;
      this.pagination.tablePageSize.subscribe((res: tablePageSize) => {
        if (this.router.url == this.routes.classRoutine) {
          this.getTableData({ skip: res.skip, limit: res.limit });
          this.pageSize = res.pageSize;
        }
      });
    });
    
  }

  public handleApplyClick = () => {
    const modalElements = document.getElementsByClassName('drop-width');
    if (modalElements.length > 0) {
      for (let i = 0; i < modalElements.length; i++) {
        const modalElement = modalElements[i];
        this.renderer.removeClass(modalElement, 'show');
      }
    }
  };
  ngOnInit() {
    this.time = new Date();
    this.time.setHours(8);
    this.time.setMinutes(30);
    this.time2 = new Date();
    this.time2.setHours(6);
    this.time2.setMinutes(30);
    this.class = [
      { name: 'I', code: '1' },
      { name: 'II', code: '2' },
      { name: 'III', code: '3' },
    ];
    this.day = [
      { name: 'Monday', code: '1' },
      { name: 'Tuesday', code: '2' },
      { name: 'Wednesday', code: '3' },
      { name: 'Thursday', code: '4' },
      { name: 'Friday', code: '5' },
    ];
    this.teacher = [
      { name: 'Erickson', code: '1' },
      { name: 'Mori', code: '2' },
      { name: 'Joseph', code: '3' },
      { name: 'James', code: '3' },
    ];
    this.section = [
      { name: 'A', code: '1' },
      { name: 'B', code: '2' },
      { name: 'C', code: '3' },
    ];
    this.roomNo = [
      { name: '101', code: '1' },
      { name: '102', code: '2' },
      { name: '103', code: '3' },
    ];
    this.selectedDay2 = this.day[0]
    this.selectedTeacher2 = this.teacher[0]
    this.selectedSection2 = this.section[0]
    this.selectedRoom2 = this.roomNo[0]
    this.selectedClass2 = this.class[0]
  }
  private getTableData(pageOption: pageSelection): void {
    this.data.getClassRoutine().subscribe((apiRes: apiResultFormat) => {
      this.tableData = [];
      this.tableDataCopy = [];
      this.serialNumberArray = [];
      this.totalData = apiRes.totalData;
      apiRes.data.map((res: classRoutineData, index: number) => {
        const serialNumber = index + 1;
        if (index >= pageOption.skip && serialNumber <= pageOption.limit) {
          res.key = serialNumber;
          this.tableData.push(res);
          this.tableDataCopy.push(res);
          this.serialNumberArray.push(serialNumber);
        }
      });
      this.dataSource = new MatTableDataSource<classRoutineData>(this.actualData);
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
