// Node modules
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BsDropdownModule, TabsModule, BsDatepickerModule, PaginationModule, ButtonsModule, ModalModule } from 'ngx-bootstrap';
import { RouterModule } from '@angular/router';
import { NgxGalleryModule } from 'ngx-gallery';
import { FileUploadModule } from 'ng2-file-upload';
import { JwtModule } from '@auth0/angular-jwt';
import { TimeAgoPipe } from 'time-ago-pipe';

// Local components
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { AlertifyService } from './_services/alertify.service';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { appRoutes } from './routes';
import { AuthGuard } from './_guards/auth.guard';
import { UserService } from './_services/user.service';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberDetailResolver } from './_resolver/member-detail.resolver';
import { MemberListResolver } from './_resolver/member-list.resolver';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberEditResolver } from './_resolver/member-edit.resolver';
import { PhotoEditorComponent } from './members/photo-editor/photo-editor.component';
import { ListsResolver } from './_resolver/lists.resolver';
import { MemberMessagesComponent } from './members/member-messages/member-messages.component';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { HasRoleDirective } from './_directives/hasRole.directive';
import { UserManagementComponent } from './admin/user-management/user-management.component';
import { AdminService } from './_services/admin.service';
import { RolesModalComponent } from './admin/roles-modal/roles-modal.component';
import { ComplaintComponent } from './complaint/complaint.component';
import { CustomerService } from './_services/customer.service';
import { ComplaintManagementComponent } from './complaint/complaint-management/complaint-management.component';

export function tokenGetter() {
  return localStorage.getItem('token');
}

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    MemberListComponent,
    MessagesComponent,
    MemberDetailComponent,
    MemberEditComponent,
    PhotoEditorComponent,
    TimeAgoPipe,
    MemberMessagesComponent,
    AdminPanelComponent,
    HasRoleDirective,
    UserManagementComponent,
    RolesModalComponent,
    ComplaintComponent,
    ComplaintManagementComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    BsDropdownModule.forRoot(),
    BsDatepickerModule.forRoot(),
    
    PaginationModule.forRoot(),
    
    ButtonsModule.forRoot(),
    
    RouterModule.forRoot(appRoutes),
    
    NgxGalleryModule,
    
    FileUploadModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        whitelistedDomains: ['localhost:5000'],
        blacklistedRoutes: ['localhost:5000/api/auth']
      }
    }),
    // Import the tab module so that we can use the tab controls in our components
    TabsModule.forRoot(),
    // Import the Modals for our modals which we used when editing user roles
    // - https://valor-software.com/ngx-bootstrap/#/modals
    ModalModule.forRoot()
  ],
  // Add the services we have created
  // Add the error interceptors
  // Add router guards
  providers: [
    AuthService,
    ErrorInterceptorProvider,
    AlertifyService,
    AuthGuard,
    UserService,
    MemberDetailResolver,
    MemberListResolver,
    MemberEditResolver,
    ListsResolver,
    AdminService,
    CustomerService
  ],
  // Entry components are any component that Angular loads imperatively/necessarily
  // by time. All the components we have added so far are loaded declaratively and when
  // we use a selector to load that component, then Angular creates the html representation
  // of that component and inserts it into the DOM of the selected element.  But, modals
  // are different since we don't have a selector for the modals that is why we need
  // to add the modals to the entry component.
  // https://valor-software.com/ngx-bootstrap/#/modals#service-component
  entryComponents: [
    RolesModalComponent
  ],
  // When the module is loaded, its gonna bootstrap the AppComponent which is the app.component.ts
  bootstrap: [AppComponent]
})
export class AppModule { }
