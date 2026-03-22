import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ResetPasswordSuccess3Component } from './reset-password-success-3.component';

describe('ResetPasswordSuccess3Component', () => {
  let component: ResetPasswordSuccess3Component;
  let fixture: ComponentFixture<ResetPasswordSuccess3Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ResetPasswordSuccess3Component]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ResetPasswordSuccess3Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
