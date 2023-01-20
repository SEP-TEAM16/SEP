import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubscribedServicesPageComponent } from './subscribed-services-page.component';

describe('SubscribedServicesPageComponent', () => {
  let component: SubscribedServicesPageComponent;
  let fixture: ComponentFixture<SubscribedServicesPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SubscribedServicesPageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SubscribedServicesPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
