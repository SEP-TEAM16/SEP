import { TestBed } from '@angular/core/testing';

import { SubscribeOptionsService } from './subscribe-options.service';

describe('SubscribeOptionsService', () => {
  let service: SubscribeOptionsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SubscribeOptionsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
