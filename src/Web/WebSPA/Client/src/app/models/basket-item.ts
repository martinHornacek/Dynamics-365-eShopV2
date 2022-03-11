import { Item } from "./item";

export interface BasketItem {
    new_id: string;
    new_item: Item;
    new_quantity: number;
}