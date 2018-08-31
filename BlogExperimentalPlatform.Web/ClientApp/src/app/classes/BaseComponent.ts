import { OnInit }                 from "@angular/core";
import { NotificationService }    from "../services/notification.service";
import { AuthenticationService }  from "../services/authentication.service";
import { IUser }                  from "../interfaces/IUser";

export class BaseComponent implements OnInit {

  constructor(
    protected notificationService: NotificationService,
    protected authenticationService: AuthenticationService,) {
  }

  // events
  ngOnInit(): void {

  }

  protected checkForErrorsOnRequest(error: any, errorMessage: string) {
    this.notificationService.printErrorMessage(errorMessage + ' ' + error);

    // Maybe redirect to error page if 500
  }

  protected isLogged(): boolean {
    return this.authenticationService.isLogged();
  }

  protected getUserName(): string {
    var user = this.authenticationService.getCurrentUser();

    if (user) {
      return user.userName;
    }
    else {
      return "";
    }
  }

  protected getCurrentUser(): IUser {
    return this.authenticationService.getCurrentUser();
  }
}
