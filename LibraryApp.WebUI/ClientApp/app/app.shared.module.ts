import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';

import { LibraryService } from './shared/services/library.service';
import { ResourceService } from './shared/services/resource.service';

import { BrowserModule } from '@angular/platform-browser';
import { MainUIModule } from './modules/module.mainUI/mainUI.module';
import { AddUpdateAssetsModule } from './modules/module.addUpdateAssets/addUpdateAssets.module';


@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
    ],
    imports: [
        BrowserModule,
        CommonModule,
        MainUIModule,
        AddUpdateAssetsModule,
        RouterModule.forRoot([
            { path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [LibraryService, ResourceService]
})
export class AppModuleShared {
}
