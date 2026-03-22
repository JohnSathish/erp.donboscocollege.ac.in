import { Component } from '@angular/core';
import { apiResultFormat, cities, dataTables } from '../../../../shared/model/pages.model';
import { routes } from '../../../../shared/routes/routes';
import { pageSelection, PaginationService, tablePageSize } from '../../../../shared/custom-pagination/pagination.service';
import { MatTableDataSource } from '@angular/material/table';
import { DataService } from '../../../../shared/data/data.service';
import { Router, RouterLink } from '@angular/router';
import { MatSortModule, Sort } from '@angular/material/sort';
import { CustomPaginationComponent } from '../../../../shared/custom-pagination/custom-pagination.component';
import { FormsModule } from '@angular/forms';
import { DateRangePickerComponent } from '../../../common/date-range-picker/date-range-picker.component';
import { TooltipContentComponent } from '../../../common/tooltip-content/tooltip-content.component';
import { CommonModule } from '@angular/common';
import { SelectModule } from 'primeng/select';
interface data {
  name: string;
  code: string;
}

@Component({
    selector: 'app-cities',
    templateUrl: './cities.component.html',
    styleUrl: './cities.component.scss',
    imports: [CustomPaginationComponent,FormsModule,MatSortModule,DateRangePickerComponent,TooltipContentComponent,RouterLink,CommonModule,SelectModule]
})
export class CitiesComponent {
  initChecked = false;
  selectedState: data | undefined;
  selectedCountry: data | undefined;
  selectedCity: data | undefined;
  selectedStatus: data | undefined;
  state: data[] | undefined;
  country: data[] | undefined;
  cities: data[] | undefined;
  status: data[] | undefined;
  public pageSize = 10;
  public tableData: Array<cities> = [];
  public tableDataCopy: Array<cities> = [];
  public actualData: Array<cities> = [];
  public routes = routes;
  public currentPage = 1;
  // pagination variables
  public skip = 0;
  public limit: number = this.pageSize;

  public serialNumberArray: Array<number> = [];
  public totalData = 0;
  showFilter = false;
  public pageSelection: pageSelection[] = [];
  dataSource!: MatTableDataSource<cities>;
  public searchDataValue = '';

  constructor(
    private data: DataService,
    private router: Router,
    private pagination: PaginationService
  ) {
    this.data.getCites().subscribe((apiRes: apiResultFormat) => {
      this.actualData = apiRes.data;
      this.pagination.tablePageSize.subscribe((res: tablePageSize) => {
        if (this.router.url == this.routes.cities) {
          this.getTableData({ skip: res.skip, limit: res.limit });
          this.pageSize = res.pageSize;
        }
      });
    });
  }

  private getTableData(pageOption: pageSelection): void {
    this.data.getCites().subscribe((apiRes: apiResultFormat) => {
      this.tableData = [];
      this.tableDataCopy = [];
      this.serialNumberArray = [];
      this.totalData = apiRes.totalData;
      apiRes.data.map((res: cities, index: number) => {
        const serialNumber = index + 1;
        if (index >= pageOption.skip && serialNumber <= pageOption.limit) {
          res.sNo = serialNumber;
          this.tableData.push(res);
          this.tableDataCopy.push(res);
          this.serialNumberArray.push(serialNumber);
        }
      });
      this.dataSource = new MatTableDataSource<cities>(this.actualData);
      this.pagination.calculatePageSize.next({
        totalData: this.totalData,
        pageSize: this.pageSize,
        tableData: this.tableData,
        tableDataCopy: this.tableDataCopy,
        serialNumberArray: this.serialNumberArray,
      });
    });
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
  public changePageSize(pageSize: number): void {
    this.pageSelection = [];
    this.limit = pageSize;
    this.skip = 0;
    this.currentPage = 1;
    // this.getTableData();
    this.pagination.tablePageSize.next({
      skip: this.skip,
      limit: this.limit,
      pageSize: this.pageSize,
    });
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
  ngOnInit() {
    this.state = [
      { name: 'California', code: '1' },
      { name: 'New York', code: '2' },
      { name: 'Texas', code: '3' },
      { name: 'Bavaria', code: '4' },
    ];
    this.country = [
      { name: 'United States', code: '1' },
      { name: 'Germany', code: '2' },
      { name: 'Canada', code: '3' },
      { name: 'Australia', code: '4' },
    ];
    this.cities= [
      { name: 'Los Angeles', code: '1' },
      { name: 'New York City', code: '2' },
      { name: 'Houston', code: '3' },
    ];
    this.status= [
      { name: 'Active', code: '1' },
      { name: 'Inactive', code: '2' },
    ];
   
    
  }
}
