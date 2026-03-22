import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';
import { CustomPaginationComponent } from '../../../../shared/custom-pagination/custom-pagination.component';
import { FormsModule } from '@angular/forms';
import { MatSortModule } from '@angular/material/sort';
import { DateRangePickerComponent } from '../../../common/date-range-picker/date-range-picker.component';
import { TooltipContentComponent } from '../../../common/tooltip-content/tooltip-content.component';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { SelectModule } from 'primeng/select';
import { Editor, NgxEditorModule, Toolbar } from 'ngx-editor';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';

@Component({
    selector: 'app-all-blog',
    templateUrl: './all-blog.component.html',
    styleUrl: './all-blog.component.scss',
    imports: [FormsModule,MatSortModule,TooltipContentComponent,RouterLink,CommonModule,SelectModule,NgxEditorModule,BsDatepickerModule]
})
export class AllBlogComponent {
  public routes = routes;
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

  ngOnInit(): void {
    this.editor = new Editor();
  }

  ngOnDestroy(): void {
    this.editor.destroy();
  }
}
