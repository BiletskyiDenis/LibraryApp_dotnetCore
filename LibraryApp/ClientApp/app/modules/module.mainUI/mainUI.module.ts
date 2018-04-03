import { NgModule } from '@angular/core';

import { RouterModule } from '@angular/router';
import { GridModule } from '@progress/kendo-angular-grid';
import { BrowserModule } from '@angular/platform-browser';
import { HttpModule } from '@angular/http';

import { LibraryService } from '../../shared/services/library.service';
import { ResourceService } from '../../shared/services/resource.service';

import { CatalogPage } from './pages/catalog/catalog.page';
import { HomePage } from './pages/home/home.page';
import { DetailsPage } from './pages/details/details.page';

import { RecentlyAddedComponent } from './components/recently-added/recently-added.component';
import { RecentlyAddedItemComponent } from './components/recently-added-item/recently-added-item.component';
import { DeleteConfirmComponent } from './components/delete-confirm/delete-confirm.component';

@NgModule({
    declarations: [
        CatalogPage,
        HomePage,
        DetailsPage,

        RecentlyAddedComponent,
        RecentlyAddedItemComponent,
        DeleteConfirmComponent
    ],
    imports: [
        BrowserModule,
        GridModule,
        HttpModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomePage },
            { path: 'catalog', component: CatalogPage },
            { path: 'details/:id', component: DetailsPage }
        ])
    ],
    providers: [LibraryService, ResourceService]
})

export class MainUIModule {
}