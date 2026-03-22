import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeacherLeavesComponent } from './teacher-leaves.component';

describe('TeacherLeavesComponent', () => {
  let component: TeacherLeavesComponent;
  let fixture: ComponentFixture<TeacherLeavesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TeacherLeavesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TeacherLeavesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
