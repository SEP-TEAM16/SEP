import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChoosePaymentTypePageComponent } from './choose-payment-type-page.component';

describe('ChoosePaymentTypePageComponent', () => {
  let component: ChoosePaymentTypePageComponent;
  let fixture: ComponentFixture<ChoosePaymentTypePageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChoosePaymentTypePageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChoosePaymentTypePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
