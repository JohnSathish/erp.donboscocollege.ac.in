import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ForgetPassword2Component } from './forget-password2.component';

describe('ForgetPassword2Component', () => {
  let component: ForgetPassword2Component;
  let fixture: ComponentFixture<ForgetPassword2Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ForgetPassword2Component]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ForgetPassword2Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
