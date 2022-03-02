﻿import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { Product } from "../shared/Product";
import { Order, OrderItem } from "../shared/Order";

@Injectable()
export class Store
{
    constructor(private http: HttpClient)
    {

    }

    public products: Product[] = [];

    public order: Order = new Order();

    loadProducts(): Observable<void>
    {
        return this.http.get<Product[]>("/api/products")
            .pipe(map(data =>
                    { 
                        this.products = ((data) as Product[]);
                        return;
                    }));
    }

    addToOrder(product: Product)
    {
        let item: OrderItem;

        item = ((this.order.items.find(o => o.productId === product.id)) as OrderItem);

        if (item)
        {
            item.quantity++;
        }
        else
        {
            item = new OrderItem();
            item.productId = product.id;
            item.productTitle = product.title;
            item.productArtId = product.artId;
            item.productArtist = product.artist;
            item.productCategory = product.category;
            item.productSize = product.size;
            item.unitPrice = product.price;
            item.quantity = 1;

            this.order.items.push(item);
        }
    }
}