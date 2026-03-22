import { ChangeDetectionStrategy, Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';


@Component({
    selector: 'app-ui-stickynote',
    templateUrl: './ui-stickynote.component.html',
    styleUrl: './ui-stickynote.component.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: []
})
export class UiStickynoteComponent {
  notes = [];
  public routes = routes;
}
