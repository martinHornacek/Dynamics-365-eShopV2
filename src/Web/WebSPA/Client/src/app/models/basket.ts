import { BasketItem } from "./basket-item";

export interface Basket {
    id: number;
    name: string;
    description: string;
    basketItems: BasketItem[];
}