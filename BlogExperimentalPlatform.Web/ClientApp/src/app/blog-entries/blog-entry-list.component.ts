import { Component }                          from '@angular/core';
import { Router }                             from "@angular/router";
import { NotificationService }                from "../services/notification.service";
import { BaseComponent }                      from "../classes/BaseComponent";

import { Observable, throwError }             from 'rxjs';
import { map, catchError }                    from 'rxjs/operators';

import { BlogDataService }                    from '../blogs/blog-data.service';
import { BlogEntryDataService }               from './blog-entry-data.service';

import { IBlog }                              from '../Interfaces/IBlog';
import { IBlogEntry }                         from '../Interfaces/IBlogEntry';
import { PaginatedResult }                    from "../classes/PaginatedResult";

@Component({
  selector: 'blog-entry-list',
  templateUrl: './blog-entry-list.component.html'
})
export class BlogEntryListComponent extends BaseComponent {
  blogs           : IBlog[];
  blogEntries     : IBlogEntry[];
  selectedBlogId  : number;
  loading         : boolean = true;

  public itemsPerPage: number = 10;
  public totalItems: number = 0;
  public currentPage: number = 1;

  constructor(
    private dataService: BlogEntryDataService,
    private blogDataService: BlogDataService,
    notificationService: NotificationService,
    private router: Router, ) {
    super(notificationService);
  }

  ngOnInit() {
    this.loadBlogs()
      .subscribe(() => {
        this.loadBlogEntries()
          .subscribe(() => {
            this.loading = false;
          },
          (error: any): void => {
            this.checkForErrorsOnRequest(error, "There's been an error loading entries list");
          });
      },
      (error: any): void => {
        this.checkForErrorsOnRequest(error, "There's been an error loading blogs list");
      });
  }

  loadBlogs(): Observable<void> {
    return this.blogDataService.getAllBlogs()
      .pipe(
        map((blogArray: IBlog[]) => {
          if (!blogArray || blogArray.length == 0) {
            this.blogs = [];
          }
          else {
            this.blogs = blogArray;
            this.selectedBlogId = blogArray[0].id;
          }
        }));
  }

  loadBlogEntries(): Observable<void> {
    return this.dataService.getBlogEntriesPaginated(this.selectedBlogId)
      .pipe(
        map((blogEntriesPage: PaginatedResult<IBlogEntry[]>) => {
          this.blogEntries = blogEntriesPage.result; // blog entries;
          this.totalItems = blogEntriesPage.pagination.TotalItems;
        }));
  }

  onBlogChanged(blogId: number): void {
    this.selectedBlogId = blogId;
    this.loadBlogEntries()
      .subscribe(() => { },
        (error: any): void => {
          this.checkForErrorsOnRequest(error, "There's been an error loading entries list");
        }
      );
  }

  pageChanged(event: any): void {
    this.currentPage = event.page;
    this.loadBlogEntries().subscribe();
  };

  addEntry(): void {  
    this.router.navigate(["/blogEntries/form", this.selectedBlogId]);
  }

  editEntry(blogEntryId: number): void {
    this.router.navigate(["/blogEntries/form", this.selectedBlogId, blogEntryId]);
  }  
}
