import { Component, OnInit } from '@angular/core';
import * as $ from 'jquery';


@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
    $('img').click(function() {
      $('img').removeClass('selected')
      $(this).addClass('selected');
    });
  }



}
