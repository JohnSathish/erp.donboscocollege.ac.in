import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentAttendanceTypeComponent } from './student-attendance-type.component';

describe('StudentAttendanceTypeComponent', () => {
  let component: StudentAttendanceTypeComponent;
  let fixture: ComponentFixture<StudentAttendanceTypeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StudentAttendanceTypeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StudentAttendanceTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
