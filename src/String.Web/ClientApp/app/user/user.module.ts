import { NgModule } from '@angular/core';

import { UserRouting } from './user.routing';

import { RegisterComponent } from './components/register.component';
import { LoginComponent } from './components/login.component'

@NgModule({
    imports: [ UserRouting ],
    exports: [],
    declarations: [ RegisterComponent, LoginComponent ],
    providers: [],
})
export class UserModule { }
