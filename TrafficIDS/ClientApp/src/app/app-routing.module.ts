import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SpikeChartComponent } from './charts/spike-chart/spike-chart.component';


const routes: Routes = [];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
