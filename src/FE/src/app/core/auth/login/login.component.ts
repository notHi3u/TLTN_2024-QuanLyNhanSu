import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  username: string = '';
  password: string = '';

  login() {
    // Here you can add your login logic, e.g., call an authentication service.
    console.log('Login attempt', { username: this.username, password: this.password });
  }
}
