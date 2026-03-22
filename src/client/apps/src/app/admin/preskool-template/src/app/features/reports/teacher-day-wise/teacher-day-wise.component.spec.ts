import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeacherDayWiseComponent } from './teacher-day-wise.component';

describe('TeacherDayWiseComponent', () => {
  let component: TeacherDayWiseComponent;
  let fixture: ComponentFixture<TeacherDayWiseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TeacherDayWiseComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TeacherDayWiseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
