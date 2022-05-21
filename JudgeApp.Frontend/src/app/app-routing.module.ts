import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LoginPageComponent} from "./components/login-page/login-page.component";
import {RegisterPageComponent} from "./components/register-page/register-page.component";
import {ProjectPageComponent} from "./components/project-page/project-page.component";
import {JudgingPageComponent} from "./components/judging-page/judging-page.component";
import {ResultPageComponent} from "./components/result-page/result-page.component";

const routes: Routes = [
  {path:'login',component:LoginPageComponent},
  {path:'',redirectTo:'/login',pathMatch:'full'},
  {path:'register',component:RegisterPageComponent},
  {path:'project',component:ProjectPageComponent},
  {path:'judging',component:JudgingPageComponent},
  {path:'result',component:ResultPageComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
