import { Injectable, Inject }             from "@angular/core";
import { HttpClient, HttpErrorResponse }  from "@angular/common/http";
import { Observable, throwError }         from "rxjs";

@Injectable()
export class BaseDataService {

  constructor(
    protected http: HttpClient,
    @Inject('BASE_URL') protected baseUrl: string) {
  }

  protected handleError(error: HttpErrorResponse): Observable<never> {
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      console.error(
        `Backend returned code ${error.status}, ` +
        `body was: ${error.error}`);
    }
    // return an observable with a user-facing error message
    return throwError(error.error);
  }
}
