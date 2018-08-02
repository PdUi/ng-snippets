import { Component, Inject } from '@angular/core';

import { USER_TOKEN } from './user.provider';
import { IUser } from './user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  user: IUser;

  constructor(@Inject(USER_TOKEN) user: IUser) {
    this.user = user;
  }
}
