import { TestBed } from '@angular/core/testing';
import { App } from './app';
import { RouterTestingModule } from '@angular/router/testing';
import { API_BASE_URL } from '@client/shared/util';

describe('App', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      declarations: [App],
      providers: [{ provide: API_BASE_URL, useValue: '/api' }],
    }).compileComponents();
  });

  it('should render the admissions portal heading', () => {
    const fixture = TestBed.createComponent(App);
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('h1')?.textContent).toContain(
      'Don Bosco College Tura'
    );
  });
});
