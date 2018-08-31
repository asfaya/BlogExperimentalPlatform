import { NgModule }             from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { BlogListComponent }    from './blog-list.component';
import { BlogEditComponent }    from "./blog-edit.component";
import { AuthGuard }            from "../guards/auth.guard";

const routes: Routes = [
  { path: "", component: BlogListComponent },
  { path: "form/:id", component: BlogEditComponent, canActivate: [AuthGuard] },
  { path: "form", component: BlogEditComponent, canActivate: [AuthGuard] }
];

@NgModule({
  exports: [RouterModule],
  imports: [RouterModule.forChild(routes)]
})
export class BlogRoutingModule { }
