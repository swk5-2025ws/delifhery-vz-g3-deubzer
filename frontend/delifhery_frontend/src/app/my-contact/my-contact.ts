import {Component, inject} from '@angular/core';
import {ContactMethodService} from '../../services/contact-method.service';
import {AsyncPipe} from '@angular/common';

@Component({
  selector: 'app-my-contact',
  standalone:true,
  imports: [AsyncPipe],
  templateUrl: './my-contact.html',
  styleUrl: './my-contact.css',
})
export class MyContact {
  private contactMethodeService = inject(ContactMethodService)

  contactMethods$ = this.contactMethodeService.getForCurrentUser();
}

