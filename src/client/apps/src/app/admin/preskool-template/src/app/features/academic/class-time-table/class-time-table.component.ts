import { Component, Renderer2 } from '@angular/core';
import { routes } from '../../../shared/routes/routes';
import { SelectModule } from 'primeng/select';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CustomPaginationComponent } from '../../../shared/custom-pagination/custom-pagination.component';
import { TooltipContentComponent } from '../../common/tooltip-content/tooltip-content.component';

interface data {
  name: string | undefined;
  code: string | undefined;
}

@Component({
    selector: 'app-class-time-table',
    templateUrl: './class-time-table.component.html',
    styleUrl: './class-time-table.component.scss',
    imports: [SelectModule,RouterLink,FormsModule,CommonModule,TooltipContentComponent]
})
export class ClassTimeTableComponent {
  public routes = routes;
  class: data[] | undefined;
  section: data[] | undefined;
  selectedClass: data | undefined;
  selectedSection: data | undefined;
  selectedClass2: data | undefined;
  selectedSection2: data | undefined;
  formData: { selectedClass: data | undefined }[] = [];
  formData2: { selectedClass: data | undefined }[] = [];
  formData3: { selectedClass: data | undefined }[] = [];
  formData4: { selectedClass: data | undefined }[] = [];
  formData5: { selectedClass: data | undefined }[] = [];
  constructor(
    private renderer: Renderer2,
  ) {

    
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

  addNewRow() {
    this.formData.push({ selectedClass: {name: undefined,code:undefined}});
  }
  addNewRow2() {
    this.formData2.push({ selectedClass: {name: undefined,code:undefined}});
  }
  addNewRow3() {
    this.formData3.push({ selectedClass: {name: undefined,code:undefined}});
  }
  addNewRow4() {
    this.formData4.push({ selectedClass: {name: undefined,code:undefined}});
  }
  addNewRow5() {
    this.formData5.push({ selectedClass: {name: undefined,code:undefined}});
  }

  removeRow(index: number) {
    this.formData.splice(index, 1);
  }
  removeRow2(index: number) {
    this.formData2.splice(index, 1);
  }
  removeRow3(index: number) {
    this.formData3.splice(index, 1);
  }
  removeRow4(index: number) {
    this.formData4.splice(index, 1);
  }
  removeRow5(index: number) {
    this.formData5.splice(index, 1);
  }
  ngOnInit() {

    this.section = [
      { name: 'A', code: '1' },
      { name: 'B', code: '2' },
      { name: 'C', code: '3' },
    ];
    this.class = [
      { name: 'I', code: '1' },
      { name: 'II', code: '2' },
      { name: 'III', code: '3' },
    ];
    this.selectedClass2 = this.class[0]
    this.selectedSection2 = this.section[0]
    this.formData = [{ selectedClass: this.class[0]}];
    this.formData2 = [{ selectedClass: this.class[0]},{ selectedClass: this.class[0]},{ selectedClass: this.class[0]},{ selectedClass: this.class[0]},{ selectedClass: this.class[0]}];
    this.formData3 = [{ selectedClass: this.class[0]},{ selectedClass: this.class[0]},{ selectedClass: this.class[0]},{ selectedClass: this.class[0]},{ selectedClass: this.class[0]}];
    this.formData4 = [{ selectedClass: this.class[0]},{ selectedClass: this.class[0]},{ selectedClass: this.class[0]},{ selectedClass: this.class[0]},{ selectedClass: this.class[0]}];
    this.formData5 = [{ selectedClass: this.class[0]},{ selectedClass: this.class[0]},{ selectedClass: this.class[0]},{ selectedClass: this.class[0]},{ selectedClass: this.class[0]}];
  }
}
