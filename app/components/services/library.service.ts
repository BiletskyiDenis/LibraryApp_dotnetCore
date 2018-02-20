import { Injectable } from "@angular/core";
import { Http, Response, Headers } from "@angular/http";
import 'rxjs/add/operator/map';
import { Observable } from 'rxjs/Rx';


import { IBook } from '../shared/interfaces/IBook';
import { IJournal } from '../shared/interfaces/IJournal';
import { IBrochure } from '../shared/interfaces/IBrochure';
import { IRecentlyItem } from '../shared/interfaces/IRecentlyItem';
import { IDetails } from '../shared/interfaces/IDetails';

import { ResponseContentType, RequestMethod, RequestOptions } from '@angular/http';

import { Router } from '@angular/router';

import { IAssetListItem } from '../shared/interfaces/IAssetListItem';

import { saveAs } from 'file-saver';

@Injectable()
export class LibraryService {

    host: string = '/';
    imageNoneUrl:string='/assets/img/none.jpg';
    imageSmallNoneUrl:string='/assets/img/none_small.png';

    private imagesPath: string = '/img/assets/';
    private smallImagesPath: string = '/img/assets_small/';


    handleError: any;

    get imagesHostPath() {
        return this.host + this.imagesPath;
    }

    get smallImagesHostPath() {
        return this.host + this.smallImagesPath;
    }


    constructor(private _http: Http, private http: Http, private router: Router) { }

    getAllAssets() {
        return this._http.get(this.host + '/api/Asset').map(res => <IAssetListItem[]>res.json());
    }

    getBook(id: number) {
        return this._http.get(this.host + '/api/Book/' + id).map(res => <IBook>res.json());
    }

    getJournal(id: number) {
        return this._http.get(this.host + '/api/Journal/' + id).map(res => <IJournal>res.json());
    }

    getBrochure(id: number) {
        return this._http.get(this.host + '/api/Brochure/' + id).map(res => <IBrochure>res.json());
    }

    getRecentlyAdded(type: string) {
        return this._http.post(this.host + '/api/RecentlyAdded/' + type, '').map(res => <IRecentlyItem[]>res.json());
    }

    getDetails(id: number) {
        return this._http.post(this.host + '/api/Details/' + id, '').map(res => <IDetails>res.json());
    }


    addNewBook(data: FormData) {
        this.assetHandler(data, '/api/Book', 'POST');
    }

    addNewJournal(data: FormData) {
        this.assetHandler(data, '/api/Journal', 'POST');
    }

    addNewBrochure(data: FormData) {
        this.assetHandler(data, '/api/Brochure', 'POST');
    }

    editBook(data: FormData) {
        this.assetHandler(data, '/api/Book', 'PUT');
    }

    editJournal(data: FormData) {
        this.assetHandler(data, '/api/Journal', 'PUT');
    }

    editBrochure(data: FormData, ) {
        this.assetHandler(data, '/api/Brochure', 'PUT');
    }

    deleteAsset(id: number) {
        return this._http.delete(this.host + '/api/Asset/' + id);
    }

    assetHandler(data: FormData, apiUrl: string, httpType: string) {
        let url: string = this.host + apiUrl;
        let xhr: XMLHttpRequest = new XMLHttpRequest();

        xhr.onreadystatechange = () => {
            if (xhr.readyState === 4) {
                if (xhr.status === 200) {
                    this.router.navigate(['/catalog']);
                }
            }
        };

        xhr.open(httpType, url, true);
        xhr.setRequestHeader('enctype', 'multipart/form-data');
        xhr.send(data);
    }

    downloadFile(id: number, type: string) {
        var dataFile = { id: id, type: type };
       this.downloadData(dataFile, '/api/DownloadData');
    }

    downloadSelected(id: number[], type: string) {
        var dataFile = { id: id, type: type };
       this.downloadData(dataFile,'/api/DownloadSelected');
    }

    downloadData(data:any, apiUrl:any) {
        var xhr = new XMLHttpRequest();
        xhr.open("POST", this.host + apiUrl, true);
        xhr.responseType = 'blob';
        xhr.setRequestHeader('Content-type', 'application/json');
        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
                var fileName:any = xhr.getResponseHeader('fileName');
                var blob = new Blob([this.response], { type: 'application/octet-stream' });
                saveAs(blob, fileName);
            }
        }
        xhr.send(JSON.stringify(data));
    }
}