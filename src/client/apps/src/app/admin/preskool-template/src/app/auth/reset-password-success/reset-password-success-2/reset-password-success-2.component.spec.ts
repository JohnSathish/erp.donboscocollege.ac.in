import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ResetPasswordSuccess2Component } from './reset-password-success-2.component';

describe('ResetPasswordSuccess2Component', () => {
  let component: ResetPasswordSuccess2Component;
  let fixture: ComponentFixture<ResetPasswordSuccess2Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ResetPasswordSuccess2Component]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ResetPasswordSuccess2Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
