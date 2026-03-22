import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StaffPayrollComponent } from './staff-payroll.component';

describe('StaffPayrollComponent', () => {
  let component: StaffPayrollComponent;
  let fixture: ComponentFixture<StaffPayrollComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StaffPayrollComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StaffPayrollComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
