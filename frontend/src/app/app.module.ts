import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { GameComponent } from './game/game.component';
import { TextwindowComponent } from './textwindow/textwindow.component';
import { VerbsComponent } from './verbs/verbs.component';
import { NavigationComponent } from './navigation/navigation.component';
import { CharacterComponent } from './character/character.component';
import { InventoryComponent } from './inventory/inventory.component';

@NgModule({
  declarations: [
    AppComponent,
    GameComponent,
    TextwindowComponent,
    VerbsComponent,
    NavigationComponent,
    CharacterComponent,
    InventoryComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
