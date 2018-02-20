import { Component, OnInit, Input } from '@angular/core';
import { ViewChild, ElementRef } from '@angular/core';

@Component({
  selector: 'image-selector',
  templateUrl: './image-selector.component.html',
  styleUrls: ['./image-selector.component.css']
})
export class ImageSelectorComponent implements OnInit {

  imagePath: string = '/img/noimg.png';
  
  @Input() imagesHostPath: string;

  @Input() set imageUrl(value: string) {
    if (value != '' && value != null && value != 'none' && value != 'null') {
      this.imagePath = this.imagesHostPath + value;
    }
  }

  @ViewChild('fileInput') inputEl: ElementRef;

  private _files: FileList;
  isImageLoaded: boolean;

  get Files(): FileList {
    let imageSelector = this.inputEl.nativeElement;
    this._files = imageSelector.files;
    return this._files;
  }

  constructor() { }

  ngOnInit() {

  }

  imageSelected(event:any) {
    var reader = new FileReader();
    if (event.target.files && event.target.files[0]) {
      var reader = new FileReader();

      reader.onload = (event: any) => {
        this.imagePath = event.target.result;
      }
      this.isImageLoaded = true;
      reader.readAsDataURL(event.target.files[0]);
    }
  }
}
