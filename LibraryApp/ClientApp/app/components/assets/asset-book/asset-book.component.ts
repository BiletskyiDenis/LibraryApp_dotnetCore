import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ImageSelectorComponent } from '../../image-selector/image-selector.component';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LibraryService } from '../../services/library.service';
import { ActivatedRoute, Params } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';
import { IBook } from '../../shared/interfaces/IBook';

@Component({
    selector: 'asset-book',
    templateUrl: './asset-book.component.html',
    styleUrls: ['./asset-book.component.css', '../share/assets.css']
})

export class AssetBookComponent implements OnInit {

    book: IBook;

    isEdit: boolean;
    imagesHostPath: string;
    isDataLoaded: boolean;

    private id: number;
    private route$: Subscription;

    @ViewChild('imageInput') inputEl: ImageSelectorComponent;

    constructor(private libraryService: LibraryService, private route: ActivatedRoute) {
        this.imagesHostPath = this.libraryService.imagesHostPath;
        this.book = {
            author: '', country: '', description: '',
            genre: '', id: 0, imageUrl: '', isbn: 0,
            language: '', numbersOfCopies: 0, pages: 0,
            price: 0, publisher: '', title: '', year: 0
        };
    }

    ngOnInit(): void {
        this.route$ = this.route.params.subscribe(
            (params: Params) => {
                this.id = +params["id"];
                if (this.id > 0) {
                    this.isEdit = true;
                    this.getAssetData(this.id);
                    return;
                }
                this.isDataLoaded = true;
            }
        );
    }

    getAssetData(id: number): void {
        this.libraryService.getBook(this.id).subscribe(
            data => {
                this.book = data;
                this.inputEl.imageUrl = data.imageUrl;
                this.isDataLoaded = true;
            }
        );
    }

    onSubmit(form: any) {
        let formData = new FormData();
        let files: FileList = this.inputEl.Files;

        formData.append('asset', JSON.stringify(form.value));

        if (files.length > 0) {
            formData.append('file', files[0], files[0].name);
        }

        if (!this.isEdit) {
            this.libraryService.addNewBook(formData);
            return;
        }
        this.libraryService.editBook(formData);
    }

    ngOnDestroy() {
        if (this.route$) this.route$.unsubscribe();
    }
}
