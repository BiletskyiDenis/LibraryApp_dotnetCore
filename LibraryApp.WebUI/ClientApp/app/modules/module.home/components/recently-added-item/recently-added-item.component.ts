import { Component, OnInit, Input } from '@angular/core';
import { IRecentlyItem } from '../../../../shared/interfaces/IRecentlyItem';

@Component({
  selector: 'recently-added-item',
  templateUrl: './recently-added-item.component.html',
  styleUrls: ['./recently-added-item.component.css']
})
export class RecentlyAddedItemComponent implements OnInit {

  @Input() recentlyItems: IRecentlyItem[]=[];
  @Input() imagesPath: string;
  
  constructor() { }

  ngOnInit() {
  }

}
