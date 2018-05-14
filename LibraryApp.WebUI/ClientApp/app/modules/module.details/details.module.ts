import { NgModule } from '@angular/core';

import { RouterModule } from '@angular/router';
import { GridModule } from '@progress/kendo-angular-grid';
import { BrowserModule } from '@angular/platform-browser';
import { HttpModule } from '@angular/http';

import { LibraryService } from '../../shared/services/library.service';
import { ResourceService } from '../../shared/services/resource.service';

import { DetailsPage } from './pages/details/details.page';

@NgModule({
    declarations: [
        DetailsPage,
    ],
    imports: [
        BrowserModule,
        GridModule,
        HttpModule,
        RouterModule.forRoot([
            { path: 'details/:id', component: DetailsPage }
        ])
    ],
    providers: [LibraryService, ResourceService]
})

export class DetailsModule {
}