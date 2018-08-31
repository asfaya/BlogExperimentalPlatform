import { Component }                      from '@angular/core';
import { Router, ActivatedRoute }         from "@angular/router";
import { FormGroup }                      from '@angular/forms';
import { FormBuilder, Validators }        from '@angular/forms';
import { BlogEntryDataService }           from './blog-entry-data.service';
import { BlogDataService }                from '../blogs/blog-data.service';
import { of as observableOf, Observable } from 'rxjs';
import { map, catchError }                from 'rxjs/operators';

import { BaseComponent }                  from "../classes/BaseComponent";
import { NotificationService }            from "../services/notification.service";
import { AuthenticationService }          from "../services/authentication.service";

import { IBlog }                          from '../Interfaces/IBlog';
import { IBlogEntry }                     from '../Interfaces/IBlogEntry';
import { IUser }                          from '../Interfaces/IUser';
import { EntryStatus }                    from '../classes/EntryStatus';

@Component({
  selector: 'blog-entry-edit',
  templateUrl: './blog-entry-edit.component.html'
})
export class BlogEntryEditComponent extends BaseComponent {

  id            : number;
  loading       : boolean = true;
  blogId        : number;
  blog          : IBlog;
  blogEntry     : IBlogEntry;
  submitEnabled : boolean = true;
  blogEntryForm : FormGroup;
  statuses = EntryStatus;

  constructor(
    notificationService: NotificationService,
    authenticationService: AuthenticationService,
    private dataService: BlogEntryDataService,
    private blogDataService: BlogDataService,
    private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder) {
    super(notificationService, authenticationService)
  }

  ngOnInit(): void {
    if (!this.route.snapshot.params["blogid"]) {
      // No blog Id --> get out of here
      this.back();
    }
    else
      this.blogId = +this.route.snapshot.params["blogid"];

    this.loadBlog(this.blogId)
      .subscribe(() => {
        if (!this.route.snapshot.params["id"])
          this.id = 0;
        else
          this.id = +this.route.snapshot.params["id"];

        this.getBlogEntry(this.id).subscribe(
          (res: IBlogEntry) => {
            this.blogEntry = res;
            this.bindForm();
            this.loading = false;
          },
          error => {
            this.notificationService.printErrorMessage("An error ocurred while setting up the form - " + error);
          }
        );
      },
      (error: any): void => {
        this.checkForErrorsOnRequest(error, "There's been an error loading the blog");
      });
  }

  loadBlog(id: number): Observable<void> {
    return this.blogDataService.getBlog(this.blogId)
      .pipe(
        map((blog: IBlog) => {
          if (!blog) {
            // Id didn't get any blog, get out
            this.back();
          }
          else {
            this.blog = blog;
          }
        }));
  }

  getBlogEntry(id: number): Observable<IBlogEntry> {
    if (id != 0) {
      return this.dataService.getBlogEntry(id);
    } else {
      return observableOf(this.getEmptyBlogEntry());
    }
  }

  getEmptyBlogEntry(): IBlogEntry {
    var emptyBlogEntry: IBlogEntry = {
      id: 0,
      title: "",
      content: "",
      blog: this.blog,
      creationDate: new Date(),
      lastUpdated: new Date(),
      entryUpdates: [],
      status: EntryStatus.Public,
      deleted: false
    }
    return emptyBlogEntry;
  }

  bindForm(): void {
    this.blogEntryForm = this.fb.group({
      id: [this.blogEntry.id, []],
      title: [this.blogEntry.title, [Validators.required]],
      content: [this.blogEntry.content, [Validators.required]],
      blog: this.fb.group({
        id: [this.blogEntry.blog.id, []],
        name: [{ value: this.blogEntry.blog.name, disabled: true }, []],
        owner: this.fb.group({
          id: [this.blogEntry.blog.owner.id, []],
          fullName: [{ value: this.blogEntry.blog.owner.fullName, disabled: true }, []],
          userName: [{ value: this.blogEntry.blog.owner.userName, disabled: true }, []],
        })
      }),
      status: [this.blogEntry.status, [Validators.required]]
    }); 
  }

  saveBlogEntry(value: any, valid: boolean): void {
    if (valid) {
      // Prevent multiple submits
      this.submitEnabled = false;

      // Set the only changed field to the entity
      this.blogEntry.title = value.title;
      this.blogEntry.content = value.content;
      this.blogEntry.status = value.status;

      // Save
      this.dataService.saveBlogEntry(this.blogEntry)
        .subscribe(() => {
            this.notificationService.printSuccessMessage("The blog entry '" + value.title + "' has been correctly saved.");
            this.back();  
        },
        (error: string): any => {
            this.notificationService.printErrorMessage("There's been an error trying to save the blog entry - " + error);
            // Reenable submit if error (go back if succeeds)
            this.submitEnabled = true;
        });
    }
  }

  getCurrentUser(): IUser {
    // So far a dummy
    // Will return hard-coded user until security is created
    var dummyUser: IUser = {
      id: 1,
      fullName: "Andres Faya",
      userName: "asfaya",
      deleted: false
    };

    return dummyUser;
  }

  back(): void {
    this.router.navigate(["/blogEntries"]);
  }
}
