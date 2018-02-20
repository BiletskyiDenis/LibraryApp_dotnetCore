import { Injectable } from "@angular/core";
import { IResource } from '../shared/interfaces/IResource';

@Injectable()
export class ResourceService {

    private static data: IResource[] = [];

    set(key: string, data: any) {

        let findKey: boolean=false;
        for (var i = 0; i < ResourceService.data.length; i++) {
            if (ResourceService.data[i].key == key) {
                ResourceService.data[i].data = data;
                findKey = true;
            }
        }
        if (!findKey) {
            ResourceService.data.push({ key: key, data: data })
        }
    }

    get(key: string): any {
        for (var i = 0; i < ResourceService.data.length; i++) {
            if (ResourceService.data[i].key == key) {
                return ResourceService.data[i].data;
            }
        }
    }

    remove(key: string){
        ResourceService.data=ResourceService.data.filter(x=>x.key!=key)
    }

}