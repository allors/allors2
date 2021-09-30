import { Injectable } from '@angular/core';

@Injectable()
export abstract class SaveService {
  abstract errorHandler: (error: any) => void;
}
