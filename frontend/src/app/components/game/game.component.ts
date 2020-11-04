import { Component, OnInit, HostListener } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AdventureService } from '../../services/adventure.service';

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.scss']
})
export class GameComponent implements OnInit {
  adventure: string;

  constructor(private route: ActivatedRoute,
    private router: Router,
    private adventureService: AdventureService) { }

  ngOnInit(): void {
    this.route.params.subscribe( params => {
      this.adventure = params['adventure'];
    });

    //I would like for people to be able to just click a link and be in the game
    //so this could be one of the most used entry points

    //check if we have an adventure and it's valid,
    //if not we'll kick back to console, eventually let them pick from a dialog box
    //have to make a get request to see if an adventure exists with this name
    console.log(this.adventure);
    this.adventureService.getAdventures().subscribe(adv => console.log(adv));

    //contact the games service to see if they've started this game,
    //if so pick up from where they left off
    //otherwise post a new game for this adventure

    //for this skeleton we're going to assume they've started the game already
  }
}
