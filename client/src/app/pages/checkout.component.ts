import { Component } from "@angular/core";
import { Store } from '../services/store.service';

@Component(
    {
    selector: "checkout",
    templateUrl: "checkout.component.html",
    styleUrls: ['checkout.component.css']
    }
)
export class Checkout
{
    constructor(public store: Store)
    {
    }

    onCheckout()
    {
        // TODO
        alert("Doing checkout");
    }
}