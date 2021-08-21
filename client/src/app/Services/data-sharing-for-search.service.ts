import { EventEmitter, Injectable, Output } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DataSharingForSearchService {
  // sharingDate = new BehaviorSubject<any>(null);
  // @Output() searchData: EventEmitter<any> = new EventEmitter();
  public static sharedData: string ="";
  constructor() { }
  public static get data(): string {
    return this.sharedData;
  }
  public static set data(value: string) {
    this.sharedData = value;
  }
}
