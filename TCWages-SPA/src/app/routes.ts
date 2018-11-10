import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { AuthGuard } from './_guards/auth.guard';

export const appRoutes: Routes = [
// route any page to home
{ path: '', component: HomeComponent},
// route all child pages to loggedin users
{
  path: '',
  runGuardsAndResolvers: 'always',
  canActivate: [AuthGuard],
  children: [
    { path: 'members', component: MemberListComponent},
    { path: 'messages', component: MessagesComponent},
    { path: 'lists', component: ListsComponent},
  ]
},
// redirect any path or wrong direct url to home
{ path: '**', redirectTo: '', pathMatch: 'full'},
];
