import { Component, OnInit, OnDestroy } from "@angular/core";
import { AuthenticationService }        from "../services/authentication.service";
import { IUser }                        from "../interfaces/IUser";
import { ComponentNotifierService }     from "../services/component-notifier.service";
import { Subscription }                 from "rxjs";

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit, OnDestroy {
  isExpanded = false;
  user: IUser;
  subscription: Subscription;

  constructor(
    private authenticationService: AuthenticationService,
    private notifier: ComponentNotifierService
    ) {
      this.subscription = this.notifier.getLoginNotifications()
                            .subscribe(user => { 
                              this.user = this.authenticationService.getCurrentUser(); 
                            });
  } 

  ngOnInit(): void {
    this.user = this.authenticationService.getCurrentUser();
  }

  ngOnDestroy() {
    // unsubscribe to ensure no memory leaks
    this.subscription.unsubscribe();
  }

  collapse(): void {
    this.isExpanded = false;
  }

  toggle(): void {
    this.isExpanded = !this.isExpanded;
  }

  logout(): void {
    this.authenticationService.logout();
  }

  isLogged(): boolean {
    return !(!this.user);
  }

  getUserName(): string {
    if (this.isLogged()) {
      return this.user.userName;
    }
    else {
      return "";
    }
  }
}
