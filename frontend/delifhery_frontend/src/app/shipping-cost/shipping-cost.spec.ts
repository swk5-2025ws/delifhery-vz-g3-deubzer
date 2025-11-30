import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShippingCost } from './shipping-cost';

describe('ShippingCost', () => {
  let component: ShippingCost;
  let fixture: ComponentFixture<ShippingCost>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ShippingCost]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ShippingCost);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
