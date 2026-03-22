import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TransportPickupPointsComponent } from './transport-pickup-points.component';

describe('TransportPickupPointsComponent', () => {
  let component: TransportPickupPointsComponent;
  let fixture: ComponentFixture<TransportPickupPointsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TransportPickupPointsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TransportPickupPointsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
