import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClassTimeTableComponent } from './class-time-table.component';

describe('ClassTimeTableComponent', () => {
  let component: ClassTimeTableComponent;
  let fixture: ComponentFixture<ClassTimeTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ClassTimeTableComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ClassTimeTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
