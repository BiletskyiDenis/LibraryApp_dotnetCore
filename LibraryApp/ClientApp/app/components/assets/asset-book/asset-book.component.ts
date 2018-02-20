import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ImageSelectorComponent } from '../../image-selector/image-selector.component';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LibraryService } from '../../services/library.service';
import { ActivatedRoute, Params } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';

@Component({
  selector: 'asset-book',
  templateUrl: './asset-book.component.html',
  styleUrls: ['./asset-book.component.css', '../share/assets.css']
})

export class AssetBookComponent implements OnInit {

  form: FormGroup;

  isEdit: boolean;

  imagesHostPath: string;
  isDataLoaded:boolean;

  private id: number;
  private route$: Subscription;

  @ViewChild('imageInput') inputEl: ImageSelectorComponent;

  constructor(private libraryService: LibraryService, private route: ActivatedRoute, private fb: FormBuilder) {

    this.form = fb.group({
      'id': [0],
      'title': [null, Validators.compose([Validators.required, Validators.maxLength(150)])],
      'author': [null, Validators.compose([Validators.required, Validators.maxLength(70)])],
      'numbersOfCopies': [null, Validators.compose([Validators.required, Validators.pattern('^[0-9]*$'), Validators.max(1000)])],
      'country': [null, Validators.compose([Validators.required, Validators.maxLength(50)])],
      'genre': [null, Validators.compose([Validators.required, Validators.maxLength(50)])],
      'isbn': [0, Validators.compose([Validators.required, Validators.pattern('^[0-9]*$')])],
      'language': [null, Validators.compose([Validators.required, Validators.maxLength(100)])],
      'pages': [null, Validators.compose([Validators.required, Validators.pattern('^[0-9]*$'), Validators.max(10000)])],
      'price': [null, Validators.compose([Validators.required, Validators.pattern('^[0-9]+\.?[0-9]*$'), Validators.max(1000000)])],
      'publisher': [null, Validators.compose([Validators.required, Validators.maxLength(100)])],
      'year': [null, Validators.compose([Validators.required, Validators.pattern('^[0-9]*$'), Validators.max(3000)])],
      'description': [null, Validators.maxLength(1000)],
      'imageUrl': [0],
    });
  }

  ngOnInit(): void {
    this.imagesHostPath = this.libraryService.imagesHostPath;

    this.route$ = this.route.params.subscribe(
      (params: Params) => {
        this.id = +params["id"];
        if (this.id > 0) {
          this.isEdit = true;

          this.libraryService.getBook(this.id).subscribe(
            data => {
              this.form.controls['id'].setValue(data.id);
              this.form.controls['title'].setValue(data.title);
              this.form.controls['author'].setValue(data.author);
              this.form.controls['numbersOfCopies'].setValue(data.numbersOfCopies);
              this.form.controls['country'].setValue(data.country);
              this.form.controls['genre'].setValue(data.genre);
              this.form.controls['isbn'].setValue(data.isbn);
              this.form.controls['language'].setValue(data.language);
              this.form.controls['pages'].setValue(data.pages);
              this.form.controls['price'].setValue(data.price);
              this.form.controls['publisher'].setValue(data.publisher);
              this.form.controls['year'].setValue(data.year);
              this.form.controls['description'].setValue(data.description);
              this.form.controls['imageUrl'].setValue(data.imageUrl);
              this.inputEl.imageUrl = data.imageUrl;
              this.isDataLoaded=true;
            }
          )
        } else{ 
          this.isDataLoaded=true;
        }
      }
    );

  }

  submit(post:any) {
    let formData = new FormData();
    let files: FileList = this.inputEl.Files;

    console.log(JSON.stringify(post));
    formData.append('asset', JSON.stringify(post));
    if (files.length > 0) {
      formData.append('file', files[0], files[0].name);
    }

    if (!this.isEdit) {
      this.libraryService.addNewBook(formData);
    } else {
      this.libraryService.editBook(formData);
    }
  }

  ngOnDestroy() {
    if (this.route$) this.route$.unsubscribe();
  }
}
