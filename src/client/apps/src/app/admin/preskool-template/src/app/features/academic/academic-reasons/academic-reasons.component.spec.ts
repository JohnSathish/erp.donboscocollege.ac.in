import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AcademicReasonsComponent } from './academic-reasons.component';

describe('AcademicReasonsComponent', () => {
  let component: AcademicReasonsComponent;
  let fixture: ComponentFixture<AcademicReasonsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AcademicReasonsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AcademicReasonsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
