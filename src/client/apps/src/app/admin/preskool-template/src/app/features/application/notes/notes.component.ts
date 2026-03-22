import { Component } from '@angular/core';
import { Editor, NgxEditorModule, Toolbar, Validators } from 'ngx-editor';
import { FormControl, FormGroup } from '@angular/forms';
import { routes } from '../../../shared/routes/routes';
import { SelectModule } from 'primeng/select';
import { CommonModule } from '@angular/common';
import { TooltipContentComponent } from '../../common/tooltip-content/tooltip-content.component';
import { RouterLink } from '@angular/router';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
@Component({
    selector: 'app-notes',
    templateUrl: './notes.component.html',
    styleUrl: './notes.component.scss',
    imports: [NgxEditorModule,BsDatepickerModule,SelectModule,CommonModule,TooltipContentComponent,RouterLink]
})
export class NotesComponent {
  value : Date = new Date();
public routes = routes
  selectedValue1 = '';
  selectedValue2 = '';
  selectedValue3 = '';
  selectedValue4 = '';
  selectedValue5 = '';
  selectedValue6 = '';
  selectedValue7 = '';
  selectedValue8 = '';
  selectedValue9 = '';
  public appSidebar = true;
  text: string | undefined;
  
  toggleChange() {
    this.appSidebar = !this.appSidebar;
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
}
