import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { Product } from "../shared/Product";
import { Order, OrderItem } from "../shared/Order";
import { LoginRequest } from "../shared/LoginResults";
import { LoginResults } from "../shared/LoginResults";

@Injectable()
export class Store
{
    constructor(private http: HttpClient)
    {

    }

    public products: Product[] = [];
    public order: Order = new Order();
    public token = "";
    public expiration = new Date();

    loadProducts(): Observable<void>
    {
        return this.http.get<Product[]>("/api/products")
            .pipe(map(data =>
                    { 
                        this.products = ((data) as Product[]);
                        return;
                    }));
    }

    get loginRequired(): boolean
    {
        if (this.token.length === 0 || this.expiration > new Date())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    login(creds: LoginRequest)
    {
        return this.http.post<LoginResults>("/account/createtoken", creds)
            .pipe(map(data =>
            {
                this.token = data.token;
                this.expiration = data.expiration;
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

    checkout()
    {
        const header = new HttpHeaders().set("Authorization", `Bearer ${this.token}`);

        return this.http.post("/api/orders", this.order, { headers: header })
            .pipe(map(() => {this.order = new Order()}));
    }
}