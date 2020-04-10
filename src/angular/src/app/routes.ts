import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { AuthGuard } from './_guards/auth.guard';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberDetailResolver } from './_resolver/member-detail.resolver';
import { MemberListResolver } from './_resolver/member-list.resolver';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberEditResolver } from './_resolver/member-edit.resolver';
import { ComplaintManagementComponent } from './complaint/complaint-management/complaint-management.component';
import { ComplaintComponent } from './complaint/complaint.component';

export const appRoutes: Routes = [
  
  { path: '', component: HomeComponent },
  
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {
        path: 'members', component: MemberListComponent,
        resolve: { users: MemberListResolver }
      },
      
      {
        path: 'members/:id', component: MemberDetailComponent,
        resolve: { user: MemberDetailResolver }
      },
      
      {
        path: 'member/edit', component: MemberEditComponent,
        resolve: { user: MemberEditResolver }
      },

      {
        path: 'complaint/edit/:id', component: ComplaintComponent        
      },
      {
        path: 'admin', component: AdminPanelComponent,      
        data: { roles: ['Admin', 'Admin']}
      }
    ]
  },
  // redirect to the full path of the home URL
  { path: '**', redirectTo: '', pathMatch: 'full' }
];

