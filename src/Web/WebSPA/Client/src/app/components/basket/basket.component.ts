import { Component, OnInit } from '@angular/core';
import { BasketItem } from 'src/app/models/basket-item';
import { StoreService } from 'src/app/services/store.service';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.css']
})
export class BasketComponent implements OnInit {

  constructor(public storeService: StoreService) { }
 
  removeFromBasket(item: BasketItem){
    this.storeService.basket.removeItem(item);
  }
 
  emptyBasket(){
    this.storeService.basket.emptyBasket();
  }
  
  ngOnInit(): void {
  }

}