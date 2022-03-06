import { Item } from "./item";

export interface BasketItem {
    id: number;
    item: Item;
    quantity: number;
}