import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SpikeChartComponent } from './charts/spike-chart/spike-chart.component';
import { FileUploadService } from './data-upload/shared/file-upload.service';
import { FileUploadComponent } from './data-upload/file-upload/file-upload.component';


const routes: Routes = [
  { path: '', redirectTo: "/upload", pathMatch: "full" },
  { path: 'upload', component: FileUploadComponent },
  { path: 'spike-chart', component: SpikeChartComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
