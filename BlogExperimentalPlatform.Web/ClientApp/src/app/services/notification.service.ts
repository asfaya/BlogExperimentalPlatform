import { Injectable } from "@angular/core";

declare let alertify: any;

@Injectable()
export class NotificationService {
  // private _notifier = AlertifyJS;
  private _notifier: any = alertify;

  /*
  opens a confirmation dialog using the alertify.js lib
  */
  openConfirmationDialog(message: string, okCallback: () => any): void {
    this._notifier.confirm(message, function (e: any): void {
      if (e) {
        okCallback();
      }
    });
  }

  /*
  prints a success message using the alertify.js lib
  */
  printSuccessMessage(message: string): void {

    var msg = this._notifier.success(message, 5);
  }

  /*
  prints an error message using the alertify.js lib
  */
  printErrorMessage(message: string): void {
    var msg = this._notifier.error(message, 5);
  }
}
