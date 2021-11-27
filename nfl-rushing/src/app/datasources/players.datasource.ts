
import { CollectionViewer, DataSource } from '@angular/cdk/collections';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { catchError, finalize, tap } from 'rxjs/operators';

import { Player } from '../models/player'
import { PlayerService } from '../services/player.service';

export class PlayersDataSource implements DataSource<Player> {

    private playersSubject = new BehaviorSubject<Player[]>([]);
    private loadingSubject = new BehaviorSubject<boolean>(false);
    private countSubject = new BehaviorSubject<number>(0);

    public loading$ = this.loadingSubject.asObservable();
    public count$ = this.countSubject.asObservable();

    constructor(private playerService: PlayerService) {}

    connect(collectionViewer: CollectionViewer): Observable<Player[]> {
        return this.playersSubject.asObservable();
    }

    disconnect() {
        this.playersSubject.complete();
        this.loadingSubject.complete();
        this.countSubject.complete();
    }

    loadPlayers(filter = '', sortField = 'yards', sortDirection = 'desc', pageIndex = 0, pageSize = 10) {
            this.loadingSubject.next(true);

            this.playerService.getPlayers(filter, sortField, sortDirection, pageIndex, pageSize)
            .pipe(
                catchError(() => of([])),
                tap( (players: Player[]) => this.countSubject.next(players.length)),
                finalize(() => this.loadingSubject.next(false))
            )
            .subscribe((players: any) => this.playersSubject.next(players));
    }
}