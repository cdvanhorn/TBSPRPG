import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'tbsprpg (text based single player rpg)';
  version = '0.01';
  //going to connect to the api to get a list of adventures
}
