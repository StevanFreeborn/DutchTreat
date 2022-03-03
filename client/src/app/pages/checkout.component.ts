import { Component } from "@angular/core";
import { Store } from '../services/store.service';
import { Router } from "@angular/router";

@Component(
    {
    selector: "checkout",
    templateUrl: "checkout.component.html",
    styleUrls: ['checkout.component.css']
    }
)
export class Checkout
{
    public errorMessage = "";

    constructor(public store: Store, private router: Router)
    {
    }

    onCheckout()
    {
        this.errorMessage = "";
        this.store.checkout()
            .subscribe(() =>
                {
                    this.router.navigate(["/"]);
                },
                error =>
                {
                    this.errorMessage = `Failed to checkout: ${error}`;
                });
    }
}