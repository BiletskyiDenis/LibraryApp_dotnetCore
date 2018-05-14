import { NgModule } from '@angular/core';

import { RouterModule } from '@angular/router';
import { GridModule } from '@progress/kendo-angular-grid';
import { BrowserModule } from '@angular/platform-browser';
import { HttpModule } from '@angular/http';

import { LibraryService } from '../../shared/services/library.service';
import { ResourceService } from '../../shared/services/resource.service';

import { HomePage } from './pages/home/home.page';

import { RecentlyAddedComponent } from './components/recently-added/recently-added.component';
import { RecentlyAddedItemComponent } from './components/recently-added-item/recently-added-item.component';

@NgModule({
    declarations: [
        HomePage,
        RecentlyAddedComponent,
        RecentlyAddedItemComponent,
    ],
    imports: [
        BrowserModule,
        GridModule,
        HttpModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomePage },
        ])
    ],
    providers: [LibraryService, ResourceService]
})

export class HomeModule {
}