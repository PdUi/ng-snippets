import { Component } from '@angular/core';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, map } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  input = new Subject<string>();
  realTimeUpdate: string[] = [];
  delayUpdate: string[] = [];

  constructor() {
    this.input
        .subscribe(input => this.realTimeUpdate.push(input));

    this.input
        .pipe(debounceTime(300))
        .pipe(map((input) => input.trim()))
        .pipe(distinctUntilChanged())
        .subscribe(input => this.delayUpdate.push(input));
  }

  onKeyup(input: string) {
    this.input.next(input);
  }
}
