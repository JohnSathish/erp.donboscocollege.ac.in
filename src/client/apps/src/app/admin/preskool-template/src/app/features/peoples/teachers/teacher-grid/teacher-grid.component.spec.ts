import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeacherGridComponent } from './teacher-grid.component';

describe('TeacherGridComponent', () => {
  let component: TeacherGridComponent;
  let fixture: ComponentFixture<TeacherGridComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TeacherGridComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TeacherGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
