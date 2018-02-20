import {IAsset} from './IAsset';
export interface IBook extends IAsset{
    Author:string;
    Pages:number;
    ISBN:number;
}