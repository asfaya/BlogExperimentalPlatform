import { CommonModule }                       from '@angular/common';
import { NgModule }                           from '@angular/core';
import { FormsModule, ReactiveFormsModule }   from '@angular/forms';

import { PaginationModule }                   from "../pagination/pagination.module";

import { BlogEntryListComponent }             from './blog-entry-list.component';
import { BlogEntryEditComponent }             from './blog-entry-edit.component';
import { BlogEntryDataService }               from "./blog-entry-data.service";
import { BlogEntryRoutingModule }             from "./blog-entry-routing.module";
import { BlogDataService }                    from '../blogs/blog-data.service';

@NgModule({
  imports: [CommonModule, FormsModule, ReactiveFormsModule, PaginationModule, BlogEntryRoutingModule],
  declarations: [BlogEntryListComponent, BlogEntryEditComponent],
  providers: [BlogEntryDataService, BlogDataService]
})
export class BlogEntryModule { }
