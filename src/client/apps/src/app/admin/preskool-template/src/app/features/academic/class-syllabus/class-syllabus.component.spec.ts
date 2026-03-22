import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClassSyllabusComponent } from './class-syllabus.component';

describe('ClassSyllabusComponent', () => {
  let component: ClassSyllabusComponent;
  let fixture: ComponentFixture<ClassSyllabusComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ClassSyllabusComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ClassSyllabusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
