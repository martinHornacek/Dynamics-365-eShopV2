import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, map, Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Basket } from '../models/basket';
import { BasketItem } from '../models/basket-item';
import { Item } from '../models/item';

@Injectable({
  providedIn: 'root'
})
export class BasketService {

  private readonly _basket = new BehaviorSubject<Basket>({ id: 1, name: "Default Basket", description: "Default Basket description", basketItems: [] as BasketItem[] } as Basket);
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

  addItemToBasket(itemId: number, quantity: number): Observable<BasketItem> {
    const url = `${this.basketsUrl}/${this.basket.id}/basketitems`;
    return this.http.post<BasketItem>(url, { ItemId: itemId, Quantity: quantity })
      .pipe(catchError(this.handleError<BasketItem>(`addItem/${this.basket.id}`, { id: 0, item: { id: 0, price: 0, name: "", category: "", description: "" } as Item, quantity: 0} as BasketItem )));
  }

  removeItemFromBasket(basketItemId: number): Observable<unknown> {
    const url = `${this.basketsUrl}/${this.basket.id}/basketitems`;
    return this.http.delete(url, { body: { Id: basketItemId } })
      .pipe(catchError(this.handleError(`removeItem/${this.basket.id}`)));
  }

  handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error);
      return of(result as T);
    }
  }

  constructor(private http: HttpClient) { }
}

