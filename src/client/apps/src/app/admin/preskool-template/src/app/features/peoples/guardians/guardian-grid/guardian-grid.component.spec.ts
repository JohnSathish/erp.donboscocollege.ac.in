import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GuardianGridComponent } from './guardian-grid.component';

describe('GuardianGridComponent', () => {
  let component: GuardianGridComponent;
  let fixture: ComponentFixture<GuardianGridComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [GuardianGridComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GuardianGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
