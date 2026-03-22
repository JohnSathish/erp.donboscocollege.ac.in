import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentLeavesComponent } from './student-leaves.component';

describe('StudentLeavesComponent', () => {
  let component: StudentLeavesComponent;
  let fixture: ComponentFixture<StudentLeavesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StudentLeavesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StudentLeavesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
