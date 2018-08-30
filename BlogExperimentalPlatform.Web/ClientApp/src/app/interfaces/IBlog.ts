import { IEntity }    from "./IEntity";
import { IUser }      from "./IUser";
import { IBlogEntry } from "./IBlogEntry";

export interface IBlog extends IEntity {
  name: string;
  owner: IUser;
  creationDate: Date;
  entries: IBlogEntry[];
}
