import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TransportAssignVehicleComponent } from './transport-assign-vehicle.component';

describe('TransportAssignVehicleComponent', () => {
  let component: TransportAssignVehicleComponent;
  let fixture: ComponentFixture<TransportAssignVehicleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TransportAssignVehicleComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TransportAssignVehicleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
