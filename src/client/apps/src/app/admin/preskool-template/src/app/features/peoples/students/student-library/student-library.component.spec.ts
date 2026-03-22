import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentLibraryComponent } from './student-library.component';

describe('StudentLibraryComponent', () => {
  let component: StudentLibraryComponent;
  let fixture: ComponentFixture<StudentLibraryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StudentLibraryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StudentLibraryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
