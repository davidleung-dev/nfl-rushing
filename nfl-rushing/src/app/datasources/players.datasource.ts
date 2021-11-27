
import { CollectionViewer, DataSource } from '@angular/cdk/collections';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { catchError, finalize, tap } from 'rxjs/operators';

import { Player } from '../models/player'
import { PlayerService } from '../services/player.service';

export class PlayersDataSource implements DataSource<Player> {

    private playersSubject = new BehaviorSubject<Player[]>([]);
    private loadingSubject = new BehaviorSubject<boolean>(false);

    public loading$ = this.loadingSubject.asObservable();
    public count$ = this.playerService.count$;

    constructor(private playerService: PlayerService) {}

    connect(collectionViewer: CollectionViewer): Observable<Player[]> {
        return this.playersSubject.asObservable();
    }

    disconnect() {
        this.playersSubject.complete();
        this.loadingSubject.complete();
    }

    loadPlayers(filter = '', sortField = 'yards', sortDirection = 'desc', pageIndex = 0, pageSize = 10) {
            this.loadingSubject.next(true);

            this.playerService.getPlayers(filter, sortField, sortDirection, pageIndex, pageSize)
            .pipe(
                catchError(() => of([])),
                finalize(() => this.loadingSubject.next(false))
            )
            .subscribe((players: any) => this.playersSubject.next(players));
    }
}