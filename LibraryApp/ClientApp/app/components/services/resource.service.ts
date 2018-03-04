import { Injectable } from "@angular/core";
import { IResource } from '../shared/interfaces/IResource';

@Injectable()
export class ResourceService {

    private static libraryData: IResource[] = [];

    set(key: string, data: any) {

        for (var i = 0; i < ResourceService.libraryData.length; i++) {
            if (ResourceService.libraryData[i].key == key) {
                ResourceService.libraryData[i].data = data;
                return;
            }
        }
        ResourceService.libraryData.push({ key: key, data: data });
    }

    get(key: string): any {
        for (var i = 0; i < ResourceService.libraryData.length; i++) {
            if (ResourceService.libraryData[i].key == key) {
                return ResourceService.libraryData[i].data;
            }
        }
    }

    remove(key: string) {
        ResourceService.libraryData = ResourceService.libraryData.filter(x => x.key != key)
    }

}