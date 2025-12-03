import {Component, inject} from '@angular/core';
import {ContactMethodService} from '../../services/contact-method.service';
import {AsyncPipe, NgOptimizedImage, NgTemplateOutlet} from '@angular/common';
import {FormsModule} from '@angular/forms';

@Component({
  selector: 'app-my-contact',
  standalone:true,
  imports: [AsyncPipe, FormsModule, NgTemplateOutlet, NgOptimizedImage],
  templateUrl: './my-contact.html',
  styleUrl: './my-contact.css',
})
export class MyContact {
  constructor() {}
  private contactMethodeService = inject(ContactMethodService)
  selectedType: "email" | "phone" = "email"

  contactMethods$ = this.contactMethodeService.getForCurrentUser();
}

