import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  hide : boolean;

  constructor(private router: Router, private userService: UserService) { }

  ngOnInit(): void {
  }

  login(): void {
    //authenticate, post to api/users/authenticate
    this.userService.authenticate().subscribe(
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
