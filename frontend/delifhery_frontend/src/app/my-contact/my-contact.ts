import {Component, inject} from '@angular/core';
import {ContactMethod, ContactMethodService} from '../../services/contact-method.service';
import {AsyncPipe, NgOptimizedImage, NgTemplateOutlet} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {filter, map} from 'rxjs';

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
  selectedType: "email" | "phone" = "email";
  value: string = "";
  label?: string = "";
  isPrimary: boolean = false;
  searchText : string = "";

  contactMethods$ = this.contactMethodeService.getForCurrentUser();
  filterContacts = this.contactMethods$;

  searchContacts() {
    const text = this.searchText.toLowerCase();

    this.filterContacts = this.contactMethods$.pipe(
      map(list =>
        list.filter(c =>
          c.value.toLowerCase().includes(text)
        )
      )
    );
  }


  makePrimary(contact: ContactMethod) {
    // TODO: API-Call zum Backend, um diesen Contact als primary zu setzen
    console.log('Set primary:', contact);
  }

  deleteContact(contact: ContactMethod) {
    this.contactMethodeService.deleteContact(contact.contactId)
      .subscribe({
        next: () => {
          this.contactMethods$ = this.contactMethodeService.getForCurrentUser();
          this.searchContacts();
        },
        error: err => {
          console.error('Delete failed', err);
        }
      });
  }

  saveContact(){
    const newContact = {
      type: this.selectedType,
      value: this.value,
      isPrimary: this.isPrimary,
    };

    this.contactMethodeService.createContactMethod(newContact).subscribe(result => {
      if(result){
        if (result) {
          this.contactMethods$ = this.contactMethodeService.getForCurrentUser();
          this.searchContacts();
          this.value = "";
          this.label = "";
          this.isPrimary = false;
        }
      }
    });
  }

  protected readonly filter = filter;
}

