import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Http } from '@angular/http';

import { LoginViewModel } from '../../_models/LoginViewModel';

import { AlertService } from '../../_services/alert.service';
import { AuthenticationService } from '../../_services/authentication.service'

@Component({
    templateUrl: 'login.component.html'
})

export class LoginComponent implements OnInit {
    model: LoginViewModel;
    loading = false;
    returnUrl: string;

    constructor(
        private http: Http,
        private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.model = new LoginViewModel();

        // reset login status
        this.authenticationService.logout();

        // get return url from route parameters or default to '/'
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    }

    // post the user's login details to server, if authenticated token is returned, then token is saved to session storage
    login(): void {
        this.loading = true;
        //event.preventDefault();
        let body = 'username=' + this.model.email + '&password=' + this.model.password + '&grant_type=password';

        this.http.post('/connect/token', body, { headers: this.authenticationService.contentHeaders() })
            .subscribe(response => {
                    // success, save the token to session storage
                this.authenticationService.login(response.json());
                    this.router.navigate(['/about']);
                },
                error => {
                    this.alertService.error(error.text());
                    console.log(error.text());
                    this.loading = false;
                }
            );
    }
}