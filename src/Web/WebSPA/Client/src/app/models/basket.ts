import { BasketItem } from "./basket-item";

export class Basket {
    basketItems: BasketItem[] = [];

    addItem(basketItem: BasketItem){
      let found:boolean = false;
      this.basketItems = this.basketItems.map(bi => 
        {
          if(bi.item?.id == basketItem.item?.id){ 
              bi.quantity++; 
              found = true;
          }
        return bi;
        });

      if(!found){
          this.basketItems.push(basketItem);
      }
    }

    removeItem(item: BasketItem) {
      const index = this.basketItems.indexOf(item, 0);
      if (index > -1) {
          this.basketItems.splice(index, 1);
      }
    }

    emptyBasket(){
      this.basketItems = [];
    }

    getTotalValue():number {
      let sum = this.basketItems.reduce(
          (a, b) => {a = a + b.item?.price * b.quantity; return a;}, 0);
      return sum;
    }

    isBasketValid(): boolean{
      if(this.basketItems.find(basktitem => (basktitem.quantity == null || basktitem.quantity <= 0)) === undefined)
          return true;      
      return false;
    }
}