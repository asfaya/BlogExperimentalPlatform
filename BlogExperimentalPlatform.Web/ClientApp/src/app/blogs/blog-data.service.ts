import { Injectable, Inject }             from "@angular/core";
import { HttpClient, HttpHeaders }        from "@angular/common/http";
import { Observable }                     from "rxjs";
import { IBlog }                          from "../interfaces/IBlog";

import { catchError, tap }                from 'rxjs/operators';
import { BaseDataService }                from "../classes/BaseDataService";

@Injectable()
export class BlogDataService extends BaseDataService {

  constructor(
    http: HttpClient,
    @Inject('BASE_URL') baseUrl: string)
  {
    super(http, baseUrl);
  }

  getBlog(id: number): Observable<IBlog> {
    return this.http.get<IBlog>(this.baseUrl + "api/blogs/" + id.toString())
      .pipe(
        catchError(this.handleError)
      );
  }

  getAllBlogs(): Observable<IBlog[]> {
    return this.http.get<IBlog[]>(this.baseUrl + "api/blogs/")
      .pipe(
        catchError(this.handleError)
      );
  }

  saveBlog(blog: IBlog): Observable<IBlog> {
    if (blog.id) {
      return this.updateBlog(blog);
    } else {
      return this.addBlog(blog);
    }
  }

  private updateBlog(blog: IBlog): Observable<IBlog> {
    let headers: HttpHeaders = new HttpHeaders().set("Content-Type", "application/json");

    return this.http.put<IBlog>(this.baseUrl + "api/blogs/" + blog.id, blog, { headers: headers }).pipe(
      catchError(this.handleError));
  }

  private addBlog(blog: IBlog): Observable<IBlog> {
    let headers: HttpHeaders = new HttpHeaders().set("Content-Type", "application/json");

    return this.http.post<IBlog>(this.baseUrl + "api/blogs/", blog, { headers: headers }).pipe(
      catchError(this.handleError));
  }

  deleteBlog(id: number): Observable<any> {
    return this.http.delete(this.baseUrl + "api/blogs/" + id).pipe(
      catchError(this.handleError));
  }
}
