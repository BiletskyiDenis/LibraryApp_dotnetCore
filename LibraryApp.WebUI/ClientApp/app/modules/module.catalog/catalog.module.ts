import { NgModule } from '@angular/core';

import { RouterModule } from '@angular/router';
import { GridModule } from '@progress/kendo-angular-grid';
import { BrowserModule } from '@angular/platform-browser';
import { HttpModule } from '@angular/http';

import { LibraryService } from '../../shared/services/library.service';
import { ResourceService } from '../../shared/services/resource.service';

import { CatalogPage } from './pages/catalog/catalog.page';
import { DeleteConfirmComponent } from './components/delete-confirm/delete-confirm.component';

@NgModule({
    declarations: [
        CatalogPage,
        DeleteConfirmComponent
    ],
    imports: [
        BrowserModule,
        GridModule,
        HttpModule,
        RouterModule.forRoot([
            { path: 'catalog', component: CatalogPage },
        ])
    ],
    providers: [LibraryService, ResourceService]
})

export class CatalogModule {
}