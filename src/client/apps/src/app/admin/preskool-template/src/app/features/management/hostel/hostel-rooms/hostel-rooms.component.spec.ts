import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HostelRoomsComponent } from './hostel-rooms.component';

describe('HostelRoomsComponent', () => {
  let component: HostelRoomsComponent;
  let fixture: ComponentFixture<HostelRoomsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [HostelRoomsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HostelRoomsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
