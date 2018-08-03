import { Component, Inject } from '@angular/core';
import { USER_TOKEN } from './user.provider';
import { IUser } from './user';

@Component({
  selector: 'app-razor',
  template: `<h1>{{user.lastName}}, {{user.firstName}}</h1>`
})
export class RazorComponent {
  user: IUser;

  constructor(@Inject(USER_TOKEN) user: IUser) {
    this.user = user;
  }
}
