import { TestBed, async, inject } from '@angular/core/testing';
import { HttpModule, Http, Response, ResponseOptions, XHRBackend } from '@angular/http';
import { MockBackend } from '@angular/http/testing';

import { Scope } from '../allors/angular';
import { Organisation } from '../allors/domain';
import { AllorsService } from './allors.service';

describe('VideoService', () => {

  beforeEach(() => {

    TestBed.configureTestingModule({
      imports: [HttpModule],
      providers: [
        AllorsService,
        { provide: XHRBackend, useClass: MockBackend },
      ]
    });
  });

  describe('AllorsService', () => {

    it('should return objects on pull',
      inject([AllorsService, XHRBackend], (allorsService: AllorsService, mockBackend: MockBackend) => {

        const mockResponse = {
          data: [
          ]
        };

        mockBackend.connections.subscribe((connection) => {
          connection.mockRespond(new Response(new ResponseOptions({
            body: JSON.stringify(mockResponse)
          })));
        });

        const scope = new Scope('Organisations', allorsService.database, allorsService.workspace);

        scope.load()
          .subscribe(() => {
            const organisations = scope.collections.organisations as Organisation[];
            expect(organisations.length).toBe(1);
          }, (error) => {
            fail(error);
          });
      }));
  });
});
