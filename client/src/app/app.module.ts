import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import ProductListView from './views/productListView.component';
import { Store } from './services/store.service';
import { CartView } from './views/cartView.component';
import { ShopPage } from './pages/shopPage.component';
import { Checkout } from './pages/checkout.component';
import router from './router';

@NgModule({
    declarations: [
        AppComponent,
        ProductListView,
        CartView,
        ShopPage,
        Checkout
    ],
    imports: [
        BrowserModule,
        HttpClientModule,
        router
    ],
    providers: [
        Store
        ],
    bootstrap: [AppComponent]
})
export class AppModule { }
