import { IEntity } from "./IEntity";

export interface IBlogEntryUpdate extends IEntity {
  updateMoment: Date;
}
