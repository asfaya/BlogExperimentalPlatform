import { NgModule }               from "@angular/core";
import { RouterModule, Routes }   from "@angular/router";

import { BlogEntryListComponent } from "./blog-entry-list.component";
import { BlogEntryEditComponent } from "./blog-entry-edit.component";

import { AuthGuard }              from "../guards/auth.guard";

const routes: Routes = [
  { path: "", component: BlogEntryListComponent },
  { path: "form/:blogid/:id", component: BlogEntryEditComponent, canActivate: [AuthGuard] },
  { path: "form/:blogid", component: BlogEntryEditComponent, canActivate: [AuthGuard] }
];

@NgModule({
  exports: [RouterModule],
  imports: [RouterModule.forChild(routes)]
})
export class BlogEntryRoutingModule { }
