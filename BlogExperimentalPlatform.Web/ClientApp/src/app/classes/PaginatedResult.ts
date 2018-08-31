import { IPagination } from "../interfaces/IPagination";

export class PaginatedResult<T> {
  result: T;
  pagination: IPagination;
}
