import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FeesAssignComponent } from './fees-assign.component';

describe('FeesAssignComponent', () => {
  let component: FeesAssignComponent;
  let fixture: ComponentFixture<FeesAssignComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [FeesAssignComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FeesAssignComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
