import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StaffDayWiseComponent } from './staff-day-wise.component';

describe('StaffDayWiseComponent', () => {
  let component: StaffDayWiseComponent;
  let fixture: ComponentFixture<StaffDayWiseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StaffDayWiseComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StaffDayWiseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
