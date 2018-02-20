import { Component, OnInit, Input, Output} from '@angular/core';
import {IRecentlyItem} from '../../shared/interfaces/IRecentlyItem';

import {LibraryService} from '../../services/library.service';
import {ResourceService} from '../../services/resource.service';

@Component({
  selector: 'recently-added',
  templateUrl: './recently-added.component.html',
  styleUrls: ['./recently-added.component.css']
})
export class RecentlyAddedComponent implements OnInit {

  @Input() title:string;
  @Input() type: string;
  @Input() imagesPath: string;

  recentlyItems:IRecentlyItem[]=[];

  constructor(private libraryService: LibraryService, private resourceService:ResourceService) { 
    
  }

  ngOnInit() {
    this.imagesPath = this.libraryService.imagesHostPath;
    this.recentlyItems=this.resourceService.get(this.type);
    this.libraryService.getRecentlyAdded(this.type).subscribe(data=>{
      this.resourceService.set(this.type, data);
      this.recentlyItems=this.resourceService.get(this.type);
    });
  }

}
