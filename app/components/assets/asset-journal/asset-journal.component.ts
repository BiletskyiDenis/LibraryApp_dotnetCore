import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ImageSelectorComponent } from '../../image-selector/image-selector.component';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LibraryService } from '../../services/library.service';
import { ActivatedRoute, Params } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';

@Component({
  selector: 'asset-journal',
  templateUrl: './asset-journal.component.html',
  styleUrls: ['./asset-journal.component.css','../share/assets.css'],
})
export class AssetJournalComponent implements OnInit {
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
        'numbersOfCopies': [null, Validators.compose([Validators.required, Validators.pattern('^[0-9]*$'), Validators.max(1000)])],
        'country': [null, Validators.compose([Validators.required, Validators.maxLength(50)])],
        'genre': [null, Validators.compose([Validators.required, Validators.maxLength(50)])],
        'language': [null, Validators.compose([Validators.required, Validators.maxLength(100)])],
        'frequency': [null, Validators.compose([Validators.required, Validators.maxLength(100)])],
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
  
            this.libraryService.getJournal(this.id).subscribe(
              data => {
                this.form.controls['id'].setValue(data.Id);
                this.form.controls['title'].setValue(data.Title);
                this.form.controls['numbersOfCopies'].setValue(data.NumbersOfCopies);
                this.form.controls['country'].setValue(data.Country);
                this.form.controls['genre'].setValue(data.Genre);
                this.form.controls['language'].setValue(data.Language);
                this.form.controls['price'].setValue(data.Price);
                this.form.controls['publisher'].setValue(data.Publisher);
                this.form.controls['year'].setValue(data.Year);
                this.form.controls['frequency'].setValue(data.Frequency);
                this.form.controls['description'].setValue(data.Description);
                this.form.controls['imageUrl'].setValue(data.ImageUrl);
                this.inputEl.imageUrl = data.ImageUrl;
                this.isDataLoaded=true;
              }
            )
          }else{ 
            this.isDataLoaded=true;
          }
        }
      );
  
    }
  
    submit(post:any) {
      let formData = new FormData();
      let files: FileList = this.inputEl.Files;
  
      formData.append('asset', JSON.stringify(post));
      if (files.length > 0) {
        formData.append('file', files[0], files[0].name);
      }
  
      if (!this.isEdit) {
        this.libraryService.addNewJournal(formData);
      } else {
        this.libraryService.editJournal(formData);
      }
    }
  
    ngOnDestroy() {
      if (this.route$) this.route$.unsubscribe();
    }
}
