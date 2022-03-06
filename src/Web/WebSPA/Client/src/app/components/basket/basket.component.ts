import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Basket } from 'src/app/models/basket';
import { BasketItem } from 'src/app/models/basket-item';
import { BasketService } from 'src/app/services/basket.service';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.css']
})
export class BasketComponent implements OnInit {

  constructor(public basketService: BasketService) { }

  removeFromBasket(basketItemId: number) {
    this.basketService.removeItemFromBasket(basketItemId)
      .subscribe(() => {
        const index = this.basketService.basket.basketItems.findIndex(bi => bi.id == basketItemId);
        if (index > -1) {
          this.basketService.basket.basketItems.splice(index, 1);
        }
      });
  }

  emptyBasket() {
    this.basketService.basket.basketItems.forEach(item => this.removeFromBasket(item.id));
  }

  getTotalValue(): number {
    let sum = this.basketService.basket.basketItems.reduce(
      (a, b) => { a = a + b.item?.price * b.quantity; return a; }, 0);
    return sum;
  }

  isBasketValid(): boolean {
    if (this.basketService.basket.basketItems.find(basktItem => (basktItem.quantity == null || basktItem.quantity <= 0)) === undefined)
      return true;
    return false;
  }

  ngOnInit(): void {
  }

}