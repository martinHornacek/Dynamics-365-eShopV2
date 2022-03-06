import { Component } from '@angular/core';
import { BasketService } from './services/basket.service';
import { StoreService } from './services/store.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'client';

  constructor(
    public basketService: BasketService
  ) { }
}
