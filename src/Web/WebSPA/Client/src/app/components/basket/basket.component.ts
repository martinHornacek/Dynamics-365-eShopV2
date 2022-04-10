import { Component, OnInit } from '@angular/core';
import { Item } from 'src/app/models/item';
import { BasketService } from 'src/app/services/basket.service';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.css']
})
export class BasketComponent implements OnInit {

  constructor(public basketService: BasketService) { }

  removeFromBasket(item: Item) {
    this.basketService.removeItemFromBasket(item)
      .subscribe(() => {
        const index = this.basketService.basket.basketItems.findIndex(basketItem => basketItem.new_item.new_id == item.new_id);
        if (index > -1) {
          this.basketService.basket.basketItems.splice(index, 1);
        }
      });
  }

  updateItemInBasket(item: Item, quantity: number) {
    this.basketService.updateItemInBasket(item, quantity)
      .subscribe(_ => {
        const index = this.basketService.basket.basketItems.findIndex((basketItem) => basketItem.new_item.new_id === item.new_id);
        if (index > -1) {
          this.basketService.basket.basketItems[index].new_quantity = quantity;
        }
      });
  }

  emptyBasket() {
    this.basketService.basket.basketItems.forEach(item => this.removeFromBasket(item.new_item));
  }

  getTotalValue(): number {
    let sum = this.basketService.basket.basketItems.reduce(
      (a, b) => { a = a + b.new_item?.new_price * b.new_quantity; return a; }, 0);
    return sum;
  }

  isBasketValid(): boolean {
    if (this.basketService.basket.basketItems.find(basktItem => (basktItem.new_quantity == null || basktItem.new_quantity <= 0)) === undefined)
      return true;
    return false;
  }

  ngOnInit(): void {
  }

}