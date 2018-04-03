import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';
import { LibraryService } from '../../../../shared/services/library.service';
import { IDetails } from '../../../../shared/interfaces/IDetails';
import 'rxjs/Rx';

import { style, state, animate, transition, trigger } from '@angular/core';

@Component({
    selector: 'asset-details',
    templateUrl: './details.page.html',
    styleUrls: ['./details.page.css'],
    animations: [
        trigger('fadeInOut', [
            transition(':enter', [
                style({ opacity: 0 }),
                animate(100, style({ opacity: 1 }))
            ]),
            transition(':leave', [
                animate(100, style({ opacity: 0 }))
            ])
        ])
    ]
})


export class DetailsPage implements OnInit {

    private id: number;
    private route$: Subscription;

    isDataLoaded: boolean = false;
    imagesHostPath: string;
    details: IDetails;

    constructor(private libraryService: LibraryService, private route: ActivatedRoute) {
        this.imagesHostPath = this.libraryService.imagesHostPath;
        this.details = {
            author: '', country: '', description: '', frequency: '', genre: '', id: 0, isbn: '',
            imageUrl: '', language: '', numbersOfCopies: 0, pages: 0, price: 0, publisher: '',
            title: '', type: '', year: 0
        };
    }

    ngOnInit(): void {
        this.route$ = this.route.params.subscribe(
            (params: Params) => {
                this.id = +params["id"];
                if (this.id == 0) {
                    return;
                }
                this.libraryService.getDetails(this.id).subscribe(
                    data => {
                        this.details = data;
                        this.isDataLoaded = true;
                    }
                )

            }
        );
    }

    downloadfile(type: string) {
        this.libraryService.downloadFile(this.id, type);
    }

    ngOnDestroy() {
        if (this.route$) this.route$.unsubscribe();
    }

}
