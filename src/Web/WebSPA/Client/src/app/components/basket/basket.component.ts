import { Component, OnInit } from '@angular/core';
import { BasketService } from 'src/app/services/basket.service';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.css']
})
export class BasketComponent implements OnInit {

  constructor(public basketService: BasketService) { }

  removeFromBasket(basketItemId: string) {
    this.basketService.removeItemFromBasket(basketItemId)
      .subscribe(() => {
        const index = this.basketService.basket.basketItems.findIndex(bi => bi.new_id == basketItemId);
        if (index > -1) {
          this.basketService.basket.basketItems.splice(index, 1);
        }
      });
  }

  emptyBasket() {
    this.basketService.basket.basketItems.forEach(item => this.removeFromBasket(item.new_id));
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