import { Injectable }                       from "@angular/core";
import { Router, ActivatedRouteSnapshot  }  from "@angular/router";
import { CanActivate, RouterStateSnapshot } from "@angular/router";

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {

    constructor(private router: Router) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        if (localStorage.getItem("currentUser")) {
            // logged in so return true
            // This can be extended to check on server on more complex security scenarios
            return true;
        }

        // not logged in so redirect to login page with the return url
        this.router.navigate(["/login"], { queryParams: { returnUrl: state.url }});
        return false;
    }
}