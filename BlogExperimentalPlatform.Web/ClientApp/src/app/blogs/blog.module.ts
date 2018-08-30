import { CommonModule }         from '@angular/common';
import { NgModule }             from '@angular/core';
import { ReactiveFormsModule }  from '@angular/forms';

import { BlogListComponent }    from "./blog-list.component";
import { BlogDataService }      from "./blog-data.service";
import { BlogEditComponent }    from './blog-edit.component';
import { BlogRoutingModule }    from "./blog-routing.module";

@NgModule({
  imports: [CommonModule, ReactiveFormsModule, BlogRoutingModule],
  declarations: [BlogListComponent, BlogEditComponent],
  providers: [BlogDataService]
})
export class BlogModule { }
