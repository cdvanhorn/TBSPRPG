import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';

import { UserService } from '../../../services/user.service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  hide : boolean;
  loginForm;

  constructor(private router: Router,
    private formBuilder: FormBuilder,
    private userService: UserService) { 
      this.loginForm = this.formBuilder.group({
        email: '',
        password: ''
      });
    }

  ngOnInit(): void {
  }

  login(loginData): void {
    console.log(loginData);
    //authenticate, post to api/users/authenticate
    this.userService.authenticate(loginData.email, loginData.password).subscribe(
      usr => { 
        console.log(usr);
        localStorage.setItem("jwttoken", usr.token);
        this.router.navigate(['/console', {}]);
      });
    //post to authenticate api endpoint
    //add jwt token to local storage
    //go to the console
  }
}
