import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';
import { LibraryService } from '../services/library.service';
import { IDetails } from '../shared/interfaces/IDetails';
import 'rxjs/Rx';

import { style, state, animate, transition, trigger } from '@angular/core';

@Component({
  selector: 'details',
  templateUrl: './details.component.html',
  styleUrls: ['./details.component.css'],
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


export class DetailsComponent implements OnInit {

  private id: number;
  private route$: Subscription;
  isDataLoaded: boolean = false;

  imageNoneUrl:string;

  details: IDetails = {
    Author: '', Country: '', Description: '', Frequency: '', Genre: '', Id: 0, ISBN: '',
    ImageUrl: '', Language: '', NumbersOfCopies: 0, Pages: 0, Price: 0, Publisher: '',
    Title: '', Type: '', Year: 0
  };

  imagesHostPath: string;


  constructor(private libraryService: LibraryService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.imagesHostPath = this.libraryService.imagesHostPath;
    this.imageNoneUrl=this.libraryService.imageNoneUrl;

    this.route$ = this.route.params.subscribe(
      (params: Params) => {
        this.id = +params["id"];
        if (this.id > 0) {

          this.libraryService.getDetails(this.id).subscribe(
            data => {
              this.details = data;
              this.isDataLoaded = true;
            }
          )
        }
      }
    );
  }

  ngOnDestroy() {
    if (this.route$) this.route$.unsubscribe();
  }

  downloadfile(type: string) {
    this.libraryService.downloadFile(this.id, type);
  }

}
