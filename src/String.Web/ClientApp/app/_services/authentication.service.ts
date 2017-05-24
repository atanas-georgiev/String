import { Injectable } from '@angular/core';
import { Http, URLSearchParams, Headers, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map'

@Injectable()
export class AuthenticationService {
    constructor(private http: Http) { }

    login(username: string, password: string) {
        let data = new URLSearchParams();
        data.append('username', username);
        data.append('password', password);

        //this.http
        //    .post('/api', data)
        //    .subscribe(data => {
        //        alert('ok');
        //    }, error => {
        //        console.log(error.json());
        //    });
        return this.http.post('/token', data)
            .map((response: Response) => {
                // login successful if there's a jwt token in the response
                console.log(response.json()['access_token']);
                let user = response.json();
                if (user && user.token) {
                    console.log(user);
                    // store user details and jwt token in local storage to keep user logged in between page refreshes
                    //localStorage.setItem('currentUser', JSON.stringify(user));
                }
            });
    }

    logout() {
        // remove user from local storage to log user out
        //localStorage.removeItem('currentUser');
    }
}