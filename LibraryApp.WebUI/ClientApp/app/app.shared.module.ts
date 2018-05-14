import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { AppPage } from './shared/components/app/app.page';
import { NavMenuComponent } from './shared/components/navmenu/navmenu.component';

import { LibraryService } from './shared/services/library.service';
import { ResourceService } from './shared/services/resource.service';

import { BrowserModule } from '@angular/platform-browser';
import { HomeModule } from './modules/module.home/home.module';
import { CatalogModule } from './modules/module.catalog/catalog.module';
import { DetailsModule } from './modules/module.details/details.module';
import { UpsertAssetsModule } from './modules/module.upsertAssets/upsertAssets.module';


@NgModule({
    declarations: [
        AppPage,
        NavMenuComponent,
    ],
    imports: [
        BrowserModule,
        CommonModule,
        HomeModule,
        CatalogModule,
        DetailsModule,
        UpsertAssetsModule,
        RouterModule.forRoot([
            { path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [LibraryService, ResourceService]
})
export class AppModuleShared {
}
