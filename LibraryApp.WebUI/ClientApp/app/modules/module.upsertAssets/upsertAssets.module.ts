import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { GridModule } from '@progress/kendo-angular-grid';
import { BrowserModule } from '@angular/platform-browser';

import { LibraryService } from '../../shared/services/library.service';
import { ResourceService } from '../../shared/services/resource.service';
import { AssetBookPage } from './pages/asset-book/asset-book.page';
import { AssetJournalPage } from './pages/asset-journal/asset-journal.page';
import { AssetBroshorePage } from './pages/asset-broshore/asset-broshore.page';
import { ImageSelectorComponent } from './components/image-selector/image-selector.component';

@NgModule({
    declarations: [
        AssetBookPage,
        AssetJournalPage,
        AssetBroshorePage,

        ImageSelectorComponent
    ],
    imports: [
        BrowserModule,
        HttpModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule.forRoot([
            { path: 'addbook', component: AssetBookPage },
            { path: 'addjournal', component: AssetJournalPage, },
            { path: 'addbroshore', component: AssetBroshorePage },
            { path: 'editbook/:id', component: AssetBookPage },
            { path: 'editjournal/:id', component: AssetJournalPage },
            { path: 'editbrochure/:id', component: AssetBroshorePage },
        ])
    ],
    providers: [LibraryService, ResourceService]
})

export class UpsertAssetsModule {
}