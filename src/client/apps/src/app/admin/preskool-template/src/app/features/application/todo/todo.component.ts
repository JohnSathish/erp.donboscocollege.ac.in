import { Component, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Editor, NgxEditorModule, Toolbar, Validators } from 'ngx-editor';
import { routes } from '../../../shared/routes/routes';
import { SidebarService } from '../../../shared/sidebar/sidebar.service';
import { CommonService } from '../../../shared/common/common.service';
import { SelectModule } from 'primeng/select';
import { CommonModule } from '@angular/common';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { TooltipContentComponent } from '../../common/tooltip-content/tooltip-content.component';
import { RouterLink } from '@angular/router';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
interface data {
  value: string;
}

@Component({
    selector: 'app-todo',
    templateUrl: './todo.component.html',
    styleUrl: './todo.component.scss',
    imports: [NgxEditorModule,BsDatepickerModule,SelectModule,CommonModule,TooltipContentComponent,RouterLink]
})
export class TodoComponent implements OnInit, OnDestroy {
  value : Date = new Date();
  public routes = routes
  public selectedValue1 = '';
  public selectedValue2 = '';
  public selectedValue3 = '';
  public selectedValue4 = '';
  public selectedValue5 = '';
  public selectedValue6 = '';
  public selectedValue8 = '';
  public selectedValue7 = '';
  public selectedValue9 = '';
  public selectedValue10 = '';
  public selectedValue11 = '';
  public selectedValue12 = '';
  public selectedValue13 = '';
  selectedList1: data[] = [
    { value: 'Bulk Actions' },
    { value: 'Delete Marked' },
    { value: 'Unmark All' },
    { value: 'Mark All' },
  ];
  selectedList2: data[] = [
    { value: 'Recent' },
    { value: 'Last Modified' },
    { value: 'Unmark All' },
    { value: 'Last Modified by me' },
  ];
  selectedList5: data[] = [
    { value: 'Sort by Date' },
    { value: 'Ascending' },
    { value: 'Descending' },
    { value: 'Recently Viewed' },
    { value: 'Recently Added' },
    { value: 'Creation Date ' },
  ];
  selectedList6: data[] = [
    { value: 'Choose' },
    { value: 'Recent1' },
    { value: 'Recent2' },
  ];
  selectedList7: data[] = [
    { value: 'Onhold' },
    { value: 'Onhold' },
    { value: 'Onhold' },
  ];
  selectedList8: data[] = [
    { value: 'High' },
    { value: 'Medium' },
    { value: 'Low' },
  ];
  selectedList9: data[] = [
    { value: 'Select' },
    { value: 'Recent1' },
    { value: 'Recent2' },
  ];
  selectedList10: data[] = [
    { value: 'Choose' },
    { value: 'Recent1' },
    { value: 'Recent2' },
  ];
  selectedList11: data[] = [
    { value: 'Select' },
    { value: 'Recent1' },
    { value: 'Recent2' },
  ];
  selectedList12: data[] = [
    { value: 'Select' },
    { value: 'Recent1' },
    { value: 'Recent2' },
  ];
  selectedList13: data[] = [
    { value: 'Select' },
    { value: 'Recent1' },
    { value: 'Recent2' },
  ];


  bsValue = new Date();
  bsRangeValue: Date[];
  maxDate = new Date();
  constructor(
    private sidebar: SidebarService,
    private common: CommonService,
    private renderer: Renderer2
  ) {
    this.maxDate.setDate(this.maxDate.getDate() + 7);
    this.bsRangeValue = [this.bsValue, this.maxDate];
  }

  editor!: Editor;
  toolbar: Toolbar = [
    ['bold', 'italic'],
    ['underline', 'strike'],
    ['code', 'blockquote'],
    ['ordered_list', 'bullet_list'],
    [{ heading: ['h1', 'h2', 'h3', 'h4', 'h5', 'h6'] }],
    ['link', 'image'],
    ['text_color', 'background_color'],
    ['align_left', 'align_center', 'align_right', 'align_justify'],
  ];

  form = new FormGroup({
    editorContent: new FormControl('', Validators.required()),
  });

  ngOnInit(): void {
    this.editor = new Editor();
  }

  ngOnDestroy(): void {
    this.editor.destroy();
  }
  public appSidebar = true;

  toggleChange() {
    this.appSidebar = !this.appSidebar;
  }
}
