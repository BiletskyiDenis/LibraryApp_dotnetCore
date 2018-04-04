import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { AppPage } from './shared/components/app/app.page';
import { NavMenuComponent } from './shared/components/navmenu/navmenu.component';

import { LibraryService } from './shared/services/library.service';
import { ResourceService } from './shared/services/resource.service';

import { BrowserModule } from '@angular/platform-browser';
import { MainUIModule } from './modules/module.mainUI/mainUI.module';
import { UpsertAssetsModule } from './modules/module.upsertAssets/upsertAssets.module';


@NgModule({
    declarations: [
        AppPage,
        NavMenuComponent,
    ],
    imports: [
        BrowserModule,
        CommonModule,
        MainUIModule,
        UpsertAssetsModule,
        RouterModule.forRoot([
            { path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [LibraryService, ResourceService]
})
export class AppModuleShared {
}
