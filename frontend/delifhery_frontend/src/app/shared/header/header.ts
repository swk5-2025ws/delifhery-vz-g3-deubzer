import {Component, OnInit} from '@angular/core';
import {NgOptimizedImage} from '@angular/common';
import {Router, RouterLink, RouterLinkActive} from '@angular/router';
import {KeycloakService} from 'keycloak-angular';

@Component({
  selector: 'app-header',
  standalone:true,
  imports: [NgOptimizedImage,RouterLink, RouterLinkActive],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header implements OnInit{
  isLoggedIn = false;
  username: string | null = null;
  constructor(private keycloak: KeycloakService) {}
  async ngOnInit() {
    this.isLoggedIn = await this.keycloak.isLoggedIn();
    if(this.isLoggedIn){
      const profile = await this.keycloak.loadUserProfile();
      this.username = profile.username || profile.firstName || null;
    }
  }
  login(){
    this.keycloak.login();
  }
  register(){
    this.keycloak.register();
  }
  logout(){
    this.keycloak.logout();
  }
  getUsername(){
    return this.username;
  }
}
