import { Component, Input, OnInit, Output, EventEmitter } from "@angular/core";
import { IAssetListItem } from '../shared/interfaces/IAssetListItem';

@Component({
  selector: 'delete-confirm',
  templateUrl: './delete-confirm.component.html',
  styleUrls: ['./delete-confirm.component.css']
})
export class DeleteConfirmComponent implements OnInit {

  @Input() isShow: boolean;
  @Input() deleteItem: IAssetListItem;

  @Input() imagesPath:string;

  @Output() onDelete = new EventEmitter<boolean>();

  constructor() { }

  ngOnInit() {
  }

  onDeleteClick(arg:any): void {
    this.onDelete.emit(arg);
  }
}
