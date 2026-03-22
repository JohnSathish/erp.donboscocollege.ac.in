import { Component, OnDestroy, Renderer2 } from '@angular/core';
import { CommonService } from '../../../shared/common/common.service';

@Component({
    selector: 'app-coming-soon',
    templateUrl: './coming-soon.component.html',
    styleUrl: './coming-soon.component.scss',
    imports: []
})
export class ComingSoonComponent implements  OnDestroy {
  public base = '';
  public page = '';
  public last = '';
  days: number = 0;
  hours: number = 0;
  minutes: number = 0;
  seconds: number = 0;
  countdownExpired: boolean = false;
  email: string = ''; // Add this line to declare the 'email' property

  private countdownInterval: any;
  constructor(private common: CommonService, private renderer: Renderer2) {
    this.common.base.subscribe((base: string) => {
      this.base = base;
    });
    this.common.page.subscribe((page: string) => {
      this.page = page;
    });
    this.common.last.subscribe((last: string) => {
      this.last = last;
    });
    if (this.page == 'coming-soon') {
      this.renderer.addClass(document.body, 'bg-light-200');
    }
  }
  
  ngOnInit() {
    this.setCountdown();
  }

  ngOnDestroy() {
    clearInterval(this.countdownInterval);
    this.renderer.removeClass(document.body, 'bg-light-200');
  }

  setCountdown() {
    const countdownDate = new Date('Jul 31, 2024 16:00:00').getTime();

    this.countdownInterval = setInterval(() => {
      const todayDate = new Date().getTime();
      const distance = countdownDate - todayDate;

      this.days = Math.floor(distance / (1000 * 60 * 60 * 24));
      this.hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
      this.minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
      this.seconds = Math.floor((distance % (1000 * 60)) / 1000);

      if (distance < 0) {
        clearInterval(this.countdownInterval);
        this.countdownExpired = true;
      }
    }, 1000);
  }
}
