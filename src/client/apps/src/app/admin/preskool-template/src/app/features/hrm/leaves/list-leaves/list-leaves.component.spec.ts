import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListLeavesComponent } from './list-leaves.component';

describe('ListLeavesComponent', () => {
  let component: ListLeavesComponent;
  let fixture: ComponentFixture<ListLeavesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ListLeavesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListLeavesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
