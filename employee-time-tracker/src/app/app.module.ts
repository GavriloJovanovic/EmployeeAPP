import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MainPageComponent } from './main-page/main-page.component';
import { TimeTrackerService } from './services/time-tracker.service';
import { HttpClientModule } from '@angular/common/http';
import { MyTableComponent } from './main-page/my-table/my-table.component';

@NgModule({
  declarations: [
    AppComponent,
    MainPageComponent,
    MyTableComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [TimeTrackerService],
  bootstrap: [AppComponent]
})
export class AppModule { }
