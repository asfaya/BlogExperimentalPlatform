import { NgModule }             from "@angular/core";

import { EnumKeyValueListPipe } from "../pipes/enum-list.pipe";

@NgModule({
    imports: [],
    declarations: [EnumKeyValueListPipe],
    providers: [],
    exports: [EnumKeyValueListPipe]
})
export class BlogCommonModule { }