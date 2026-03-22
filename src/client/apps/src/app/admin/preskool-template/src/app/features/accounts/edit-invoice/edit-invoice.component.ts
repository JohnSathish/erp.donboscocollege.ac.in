import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { Editor, NgxEditorModule, Toolbar, Validators } from 'ngx-editor';
import { routes } from '../../../shared/routes/routes';
import { SelectModule } from 'primeng/select';

@Component({
    selector: 'app-edit-invoice',
    templateUrl: './edit-invoice.component.html',
    styleUrl: './edit-invoice.component.scss',
    imports: [NgxEditorModule,SelectModule,RouterLink]
})
export class EditInvoiceComponent {
  public routes = routes;
  constructor(private router: Router) {}
  editor!: Editor;
  editor2!: Editor;
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
    this.editor2 = new Editor();
  }

  ngOnDestroy(): void {
    this.editor.destroy();
    this.editor2.destroy();
  }
  public onSubmit() {
    // this.router.navigate([routes.invoice]);
  }
}
