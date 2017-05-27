import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { RegisterViewModel } from '../../_models/RegisterViewModel';
import { Http } from '@angular/http';
import { AuthenticationService } from '../../_services/authentication.service'

import { AlertService } from '../../_services/alert.service';
import { UserService } from '../../_services/user.service';

@Component({
    templateUrl: 'register.component.html'
})

export class RegisterComponent {
    model: RegisterViewModel;
    loading = false;

    constructor(
        private http: Http,
        private router: Router,
        private userService: UserService,
        private authenticationService: AuthenticationService,
        private alertService: AlertService) {
        this.model = new RegisterViewModel();
    }

    register() {
        this.loading = true;

        let body = { 'email': this.model.email, 'password': this.model.password, 'verifyPassword': this.model.verifyPassword };

        this.http.post('/Account/Register', JSON.stringify(body), { headers: this.authenticationService.jsonHeaders() })
            .subscribe(response => {
                    if (response.status == 200) {
                        this.router.navigate(['/login']);
                    } else {
                        alert(response.json().error);
                        this.loading = false;
                        console.log(response.json().error);
                    }
                },
                error => {
                    // TODO: parse error messages, generate toast popups
                    // {"Email":["The Email field is required.","The Email field is not a valid e-mail address."],"Password":["The Password field is required.","The Password must be at least 6 characters long."]}
                    this.loading = false;
                    alert(error.text());
                    console.log(error.text());
                });
    }
}