import { BasketItem } from "./basket-item";

export interface Basket {
    new_id: string;
    new_name: string;
    new_description: string;
    basketItems: BasketItem[];
}