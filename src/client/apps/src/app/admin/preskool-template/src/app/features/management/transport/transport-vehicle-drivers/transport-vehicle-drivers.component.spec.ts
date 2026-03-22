import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TransportVehicleDriversComponent } from './transport-vehicle-drivers.component';

describe('TransportVehicleDriversComponent', () => {
  let component: TransportVehicleDriversComponent;
  let fixture: ComponentFixture<TransportVehicleDriversComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TransportVehicleDriversComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TransportVehicleDriversComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
