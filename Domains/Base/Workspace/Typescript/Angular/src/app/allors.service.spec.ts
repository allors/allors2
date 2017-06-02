import { TestBed, inject } from '@angular/core/testing';

import { AllorsService } from './allors.service';

describe('AllorsService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AllorsService]
    });
  });

  it('should ...', inject([AllorsService], (service: AllorsService) => {
    expect(service).toBeTruthy();
  }));
});
