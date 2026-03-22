import { Component, Renderer2 } from '@angular/core';
import { routes } from '../../../shared/routes/routes';
import { apiResultFormat, pageSelection, accountInvoiceModal } from '../../../shared/model/pages.model';
import { MatTableDataSource } from '@angular/material/table';
import { DataService } from '../../../shared/data/data.service';
import { PaginationService, tablePageSize } from '../../../shared/custom-pagination/pagination.service';
import { Router, RouterLink } from '@angular/router';
import { MatSortModule, Sort } from '@angular/material/sort';
import { SelectModule } from 'primeng/select';
import { CustomPaginationComponent } from '../../../shared/custom-pagination/custom-pagination.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DateRangePickerComponent } from '../../common/date-range-picker/date-range-picker.component';
import { TooltipContentComponent } from '../../common/tooltip-content/tooltip-content.component';
interface data {
  name: string;
  code: string;
}

@Component({
    selector: 'app-accounts-invoices',
    templateUrl: './accounts-invoices.component.html',
    styleUrl: './accounts-invoices.component.scss',
    imports: [RouterLink,SelectModule,CustomPaginationComponent,CommonModule,FormsModule,MatSortModule,DateRangePickerComponent,TooltipContentComponent]
})
export class AccountsInvoicesComponent {
  public routes = routes;
  invoiceNo: data[] | undefined;
  selectedinvoiceNo: data | undefined;
  sourceName: data[] | undefined;
  selectedsourceName: data | undefined;
  incomeName: data[] | undefined;
  selectedincome: data | undefined;
  invoice_Date: data[] | undefined;
  selectedDate: data | undefined;
  paymentMethod: data[] | undefined;
  selectedMethod: data | undefined;
  selectedMethod1: data | undefined;
  bsValue = new Date();
  bsRangeValue: Date[];
  maxDate = new Date();
  initChecked = false;
  public pageSize = 10;
  public tableData: Array<accountInvoiceModal> = [];
  public tableDataCopy: Array<accountInvoiceModal> = [];
  public actualData: Array<accountInvoiceModal> = [];
  public currentPage = 1;
  // pagination variables
  public skip = 0;
  public limit: number = this.pageSize;

  public serialNumberArray: Array<number> = [];
  public totalData = 0;
  showFilter = false;
  public pageSelection: pageSelection[] = [];
  dataSource!: MatTableDataSource<accountInvoiceModal>;
  public searchDataValue = '';
  constructor(
    private renderer: Renderer2,
    private data: DataService,
    private pagination: PaginationService,
    private router: Router
  ) {
    this.maxDate.setDate(this.maxDate.getDate() + 7);
    this.bsRangeValue = [this.bsValue, this.maxDate];
    this.data.getInvoiceData().subscribe((apiRes: apiResultFormat) => {
      this.actualData = apiRes.data;
      this.pagination.tablePageSize.subscribe((res: tablePageSize) => {
        if (this.router.url == this.routes.accountsInvoices) {
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

    this.incomeName = [
      { name: 'April Month Fees', code: '1' },
      { name: 'STEM Program Grant', code: '2' },
      { name: 'Alumni Scholarship', code: '3' },
    ];
    this.sourceName = [
      { name: 'Tuition Fees', code: '1' },
      { name: 'Government Grants', code: '2' },
      { name: 'Donations', code: '3' },
    ];
    this.invoiceNo = [
      { name: 'INV681537', code: '1' },
      { name: 'INV681536', code: '2' },
      { name: 'INV681535', code: '3' },
    ];
    this.invoice_Date = [
      { name: '25 Apr 2024', code: '1' },
      { name: '27 Apr 2024', code: '2' },
      { name: '03 May 2024', code: '2' },
    ];
    this.paymentMethod = [
      { name: 'Cash', code: '1' },
      { name: 'Credit', code: '2' },
    ];
    this.selectedMethod1 = this.paymentMethod[1];
  }
  private getTableData(pageOption: pageSelection): void {
    this.data.getInvoiceData().subscribe((apiRes: apiResultFormat) => {
      this.tableData = [];
      this.tableDataCopy = [];
      this.serialNumberArray = [];
      this.totalData = apiRes.totalData;
      apiRes.data.map((res: accountInvoiceModal, index: number) => {
        const serialNumber = index + 1;
        if (index >= pageOption.skip && serialNumber <= pageOption.limit) {
          res.key = serialNumber;
          this.tableData.push(res);
          this.tableDataCopy.push(res);
          this.serialNumberArray.push(serialNumber);
        }
      });
      this.dataSource = new MatTableDataSource<accountInvoiceModal>(this.actualData);
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
