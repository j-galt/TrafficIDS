import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { ChartsModule } from 'ng2-charts';

import { HttpClientModule } from '@angular/common/http';
import { SpikeChartComponent } from './charts/spike-chart/spike-chart.component';
import { ChartService } from './charts/shared/chart-service';

@NgModule({
  declarations: [
    AppComponent,
    SpikeChartComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ChartsModule
  ],
  providers: [
    ChartService
  ],
  bootstrap: [
    AppComponent
  ]
})
export class AppModule { }
