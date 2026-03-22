import { Component } from '@angular/core';
import { routes } from '../../../../shared/routes/routes';
import { CommonModule } from '@angular/common';


@Component({
    selector: 'app-ui-cards',
    templateUrl: './ui-cards.component.html',
    styleUrl: './ui-cards.component.scss',
    imports: [CommonModule]
})
export class UiCardsComponent {
  public routes = routes;
  isCardFullscreen = false;

  toggleFullscreen() {
    this.isCardFullscreen = !this.isCardFullscreen;
  }
  isCardVisible = true;

  toggleCardVisibility() {
    this.isCardVisible = !this.isCardVisible;
  }
}
