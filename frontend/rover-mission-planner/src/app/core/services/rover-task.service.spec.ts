import { TestBed } from '@angular/core/testing';

import { RoverTaskService } from './rover-task.service';

describe('RoverTaskService', () => {
  let service: RoverTaskService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RoverTaskService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
