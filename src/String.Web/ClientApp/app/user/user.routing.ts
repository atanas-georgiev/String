import { Routes, RouterModule } from '@angular/router';

import { RegisterComponent } from './components/register.component';
import { LoginComponent } from './components/login.component';

const userRoutes: Routes = [
    {
        path: 'user',
        children: [
            { path: 'register', component: RegisterComponent },
            { path: 'login', component: LoginComponent }
        ]
    }
];

export const UserRouting = RouterModule.forChild(userRoutes);