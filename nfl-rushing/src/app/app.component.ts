import { Component, ViewChild } from '@angular/core';
import { PlayerTableComponent } from './components/player-table/player-table.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  @ViewChild(PlayerTableComponent) playerComp: PlayerTableComponent;

}
