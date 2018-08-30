import { NgModule }             from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { BlogListComponent }    from './blog-list.component';
import { BlogEditComponent }    from "./blog-edit.component";

const routes: Routes = [
  { path: "", component: BlogListComponent },
  { path: "form/:id", component: BlogEditComponent },
  { path: "form", component: BlogEditComponent }
];

@NgModule({
  exports: [RouterModule],
  imports: [RouterModule.forChild(routes)]
})
export class BlogRoutingModule { }
