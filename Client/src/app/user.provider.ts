import { InjectionToken } from '@angular/core';

import { IUser } from './user';

const user = (typeof window !== 'undefined') ? (<any> window).user : null;

export const USER_TOKEN = new InjectionToken('user-token', { providedIn: 'root', factory: (): IUser => user});
