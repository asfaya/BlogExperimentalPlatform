import { NgModule }             from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { HomeComponent }        from "./home/home.component";

const routes: Routes =
  [
    { path: '', component: HomeComponent },
    { path: 'blogs', loadChildren: './blogs/blog.module#BlogModule' },
    { path: 'blogEntries', loadChildren: './blog-entries/blog-entry.module#BlogEntryModule' },
  ];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: []
})
export class AppRoutingModule { }
