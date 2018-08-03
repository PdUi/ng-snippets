import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { RazorComponent } from './razor-data-display.component';
import { TypeaheadComponent } from './typeahead.component';

@NgModule({
  declarations: [
    AppComponent,
    RazorComponent,
    TypeaheadComponent
  ],
  imports: [
    BrowserModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
