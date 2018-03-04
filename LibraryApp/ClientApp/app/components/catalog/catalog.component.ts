import { Component, ViewChild, ElementRef } from '@angular/core';
import { IAssetsList } from '../shared/interfaces/IAssetsList';
import { IAssetListItem } from '../shared/interfaces/IAssetListItem';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';

import { LibraryService } from '../services/library.service';
import { ResourceService } from '../services/resource.service';
import { forEach } from '@angular/router/src/utils/collection';

import { style, state, animate, transition, trigger } from '@angular/core';

@Component({
    selector: 'catalog',
    templateUrl: './catalog.component.html',
    styleUrls: ['./catalog.component.css'],
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

export class CatalogComponent {

    private gridView: GridDataResult;
    private gridData: IAssetListItem[] = [];

    private selectedItems: number[] = [];
    private pageSize: number = 10;
    private skip: number = 0;

    imagesPath: string;

    deleteItem: IAssetListItem;
    deleteImgUrl: string;
    isDeleteConfirm: boolean;

    constructor(private libraryService: LibraryService, private resourceService: ResourceService) {
        this.loadItems();
        this.imagesPath = this.libraryService.smallImagesHostPath;
    }

    ngOnInit() {
        this.getAsserList();
    }

    protected pageChange(event: PageChangeEvent): void {
        this.skip = event.skip;
        this.loadItems();
    }

    private loadItems(): void {
        this.gridView = {
            data: this.gridData.slice(this.skip, this.skip + this.pageSize),
            total: this.gridData.length
        };
    }

    private getAsserList() {
        this.gridData = this.resourceService.get('assetsList');
        this.libraryService.getAllAssets().subscribe(
            data => {
                this.resourceService.set('assetsList', data)
                this.gridData = this.resourceService.get('assetsList');
            });
    }

    deleteConfirm(item: IAssetListItem, img: string) {
        this.deleteItem = item;
        this.deleteImgUrl = img;
        this.isDeleteConfirm = true;
    }

    deleteConfirmed(event: boolean) {
        if (event) {
            this.deleAsset(this.deleteItem.id);
        }
        this.isDeleteConfirm = false;
    }

    deleAsset(id: number): void {
        this.libraryService.deleteAsset(id).subscribe(
            data => {
                this.gridData = this.gridData.filter(x => x.id != id)
                this.resourceService.set('assetsList', this.gridData);
            }
        )
    }

    onClick(id: number): void {
        this.libraryService.deleteAsset(id).subscribe(
            data => {
                this.gridData = this.gridData.filter(x => x.id != id)
            }
        );
    }

    fileUpload(event: any) {
        let url: string = this.libraryService.host + '/api/UploadData';
        let data: FormData = new FormData();
        data.append('file', event.target.files[0], event.target.files[0].name)
        let xhr: XMLHttpRequest = new XMLHttpRequest();

        xhr.onreadystatechange = () => {
            if (xhr.readyState === 4 && xhr.status === 200) {
                this.getAsserList();

            }
        };

        xhr.open('POST', url, true);
        xhr.setRequestHeader('enctype', 'multipart/form-data');
        xhr.send(data);
    }

    onSelect(event: any) {
        for (let i = 0; i < event.deselectedRows.length; i++) {
            this.selectedItems = this.selectedItems.filter(x => x != event.deselectedRows[i].dataItem.id);
        }

        for (let i = 0; i < event.selectedRows.length; i++) {
            this.selectedItems.push(event.selectedRows[i].dataItem.id)
        }
    }

    getSelectedData(type: string) {
        this.libraryService.downloadSelected(this.selectedItems, type);
    }

}
