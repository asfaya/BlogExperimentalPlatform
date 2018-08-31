import { Component, ElementRef, EventEmitter, Input, OnInit, Output, Renderer, Self } from "@angular/core";
import { ControlValueAccessor, NgModel }                                              from "@angular/forms";

import { IKeyAttribute }                                                              from "./common";

// todo: extract base functionality classes
// todo: expose an option to change default configuration
export interface IPaginationConfig extends IKeyAttribute {
    maxSize: number;
    itemsPerPage: number;
    // is navigation buttons visible
    boundaryLinks: boolean;
    directionLinks: boolean;
    // labels
    firstText: string;
    previousText: string;
    nextText: string;
    lastText: string;

    rotate: boolean;
}
export interface IPageChangedEvent {
    itemsPerPage: number;
    page: number;
}

const paginationConfig: IPaginationConfig = {
    maxSize: void 0,
    itemsPerPage: 10,
    boundaryLinks: false,
    directionLinks: true,
    firstText: "First",
    previousText: "Previous",
    nextText: "Next",
    lastText: "Last",
    rotate: true
};

const PAGINATION_TEMPLATE: string = `
  <ul class="pagination" [ngClass]="classMap">
    <li class="pagination-first page-item"
        *ngIf="boundaryLinks"
        [class.disabled]="noPrevious()||disabled">
      <a class="page-link" href (click)="selectPage(1, $event)" [innerHTML]="getText('first')"></a>
    </li>
    <li class="pagination-prev page-item"
        *ngIf="directionLinks"
        [class.disabled]="noPrevious()||disabled">
      <a class="page-link" href (click)="selectPage(page - 1, $event)" [innerHTML]="getText('previous')"></a>
      </li>
    <li *ngFor="let pg of pages"
        [class.active]="pg.active"
        [class.disabled]="disabled&&!pg.active"
        class="pagination-page page-item">
      <a class="page-link" href (click)="selectPage(pg.number, $event)" [innerHTML]="pg.text"></a>
    </li>
    <li class="pagination-next page-item"
        *ngIf="directionLinks"
        [class.disabled]="noNext()">
      <a class="page-link" href (click)="selectPage(page + 1, $event)" [innerHTML]="getText('next')"></a></li>
    <li class="pagination-last page-item"
        *ngIf="boundaryLinks"
        [class.disabled]="noNext()">
      <a class="page-link" href (click)="selectPage(totalPages, $event)" [innerHTML]="getText('last')"></a></li>
  </ul>
  `;

/* tslint:disable */
@Component({
    selector: "pagination[ngModel]",
    template: PAGINATION_TEMPLATE,
})
/* tslint:enable */
export class PaginationComponent implements ControlValueAccessor, OnInit, IPaginationConfig, IKeyAttribute {
    public config: any;
    @Input() public align: boolean;
    @Input() public maxSize: number;

    @Input() public boundaryLinks: boolean;
    @Input() public directionLinks: boolean;
    // labels
    @Input() public firstText: string;
    @Input() public previousText: string;
    @Input() public nextText: string;
    @Input() public lastText: string;
    @Input() public rotate: boolean;

    @Input() public disabled: boolean;

    @Output() public numPages: EventEmitter<number> = new EventEmitter<number>(false);
    @Output() public pageChanged: EventEmitter<IPageChangedEvent> = new EventEmitter<IPageChangedEvent>(false);

    @Input()
    public get itemsPerPage(): number {
        return this._itemsPerPage;
    }

    public set itemsPerPage(v: number) {
        this._itemsPerPage = v;
        this.totalPages = this.calculateTotalPages();
    }

    @Input()
    public get totalItems(): number {
        return this._totalItems;
    }

    public set totalItems(v: number) {
        this._totalItems = v;
        this.totalPages = this.calculateTotalPages();
    }

    public get totalPages(): number {
        return this._totalPages;
    }

    public set totalPages(v: number) {
        this._totalPages = v;
        this.numPages.emit(v);
        if (this.inited) {
            this.selectPage(this.page);
        }
    }

    public set page(value: number) {
        const _previous: number = this._page;
        this._page = (value > this.totalPages) ? this.totalPages : (value || 1);

        if (_previous === this._page || typeof _previous === "undefined") {
            return;
        }

        this.pageChanged.emit({
            page: this._page,
            itemsPerPage: this.itemsPerPage
        });
    }

    public get page(): number {
        return this._page;
    }

    public onChange: any = Function.prototype;
    public onTouched: any = Function.prototype;

    public cd: NgModel;
    public renderer: Renderer;
    public elementRef: ElementRef;

    private classMap: string;

    private _itemsPerPage: number;
    private _totalItems: number;
    private _totalPages: number;
    private inited: boolean = false;
    // ??
    private _page: number;
    private pages: Array<any>;

    public constructor( @Self() cd: NgModel, renderer: Renderer, elementRef: ElementRef) {
        this.cd = cd;
        this.renderer = renderer;
        this.elementRef = elementRef;
        cd.valueAccessor = this;
        this.config = this.config || paginationConfig;
    }

    public ngOnInit(): void {
        this.classMap = this.elementRef.nativeElement.getAttribute("class") || "";
        // watch for maxSize
        this.maxSize = typeof this.maxSize !== "undefined"
            ? this.maxSize
            : paginationConfig.maxSize;
        this.rotate = typeof this.rotate !== "undefined"
            ? this.rotate
            : paginationConfig.rotate;
        this.boundaryLinks = typeof this.boundaryLinks !== "undefined"
            ? this.boundaryLinks
            : paginationConfig.boundaryLinks;
        this.directionLinks = typeof this.directionLinks !== "undefined"
            ? this.directionLinks
            : paginationConfig.directionLinks;

        // base class
        this.itemsPerPage = typeof this.itemsPerPage !== "undefined"
            ? this.itemsPerPage
            : paginationConfig.itemsPerPage;
        this.totalPages = this.calculateTotalPages();
        // this class
        this.pages = this.getPages(this.page, this.totalPages);
        this.page = this.cd.value;
        this.inited = true;
    }

    public writeValue(value: number): void {
        this.page = value;
        this.pages = this.getPages(this.page, this.totalPages);
    }

    public getText(key: string): string {
        return (this as IKeyAttribute)[key + "Text"] || paginationConfig[key + "Text"];
    }

    public noPrevious(): boolean {
        return this.page === 1;
    }

    public noNext(): boolean {
        return this.page === this.totalPages;
    }

    public registerOnChange(fn: (_: any) => {}): void {
        this.onChange = fn;
    }

    public registerOnTouched(fn: () => {}): void {
        this.onTouched = fn;
    }

    private selectPage(page: number, event?: MouseEvent): void {
        if (event) {
            event.preventDefault();
        }

        if (!this.disabled) {
            if (event && event.target) {
                let target: any = event.target;
                target.blur();
            }
            this.writeValue(page);
            this.cd.viewToModelUpdate(this.page);
        }
    }

    // create page object used in template
    private makePage(num: number, text: string, isActive: boolean): { number: number, text: string, active: boolean } {
        return {
            number: num,
            text: text,
            active: isActive
        };
    }

    private getPages(currentPage: number, totalPages: number): Array<any> {
        let pages: any[] = [];

        // default page limits
        let startPage: number = 1;
        let endPage: number = totalPages;
        let isMaxSized: boolean = typeof this.maxSize !== "undefined" && this.maxSize < totalPages;

        // recompute if maxSize
        if (isMaxSized) {
            if (this.rotate) {
                // current page is displayed in the middle of the visible ones
                startPage = Math.max(currentPage - Math.floor(this.maxSize / 2), 1);
                endPage = startPage + this.maxSize - 1;

                // adjust if limit is exceeded
                if (endPage > totalPages) {
                    endPage = totalPages;
                    startPage = endPage - this.maxSize + 1;
                }
            } else {
                // visible pages are paginated with maxSize
                startPage = ((Math.ceil(currentPage / this.maxSize) - 1) * this.maxSize) + 1;

                // adjust last page if limit is exceeded
                endPage = Math.min(startPage + this.maxSize - 1, totalPages);
            }
        }

        // add page number links
        for (let num: number = startPage; num <= endPage; num++) {
            let page: any = this.makePage(num, num.toString(), num === currentPage);
            pages.push(page);
        }

        // add links to move between page sets
        if (isMaxSized && !this.rotate) {
            if (startPage > 1) {
                let previousPageSet: any = this.makePage(startPage - 1, "...", false);
                pages.unshift(previousPageSet);
            }

            if (endPage < totalPages) {
                let nextPageSet: any = this.makePage(endPage + 1, "...", false);
                pages.push(nextPageSet);
            }
        }

        return pages;
    }

    // base class
    private calculateTotalPages(): number {
        let totalPages: number = this.itemsPerPage < 1
            ? 1
            : Math.ceil(this.totalItems / this.itemsPerPage);
        return Math.max(totalPages || 0, 1);
    }
}
