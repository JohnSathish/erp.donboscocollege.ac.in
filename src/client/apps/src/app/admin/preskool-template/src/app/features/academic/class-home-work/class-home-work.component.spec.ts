import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClassHomeWorkComponent } from './class-home-work.component';

describe('ClassHomeWorkComponent', () => {
  let component: ClassHomeWorkComponent;
  let fixture: ComponentFixture<ClassHomeWorkComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ClassHomeWorkComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ClassHomeWorkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
