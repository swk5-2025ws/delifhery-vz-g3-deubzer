import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymentComplete } from './payment-complete';

describe('PaymentComplete', () => {
  let component: PaymentComplete;
  let fixture: ComponentFixture<PaymentComplete>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PaymentComplete]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PaymentComplete);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
