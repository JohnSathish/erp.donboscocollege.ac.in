import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeachersRoutineComponent } from './teachers-routine.component';

describe('TeachersRoutineComponent', () => {
  let component: TeachersRoutineComponent;
  let fixture: ComponentFixture<TeachersRoutineComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TeachersRoutineComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TeachersRoutineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
