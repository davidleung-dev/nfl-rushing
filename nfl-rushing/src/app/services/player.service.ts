import { Injectable } from '@angular/core';

import { HttpClient, HttpParams } from  '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { Player } from '../models/player';

const DUMMY_DATA: Player[] = [
  {
    player: "Lance Dunbar",
    team: "DAL",
    position: "RB",
    attempts: 9,
    attempts_game: 0.7,
    yards: 31,
    average: 3.4,
    yards_game: 2.4,
    touchdowns: 1,
    longest: "10",
    first: 3,
    first_pct: 33.3,
    twenty_plus: 0,
    forty_plus: 0,
    fumbles: 0
  },
  {
    player:"Mark Ingram",
    team:"NO",
    position:"RB",
    attempts:205,
    attempts_game:12.8,
    yards:1043,
    average:5.1,
    yards_game:65.2,
    touchdowns:6,
    longest:"75T",
    first:49,
    first_pct:23.9,
    twenty_plus:4,
    forty_plus:2,
    fumbles:2
  },
  {
    player:"Reggie Bush",
    team:"BUF",
    position:"RB",
    attempts:12,
    attempts_game:0.9,
    yards:-3,
    average:-0.3,
    yards_game:-0.2,
    touchdowns:1,
    longest:5,
    first:2,
    first_pct:16.7,
    twenty_plus:0,
    forty_plus:0,
    fumbles:1
  }
];

@Injectable({
  providedIn: 'root'
})
export class PlayerService {

  constructor(private http: HttpClient) { }

  getPlayers(filter = '', sortOrder = 'asc', pageNumber = 0, pageSize = 10): Observable<Player[]>
  {
    return of(DUMMY_DATA);

    //return this.http.get('/api/players', {
    //  params: new HttpParams()
    //  .set('filter', filter)
    //  .set('sortOrder', sortOrder)
    //  .set('pageNumber', pageNumber.toString())
    //  .set('pageSize', pageSize.toString())
    //}).pipe(
    //  map(res => res["payload"])
    //);
  };
}
