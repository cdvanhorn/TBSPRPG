import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { Router } from '@angular/router';

import { UserService } from '../../../services/user.service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  hide : boolean;
  loginForm = new FormGroup({
    email: new FormControl(''),
    password: new FormControl('')
  });

  constructor(private router: Router,
    private userService: UserService) { }

  ngOnInit(): void {
  }

  get email() { return this.loginForm.get('email'); }

  login(): void {
    var loginData = this.loginForm.value;
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
