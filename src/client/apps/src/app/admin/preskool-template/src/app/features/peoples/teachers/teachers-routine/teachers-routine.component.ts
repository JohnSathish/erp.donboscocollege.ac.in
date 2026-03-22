import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-teachers-routine',
    templateUrl: './teachers-routine.component.html',
    styleUrl: './teachers-routine.component.scss',
    imports: [RouterLink]
})
export class TeachersRoutineComponent {
public routes = routes
}
