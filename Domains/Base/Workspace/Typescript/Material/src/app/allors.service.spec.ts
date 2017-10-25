import { async, inject, TestBed } from "@angular/core/testing";
import { Http, HttpModule, Response, ResponseOptions, XHRBackend } from "@angular/http";
import { MockBackend } from "@angular/http/testing";

import { Scope } from "@allors";
import { Organisation } from "@allors";
import { AllorsService } from "@allors";

describe("VideoService", () => {

  beforeEach(() => {

    TestBed.configureTestingModule({
      imports: [HttpModule],
      providers: [
        AllorsService,
        { provide: XHRBackend, useClass: MockBackend },
      ],
    });
  });

  describe("AllorsService", () => {

    it("should return objects on pull",
      inject([AllorsService, XHRBackend], (allorsService: AllorsService, mockBackend: MockBackend) => {

        const mockResponse = {
          data: [
          ],
        };

        mockBackend.connections.subscribe((connection) => {
          connection.mockRespond(new Response(new ResponseOptions({
            body: JSON.stringify(mockResponse),
          })));
        });

        const scope = new Scope(allorsService.database, allorsService.workspace);

        scope.load("Organisations")
          .subscribe((loaded) => {
            const organisations = loaded.collections.organisations as Organisation[];
            expect(organisations.length).toBe(1);
          }, (error) => {
            fail(error);
          });
      }));
  });
});
