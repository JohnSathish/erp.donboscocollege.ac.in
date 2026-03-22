import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AcademicSettingsComponent } from './academic-settings.component';

describe('AcademicSettingsComponent', () => {
  let component: AcademicSettingsComponent;
  let fixture: ComponentFixture<AcademicSettingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AcademicSettingsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AcademicSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
