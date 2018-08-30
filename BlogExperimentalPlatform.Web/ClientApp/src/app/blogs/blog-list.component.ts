import { Component }            from '@angular/core';
import { Router }               from "@angular/router";
import { BaseComponent }        from "../classes/BaseComponent";
import { NotificationService }  from "../services/notification.service";

import { BlogDataService }      from './blog-data.service';

import { IBlog }                from '../Interfaces/IBlog';

@Component({
  selector: 'blog-list',
  templateUrl: './blog-list.component.html'
})
export class BlogListComponent extends BaseComponent {
  blogs: IBlog[];

  constructor(
    notificationService: NotificationService,
    private dataService: BlogDataService,
    private router: Router, ) {

    super(notificationService);
  } 

  ngOnInit(): void {
    this.loadBlogs();
  }

  loadBlogs() : void
  {
    this.dataService.getAllBlogs()
      .subscribe((res: IBlog[]) => {
            this.blogs = res; 
        },
        (error: any): void => {
          this.checkForErrorsOnRequest(error, "There's been an error while retrieving the blogs list");
        });
  }

  add(): void {
    this.router.navigate(["/blogs/form"]);
  }

  removeBlog(blog: IBlog): void {
    this.notificationService.openConfirmationDialog("Are you sure you want to delete the blog?",
      () => {
        this.dataService.deleteBlog(blog.id)
          .subscribe(() => {
            this.notificationService.printSuccessMessage(blog.name + " has been deleted.");
            this.loadBlogs();
          },
            (error: any): void => {
              this.checkForErrorsOnRequest(error, "There's been an error while deleting blog '" + blog.name + "'");
            });
      });
  }
}
