import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MeterListComponent } from './components/meter-list/meter-list.component';
import { ReportListComponent } from './components/report-list/report-list.component';
import { HomeComponent } from './components/home/home.component';

const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'meters', component: MeterListComponent },
  { path: 'reports', component: ReportListComponent },
  { path: '', redirectTo: '/home', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
