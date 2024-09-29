import { Component } from '@angular/core';
import { Router } from '@angular/router';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'hrms-fe';
  constructor(private router: Router) {}

  ngOnInit(): void {
    const navbarLinks = document.querySelectorAll('#navbar .scrollto');

    function toggleActiveClass(): void {
      const position = window.scrollY + 200;

      navbarLinks.forEach((link) => {
        const hash = (link as HTMLAnchorElement).hash;
        const section = document.querySelector(hash) as HTMLElement;

        if (!section) {
          return;
        }

        if (position >= section.offsetTop && position <= (section.offsetTop + section.offsetHeight)) {
          link.classList.add('active');
        } else {
          link.classList.remove('active');
        }
      });
    }

    window.addEventListener('load', toggleActiveClass);
    document.addEventListener('scroll', toggleActiveClass);
  }
}

