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
    const index = this.basketService.basket.basketItems.findIndex((bi) => bi.new_item.new_id === this.item.new_id);

    if (index !== -1) {
      const quantity = this.basketService.basket.basketItems[index].new_quantity;
      this.basketService.updateItemInBasket(this.item, quantity + 1)
      .subscribe(_ => {
        this.basketService.basket.basketItems[index].new_quantity = (quantity + 1);
      });
    } else {
      this.basketService.addItemToBasket(this.item, 1)
      .subscribe((basketItem) => {
        basketItem.new_item = this.item; // update the returned basket item object with item ref
        this.basketService.basket.basketItems.push(basketItem) 
      });
    }

    this.router.navigate(['/basket']);
  }
}