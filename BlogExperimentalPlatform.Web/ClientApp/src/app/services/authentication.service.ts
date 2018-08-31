import { Injectable, Inject }       from "@angular/core";
import { HttpClient, HttpHeaders }  from "@angular/common/http";
import { Router }                   from "@angular/router";
import { map, catchError }          from "rxjs/operators";
import { ILogin }                   from "../interfaces/ILogin";
import { IUser }                    from "../interfaces/IUser";
import { BaseDataService }          from "../classes/BaseDataService";
import { ComponentNotifierService } from './component-notifier.service';

@Injectable({ providedIn: 'root' })
export class AuthenticationService extends BaseDataService {

    constructor(
        http: HttpClient,
        @Inject('BASE_URL') baseUrl: string,
        private router: Router,
        private notifier: ComponentNotifierService) { 
            super(http, baseUrl);
        }

    login(username: string, password: string) {
        var login: ILogin = {
            userName: username,
            password: password
        }

        const headers: HttpHeaders = new HttpHeaders().set("Content-Type", "application/json");

        return this.http.post<any>(this.baseUrl + "api/login/", login, { headers: headers })
            .pipe(
                map(user => {
                    // login successful if there's a jwt token in the response
                    if (user && user.token) {
                        // store user details and jwt token in local storage to keep user logged in between page refreshes
                        localStorage.setItem('currentUser', JSON.stringify(user));
                        this.notifier.notifyLogin(user);
                    }

                    return user;
                }),
                catchError(this.handleError));
    }

    logout() {
        // remove user from local storage to log user out
        localStorage.removeItem("currentUser");
        this.notifier.notifyLogin(null);
        this.router.navigate(["/"]);
    }

    isLogged() {
        if (localStorage.getItem("currentUser"))
            return true;
        else
            return false;
    }

    getCurrentUser() {
        if (this.isLogged)
            return <IUser>JSON.parse(localStorage.getItem("currentUser"));
        else
            return null;
    }
}