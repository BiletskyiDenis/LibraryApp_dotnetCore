import {IAsset} from './IAsset';
export interface IBook extends IAsset{
    author:string;
    pages:number;
    isbn:number;
}