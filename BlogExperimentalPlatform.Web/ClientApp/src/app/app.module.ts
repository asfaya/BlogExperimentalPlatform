import { BrowserModule }        from "@angular/platform-browser";
import { NgModule }             from "@angular/core";
import { FormsModule }          from "@angular/forms";
import { ReactiveFormsModule }  from "@angular/forms";
import { HttpClientModule }     from "@angular/common/http";
import { HTTP_INTERCEPTORS }    from "@angular/common/http";

import { NotificationService }  from "./services/notification.service";
import { AppComponent }         from "./app.component";
import { NavMenuComponent }     from "./nav-menu/nav-menu.component";
import { HomeComponent }        from "./home/home.component";
import { LoginComponent }       from "./login/login.component";
import { AppRoutingModule }     from "./app-routing.module";
import { JwtInterceptor }       from "./helpers/jwt.interceptor";
import { ErrorInterceptor }     from "./helpers/error.interceptor";

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    NotificationService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
