import { Component, OnInit, ViewChild } from '@angular/core';
import { NgbCarouselConfig } from '@ng-bootstrap/ng-bootstrap';
import { NgbCarousel, NgbSlideEvent, NgbSlideEventSource } from '@ng-bootstrap/ng-bootstrap';
import { Category } from '../Models/Category';
import { VipAds } from '../Models/VipAds';
import { CategoryServicesService } from '../Services/CategoryServices/category-services.service';
import { VipService } from '../Services/VIPServices/vip.service';
import { NavigationExtras, Router } from '@angular/router';



@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})

export class MainPageComponent implements OnInit {

  vipAds: VipAds[] | any;
  categories: Category[] | any;

  constructor(config: NgbCarouselConfig, private categoryService: CategoryServicesService
    , private vipAdsServices: VipService, private router: Router) {
    config.interval = 3000;
    config.keyboard = true;
    config.pauseOnHover = true;
    vipAds: [];
    categories: [];
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

    this.categoryService.GetCategories().subscribe(list => {
      this.categories = list;
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
