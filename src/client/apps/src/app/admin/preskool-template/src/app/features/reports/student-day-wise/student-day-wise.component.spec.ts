import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentDayWiseComponent } from './student-day-wise.component';

describe('StudentDayWiseComponent', () => {
  let component: StudentDayWiseComponent;
  let fixture: ComponentFixture<StudentDayWiseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StudentDayWiseComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StudentDayWiseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
