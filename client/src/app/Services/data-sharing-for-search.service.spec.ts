import { TestBed } from '@angular/core/testing';

import { DataSharingForSearchService } from './data-sharing-for-search.service';

describe('DataSharingForSearchService', () => {
  let service: DataSharingForSearchService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DataSharingForSearchService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
