import { Component }              from "@angular/core";
import { Router }                 from "@angular/router";
import { BaseComponent }          from "../classes/BaseComponent";
import { NotificationService }    from "../services/notification.service";

import { BlogDataService }        from "./blog-data.service";

import { IBlog }                  from "../Interfaces/IBlog";
import { AuthenticationService }  from "../services/authentication.service";

@Component({
  selector: 'blog-list',
  templateUrl: './blog-list.component.html'
})
export class BlogListComponent extends BaseComponent {
  blogs: IBlog[];

  constructor(
    notificationService: NotificationService,
    authenticationService: AuthenticationService, 
    private dataService: BlogDataService,
    
    private router: Router, ) {

    super(notificationService, authenticationService);
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
    var user = this.getCurrentUser();
    if (user && user.id == blog.owner.id) {
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
    else {
      this.notificationService.printErrorMessage("You have to be logged in and the owner in order to delete a blog");
    }
  }
}
