import { OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';

// https://medium.com/thecodecampus-knowledge/the-easiest-way-to-unsubscribe-from-observables-in-angular-5abde80a5ae3
export class SubscribedBaseComponent implements OnDestroy {
  protected onDestroy$ = new Subject();

  constructor() {}

  ngOnDestroy() {
    this.onDestroy$.next();
  }
}
