import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StaffsAttendanceComponent } from './staffs-attendance.component';

describe('StaffsAttendanceComponent', () => {
  let component: StaffsAttendanceComponent;
  let fixture: ComponentFixture<StaffsAttendanceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StaffsAttendanceComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StaffsAttendanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
