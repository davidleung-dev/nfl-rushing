
import { CollectionViewer, DataSource } from '@angular/cdk/collections';
import { BehaviorSubject, of } from 'rxjs';
import { Observable } from 'rxjs/dist/types/internal/Observable';
import { catchError, finalize } from 'rxjs/dist/types/operators';

import { Player } from '../models/player'
import { PlayerService } from '../services/player.service';

export class PlayersDataSource implements DataSource<Player> {

    private playersSubject = new BehaviorSubject<Player[]>([]);
    private loadingSubject = new BehaviorSubject<boolean>(false);

    constructor(private playerService: PlayerService) {}

    connect(collectionViewer: CollectionViewer): Observable<Player[]> {
        return this.playersSubject.asObservable();
    }

    disconnect() {
        this.playersSubject.complete();
        this.loadingSubject.complete();
    }

    loadPlayers(filter = '', sortDirection = 'desc',
        pageIndex = 0, pageSize = 10) {
            this.loadingSubject.next(true);

            this.playerService.getPlayers(filter, sortDirection, pageIndex, pageSize)
            .pipe(
                catchError(() => of([])),
                finalize(() => this.loadingSubject.next(false))
            )
            .subscribe((players: Player[]) => this.playersSubject.next(players));
    }
}