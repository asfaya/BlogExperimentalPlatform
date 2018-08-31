import { Injectable }           from "@angular/core";
import { Observable, Subject }  from "rxjs";
import { IUser }                from "../interfaces/IUser";
 
@Injectable({ providedIn: 'root' })
export class ComponentNotifierService {
    private subject = new Subject<any>();
 
    notifyLogin(user: IUser) {
        this.subject.next({ user: user });
    }
    
    getLoginNotifications(): Observable<any> {
        return this.subject.asObservable();
    }
}