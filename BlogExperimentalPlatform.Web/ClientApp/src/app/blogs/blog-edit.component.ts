import { Component }                        from "@angular/core";
import { Router }                           from "@angular/router";
import { ActivatedRoute }                   from "@angular/router";
import { FormGroup }                        from "@angular/forms";
import { FormBuilder, Validators }          from "@angular/forms";
import { BlogDataService }                  from "./blog-data.service";
import { of as observableOf, Observable }   from "rxjs";

import { BaseComponent }                    from "../classes/BaseComponent";
import { NotificationService }              from "../services/notification.service";
import { AuthenticationService }            from "../services/authentication.service";

import { IBlog }                            from "../Interfaces/IBlog";

@Component({
  selector: 'blog-edit',
  templateUrl: './blog-edit.component.html'
})
export class BlogEditComponent extends BaseComponent {

  id                    : number;
  loading               : boolean = true;
  blog                  : IBlog;
  submitEnabled  : boolean = true;

  blogForm: FormGroup;

  constructor(
    private dataService: BlogDataService,
    notificationService: NotificationService,
    authenticationService: AuthenticationService, 
    private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder) {

    super(notificationService, authenticationService);
  }

  ngOnInit(): void {
    if (!this.route.snapshot.params["id"]) 
      this.id = 0;
    else
      this.id = +this.route.snapshot.params["id"];

    this.getBlog(this.id).subscribe(
      (res: IBlog) => {
        this.blog = res;
        this.bindForm();
        this.loading = false;
      },
      error => {
          this.notificationService.printErrorMessage("An error ocurred while setting up the form - " + error);
      }
    );
  }

  getBlog(id: number): Observable<IBlog> {
    if (id != 0) {
      return this.dataService.getBlog(id);
    } else {
      return observableOf(this.getEmptyBlog());
    }
  }

  getEmptyBlog(): IBlog {
    var emptyBlog: IBlog = {
      id: 0,
      name: "",
      owner: this.getCurrentUser(),
      creationDate: new Date(),
      entries: [],
      deleted: false
    }
    return emptyBlog;
  }

  bindForm(): void {
    this.blogForm = this.fb.group({
      id: [this.blog.id, []],
      name: [this.blog.name, [Validators.required]],
      owner: this.fb.group({
        id: [this.blog.owner.id, []],
        fullName: [{ value: this.blog.owner.fullName, disabled: true }, []],
        userName: [{ value: this.blog.owner.userName, disabled: true }, []],
      })
    }); 
  }

  saveBlog(value: any, valid: boolean): void {
    if (valid) {
      // Prevent multiple submits
      this.submitEnabled = false;

      // Set the only changed field to the entity
      this.blog.name = value.name;

      // Save
      this.dataService.saveBlog(this.blog)
        .subscribe(() => {
            this.notificationService.printSuccessMessage("The blog '" + value.name + " has been correctly saved.");
            this.back();  
        },
        (error: string): any => {
            this.notificationService.printErrorMessage("There's been an error trying to save the blog - " + error);
            // Reenable submit if error (go back if succeeds)
            this.submitEnabled = true;
        });
    }
  }

  back(): void {
    this.router.navigate(["/blogs"]);
  }
}
