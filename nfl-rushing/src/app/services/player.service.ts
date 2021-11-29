import { Injectable } from '@angular/core';

import { HttpClient, HttpParams } from  '@angular/common/http';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { LongestRush, Player } from '../models/player';


@Injectable({
  providedIn: 'root'
})
export class PlayerService {

  private playersApi: string = 'http://localhost:5001';

  private playerRequiredFields = [
      "Player", "team", "Pos", "Att", "Att/G", "Yds", "Avg", "Yds/G",
      "TD", "Lng", "1st", "1st%", "20+", "40+", "FUM"
  ];

  private longestRushRequiredFields = [
    "yards", "touchdown"
  ];

  private countSubject = new BehaviorSubject<number>(0);

  public count$ = this.countSubject.asObservable();

  constructor(private http: HttpClient) { }

  ngOnDestroy() {
    console.log("Destroying service");
    this.countSubject.complete();
  }

  getPlayers(filter: string, sortField: string , sortOrder: string, pageNumber: number, pageSize:number): Observable<Player[]>
  {
    return this.http.get<Object[]>(`${this.playersApi}/players`, {
      params: new HttpParams()
      .set('filter', filter)
      .set('sortField', sortField)
      .set('sortOrder', sortOrder)
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString())
    }).pipe(
      tap((res: any) => this.countSubject.next(res["totalPlayerCount"])),
      map((res: any) => res["players"]),
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

  downloadCsv(filter: string, sortField: string, sortOrder: string, pageIndex: number, pageSize: number): Observable<Blob>
  {
    return this.http.get(`${this.playersApi}/players/download`,
    {
      responseType: 'blob',
      params: new HttpParams()
      .set('filter', filter)
      .set('sortField', sortField)
      .set('sortOrder', sortOrder)
      .set('pageNumber', pageIndex.toString())
      .set('pageSize', pageSize.toString())
    });
  }

  private validateHttpPlayer(httpPlayer: any): boolean {
    let result = true;

    this.playerRequiredFields.forEach(field => {
      if (!(field in httpPlayer))
      {
        result = false;
        return;
      }
    })

    let lngRshObj = httpPlayer["Lng"];

    this.longestRushRequiredFields.forEach(field => {
      if (!(field in lngRshObj))
      {
        result = false;
        return;
      }
    })

    return result;
  }

  private parseHttpPlayer(httpPlayer: any): Player {
    let longestRush: LongestRush = {
      yards: httpPlayer["Lng"]["yards"],
      touchdown: httpPlayer["Lng"]["touchdown"]
    };

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
      longest: longestRush,
      first: httpPlayer["1st"],
      first_pct: httpPlayer["1st%"],
      twenty_plus: httpPlayer["20+"],
      forty_plus: httpPlayer["40+"],
      fumbles: httpPlayer["FUM"]
    };

    return player;
  }
}
