import { NgModule }             from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { BlogEntryListComponent } from './blog-entry-list.component';
import { BlogEntryEditComponent } from "./blog-entry-edit.component";

const routes: Routes = [
  { path: "", component: BlogEntryListComponent },
  { path: "form/:blogid/:id", component: BlogEntryEditComponent },
  { path: "form/:blogid", component: BlogEntryEditComponent }
];

@NgModule({
  exports: [RouterModule],
  imports: [RouterModule.forChild(routes)]
})
export class BlogEntryRoutingModule { }
