import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountsInvoicesComponent } from './accounts-invoices.component';

describe('AccountsInvoicesComponent', () => {
  let component: AccountsInvoicesComponent;
  let fixture: ComponentFixture<AccountsInvoicesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AccountsInvoicesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AccountsInvoicesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
