import { Component, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, takeUntil } from 'rxjs/operators';
import { random } from 'faker';

import { SubscribedBaseComponent } from './subscribed-base.component';

@Component({
  selector: 'app-typeahead',
  template: `
    <input #input type="text" (keyup)="onKeyup(input.value)" />
    <div class="row">
      <div class="column fill">
        <h1>Real Time Suggestions</h1>
        <div *ngFor="let word of realTimeSuggestions">
          {{word}}
        </div>
      </div>
      <div class="column fill">
        <h1>Delayed Suggestions</h1>
        <div *ngFor="let word of delayedSuggestions">
          {{word}}
        </div>
      </div>
    </div>
  `
})
export class TypeaheadComponent extends SubscribedBaseComponent implements OnDestroy {
  input = new Subject<string>();
  realTimeSuggestions: string[] = [];
  delayedSuggestions: string[] = [];

  constructor() {
    super();

    this.input
        .pipe(takeUntil(this.onDestroy$))
        .subscribe(input => this.realTimeSuggestions = this.getWords(input));

    this.input
        .pipe(takeUntil(this.onDestroy$))
        // doesn't react to every keystroke immediately.
        // delay of 300ms is hardly noticeable to the user, but prevents unnecessary network traffic
        // when the user pauses their keystrokes, then it will request the suggestions
        .pipe(debounceTime(300))
        // removes trailing whitespace from the input
        // used mostly to help with the next piped operator
        .pipe(map((input) => input.trim()))
        // only fires if the input is different from the last time
        .pipe(distinctUntilChanged())
        .subscribe(input => this.delayedSuggestions = this.getWords(input));
  }

  getWords(input: string): string[] {
    return [
      // use Set and spread operator to remove repeats
      ...new Set<string>(random.words(250)
                               // faker returns all words as one space delimited string
                               .split(' ')
                               // get any of the randomly generated words that contain the user's input
                               .filter(word => word.lastIndexOf(input) !== -1)
                        )
    // alphabetize the results
    ].sort((word1, word2) => word1.localeCompare(word2));
  }

  onKeyup(input: string) {
    this.input.next(input);
  }

  ngOnDestroy() {
    super.ngOnDestroy();
  }
}
