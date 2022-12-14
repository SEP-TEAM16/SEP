import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoggedCompanyPageComponent } from './logged-company-page.component';

describe('LoggedCompanyPageComponent', () => {
  let component: LoggedCompanyPageComponent;
  let fixture: ComponentFixture<LoggedCompanyPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LoggedCompanyPageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LoggedCompanyPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
