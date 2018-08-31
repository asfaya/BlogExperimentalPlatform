import { Injectable, Inject }                     from "@angular/core";
import { HttpClient, HttpHeaders, HttpResponse }  from "@angular/common/http";
import { Observable }                             from "rxjs";
import { catchError, map }                        from 'rxjs/operators';
import { IBlogEntry }                             from "../interfaces/IBlogEntry";

import { IPagination }                            from "../interfaces/IPagination";
import { PaginatedResult }                        from "../classes/PaginatedResult";

import { BaseDataService }                        from "../classes/BaseDataService";

@Injectable()
export class BlogEntryDataService extends BaseDataService {

  constructor(
    http: HttpClient,
    @Inject('BASE_URL') baseUrl: string) {

    super(http, baseUrl);
  }

  getBlogEntriesPaginated(selectedBlogId: number, page?: number, itemsPerPage?: number): Observable<PaginatedResult<IBlogEntry[]>> {

    var paginatedResult: PaginatedResult<IBlogEntry[]> = new PaginatedResult<IBlogEntry[]>();

    var headers: HttpHeaders;
    if (page != null && itemsPerPage != null) {
      headers = new HttpHeaders().set("Pagination", page.toString() + "," + itemsPerPage.toString());
    } else {
      headers = new HttpHeaders();
    }

    return this.http.get<IBlogEntry[]>(this.baseUrl + "api/blogEntries/GetAllByBlogId/" + selectedBlogId, { headers: headers, observe: "response" })
      .pipe(
        map((res: HttpResponse<IBlogEntry[]>) => {
          paginatedResult.result = res.body;
          if (res.headers.get("Pagination") != null) {
            var paginationHeader: IPagination = <IPagination>JSON.parse(JSON.stringify(res.headers.get("Pagination")));
            paginatedResult.pagination = paginationHeader;
          }
          return paginatedResult;
        }),
        catchError(this.handleError));
  }

  getBlogEntry(id: number): Observable<IBlogEntry> {
    return this.http.get<IBlogEntry>(this.baseUrl + "api/blogEntries/" + id.toString())
      .pipe(
        catchError(this.handleError)
      );
  }
}
