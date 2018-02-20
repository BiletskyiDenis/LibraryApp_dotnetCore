import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';

import { LibraryService } from './components/services/library.service';
import { ResourceService } from './components/services/resource.service';

import { RecentlyAddedComponent } from './components/home/recently-added/recently-added.component';
import { RecentlyAddedItemComponent } from './components/home/recently-added-item/recently-added-item.component';

import { ImageSelectorComponent } from './components/image-selector/image-selector.component';
import { AssetBookComponent } from './components/assets/asset-book/asset-book.component';
import { AssetJournalComponent } from './components/assets/asset-journal/asset-journal.component';
import { AssetBroshoreComponent } from './components/assets/asset-broshore/asset-broshore.component';

import { BrowserModule } from '@angular/platform-browser';

import { CatalogComponent } from './components/catalog/catalog.component';
import { DetailsComponent } from './components/details/details.component';
import { DeleteConfirmComponent } from './components/delete-confirm/delete-confirm.component';


@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        RecentlyAddedComponent,
        RecentlyAddedItemComponent,

        ImageSelectorComponent,
        AssetBookComponent,
        AssetJournalComponent,
        AssetBroshoreComponent,
        CatalogComponent,
        DetailsComponent,
        DeleteConfirmComponent
    ],
    imports: [
        BrowserModule,
        CommonModule,
        HttpModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'catalog', component: CatalogComponent },
            { path: 'addbook', component: AssetBookComponent },
            { path: 'addjournal', component: AssetJournalComponent, },
            { path: 'addbroshore', component: AssetBroshoreComponent },
            { path: 'editbook/:id', component: AssetBookComponent },
            { path: 'editjournal/:id', component: AssetJournalComponent },
            { path: 'editbrochure/:id', component: AssetBroshoreComponent },
            { path: 'details/:id', component: DetailsComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [LibraryService, ResourceService]

})
export class AppModuleShared {
}
