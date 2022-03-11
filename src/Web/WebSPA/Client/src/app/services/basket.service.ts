import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Basket } from '../models/basket';
import { BasketItem } from '../models/basket-item';
import { Item } from '../models/item';

@Injectable({
  providedIn: 'root'
})
export class BasketService {

  private readonly _basket = new BehaviorSubject<Basket>({ new_id: "1", new_name: "Default Basket", new_description: "Default Basket description.", basketItems: [] as BasketItem[] } as Basket);
  readonly basket$ = this._basket.asObservable();

  get basket(): Basket {
    return this._basket.getValue();
  }

  set basket(val: Basket) {
    this._basket.next(val);
  }

  basketsUrl = `${environment.basketsApiUrl}/baskets`;

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  addItemToBasket(item: Item, quantity: number): Observable<BasketItem> {
    const url = `${this.basketsUrl}/${this.basket.new_id}/basketitems`;
    const payload =  { new_name: item.new_name, new_id: String(Math.floor(Math.random() * 100)), new_itemid: item.new_id, new_quantity: quantity, new_basketid: this.basket.new_id };
    return this.http.post<BasketItem>(url, payload)
      .pipe(catchError(this.handleError<BasketItem>(`addItem/${this.basket.new_id}`, { new_id: "0", new_item: { new_id: "0", new_price: 0, new_name: "", new_category: "", new_description: "" } as Item, new_quantity: 0} as BasketItem )));
  }

  removeItemFromBasket(basketItemId: string): Observable<unknown> {
    const url = `${this.basketsUrl}/${this.basket.new_id}/basketitems`;
    return this.http.delete(url, { body: { new_id: basketItemId } })
      .pipe(catchError(this.handleError(`removeItem/${this.basket.new_id}`)));
  }

  handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error);
      return of(result as T);
    }
  }

  constructor(private http: HttpClient) { }
}

