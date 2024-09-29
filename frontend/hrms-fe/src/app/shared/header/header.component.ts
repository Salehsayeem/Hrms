import { Component, ElementRef, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../core/auth.service';
import { DecodedToken } from '../../core/interfaces/auth.interface';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent implements OnInit{
  token: DecodedToken | null = null;

  constructor(private el: ElementRef,
    public router: Router,
    private authService:AuthService
    ) {}
  ngOnInit(): void {
    this.decodeToken();
    const toggleSidebarBtn = this.el.nativeElement.querySelector('.toggle-sidebar-btn');
    toggleSidebarBtn.addEventListener('click', () => {
      document.body.classList.toggle('toggle-sidebar');
    });
  }

  decodeToken() {
    const jwt = localStorage.getItem('token')
    if(jwt){
      this.token = this.authService.getDecodedToken(jwt);
    }
    else{
      this.token = null
    }
    return this.token;
  }
  logOut(){
    localStorage.clear();
    this.router.navigate(['/auth/login']);

  }
}
