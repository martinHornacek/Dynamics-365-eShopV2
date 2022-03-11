import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Item } from 'src/app/models/item';
import { BasketService } from 'src/app/services/basket.service';
import { ItemsService } from 'src/app/services/items.service';

@Component({
  selector: 'app-item-details',
  templateUrl: './item-details.component.html',
  styleUrls: ['./item-details.component.css']
})
export class ItemDetailsComponent implements OnInit {

  item: Item = {new_id: "0", new_name:"", new_price: 0, new_category: "", new_description: ""};

  constructor(
    private route: ActivatedRoute,
    private itemsService: ItemsService,
    private basketService: BasketService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.getItem();
  }

  getItem(): void {
    const id = String(this.route.snapshot.paramMap.get('id'));
    this.itemsService.getItem(id)
      .subscribe(item => this.item = item);   
  }

  addToBasket(): void {
    // TODO handle increase of quantity if item already exists
    // by initiating updateItemInBasket action (not implemented)

    this.basketService.addItemToBasket(this.item, 1)
      .subscribe((basketItem) => {
        basketItem.new_item = this.item; // update the returned basket item object with item ref
        this.basketService.basket.basketItems.push(basketItem) 
      });

    this.router.navigate(['/basket']);
  }
}