import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { AlertService } from '../_services/alert.service';
import { UserService } from '../_services/user.service';
import { AuthenticationService } from '../_services/authentication.service'

import { UserRouting } from './user.routing';

import { AlertComponent } from '../_directives/alert.component'
import { RegisterComponent } from './components/register.component';
import { LoginComponent } from './components/login.component'

@NgModule({
    imports: [ FormsModule, BrowserModule, HttpModule, UserRouting ],
    exports: [],
    declarations: [ AlertComponent, RegisterComponent, LoginComponent ],
    providers: [AlertService, UserService, AuthenticationService]
})
export class UserModule { }
