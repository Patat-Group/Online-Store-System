import { Component, OnInit, ViewChild } from '@angular/core';
import { NgbCarouselConfig } from '@ng-bootstrap/ng-bootstrap';
import { NgbCarousel, NgbSlideEvent, NgbSlideEventSource } from '@ng-bootstrap/ng-bootstrap';
import { Products } from '../Models/Products';
import { VipAds } from '../Models/VipAds';
import { ProductsService } from '../Services/ProductService/products.service';
import { VipService } from '../Services/VIPService/vip.service';



@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent implements OnInit {

  vipAds: VipAds[] | any;
  products: Products[] | any;

  constructor(config: NgbCarouselConfig, private productsServices: ProductsService
    , private vipAdsServices: VipService) {
    config.interval = 2000;
    config.keyboard = true;
    config.pauseOnHover = true;
    vipAds: [];
    products: [];
  }

  paused = false;
  unpauseOnArrow = false;
  pauseOnIndicator = false;
  pauseOnHover = true;
  pauseOnFocus = true;


  ngOnInit(): void {
    this.vipAdsServices.GetVips().subscribe(list => {
      this.vipAds = list;
    }, err => console.log(err));

    this.productsServices.GetProducts().subscribe(list => {
      this.products = list;
    }, err => console.log(err));
  }

  @ViewChild('carousel', { static: true }) carousel: NgbCarousel | any;


  togglePaused() {
    if (this.paused) {
      this.carousel.cycle();
    } else {
      this.carousel.pause();
    }
    this.paused = !this.paused;
  }

  onSlide(slideEvent: NgbSlideEvent) {
    if (this.unpauseOnArrow && slideEvent.paused &&
      (slideEvent.source === NgbSlideEventSource.ARROW_LEFT || slideEvent.source === NgbSlideEventSource.ARROW_RIGHT)) {
      this.togglePaused();
    }
    if (this.pauseOnIndicator && !slideEvent.paused && slideEvent.source === NgbSlideEventSource.INDICATOR) {
      this.togglePaused();
    }
  }

}
