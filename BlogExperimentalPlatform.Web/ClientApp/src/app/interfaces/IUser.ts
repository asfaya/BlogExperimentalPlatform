import { IEntity } from "./IEntity";

export interface IUser extends IEntity {
  userName: string;
  fullName: string;
}
