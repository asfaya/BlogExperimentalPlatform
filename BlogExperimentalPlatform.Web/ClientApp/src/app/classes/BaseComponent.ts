import { OnInit }               from "@angular/core";
import { NotificationService }  from "../services/notification.service";

export class BaseComponent implements OnInit {

  constructor(protected notificationService: NotificationService) {
  }

  // events
  ngOnInit(): void {

  }

  protected checkForErrorsOnRequest(error: any, errorMessage: string) {
    // Later - handle 401 / 403 status code when adding security
    this.notificationService.printErrorMessage(errorMessage + ' ' + error);

    // Maybe redirect to error page if 500
  }
}
