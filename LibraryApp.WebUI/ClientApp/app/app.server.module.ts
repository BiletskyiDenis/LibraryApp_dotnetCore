import { NgModule } from '@angular/core';
import { ServerModule } from '@angular/platform-server';
import { AppModuleShared } from './app.shared.module';
import { AppPage } from './shared/components/app/app.page';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
    bootstrap: [AppPage ],
    imports: [
        ServerModule,
        AppModuleShared,
        NoopAnimationsModule
    ]
})
export class AppModule {
}
