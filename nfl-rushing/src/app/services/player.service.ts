import { Injectable } from '@angular/core';

import { HttpClient, HttpParams } from  '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { Player } from '../models/player';


@Injectable({
  providedIn: 'root'
})
export class PlayerService {

  private playersApi: string = 'https://localhost:5001';

  private requiredFields = [
      "Player", "team", "Pos", "Att", "Att/G", "Yds", "Avg", "Yds/G",
      "TD", "Lng", "1st", "1st%", "20+", "40+", "FUM"
  ];

  constructor(private http: HttpClient) { }

  getPlayers(filter = '', sortOrder = 'asc', pageNumber = 0, pageSize = 10): Observable<Player[]>
  {
    return this.http.get<Object[]>(`${this.playersApi}/players`, {
      params: new HttpParams()
      .set('filter', filter)
      .set('sortOrder', sortOrder)
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString())
    }).pipe(
      map((res: Object[]) => {
        let players: Player[] = [];

        res.forEach(val => {
          if (!this.validateHttpPlayer(val))
          {
            // Skip this player
            console.error(`Error parsing player: ${JSON.stringify(val)}`)
            return;
          }

          players.push(this.parseHttpPlayer(val));
        })

        return players;
      })
    );
  };

  private validateHttpPlayer(httpPlayer: any): boolean {
    let result = true;

    this.requiredFields.forEach(field => {
      if (!(field in httpPlayer))
      {
        result = false;
        return;
      }
    })

    return result;
  }

  private parseHttpPlayer(httpPlayer: any): Player {
    let player: Player = {
      name: httpPlayer["Player"],
      team: httpPlayer["team"],
      position: httpPlayer["Pos"],
      attempts: httpPlayer["Att"],
      attempts_game: httpPlayer["Att/G"],
      yards: httpPlayer["Yds"],
      average: httpPlayer["Avg"],
      yards_game: httpPlayer["Yds/G"],
      touchdowns: httpPlayer["TD"],
      longest: httpPlayer["Lng"],
      first: httpPlayer["1st"],
      first_pct: httpPlayer["1st%"],
      twenty_plus: httpPlayer["20+"],
      forty_plus: httpPlayer["40+"],
      fumbles: httpPlayer["FUM"]
    };

    return player;
  }
}
