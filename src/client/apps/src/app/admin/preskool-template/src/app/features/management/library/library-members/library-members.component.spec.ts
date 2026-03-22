import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibraryMembersComponent } from './library-members.component';

describe('LibraryMembersComponent', () => {
  let component: LibraryMembersComponent;
  let fixture: ComponentFixture<LibraryMembersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [LibraryMembersComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LibraryMembersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
