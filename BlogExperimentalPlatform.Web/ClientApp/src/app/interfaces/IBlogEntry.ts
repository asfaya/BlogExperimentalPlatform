import { IEntity }          from "./IEntity";
import { IBlog }            from "./IBlog";
import { IBlogEntryUpdate } from "./IBlogEntryUpdate";
import { EntryStatus }      from "../classes/EntryStatus";

export interface IBlogEntry extends IEntity {
  title: string;
  content: string;
  blog: IBlog;
  creationDate: Date;
  lastUpdated: Date;
  entryUpdates: IBlogEntryUpdate[];
  status: EntryStatus;
}
