import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { fromEvent, merge } from 'rxjs';
import { debounceTime, distinctUntilChanged, tap } from 'rxjs/operators';
import { PlayersDataSource } from 'src/app/datasources/players.datasource';
import { PlayerService } from 'src/app/services/player.service';

@Component({
  selector: 'app-player-table',
  templateUrl: './player-table.component.html',
  styleUrls: ['./player-table.component.scss']
})
export class PlayerTableComponent implements AfterViewInit, OnInit {

  displayedColumns = [
    "name",
    "team",
    "position",
    "attempts",
    "attempts_game",
    "yards",
    "average",
    "yards_game",
    "touchdowns",
    "longest",
    "first",
    "first_pct",
    "twenty_plus",
    "forty_plus",
    "fumbles"
  ];

  pageSizeOptions = [ 10, 25, 100 ];

  dataSource: PlayersDataSource;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('input') input: ElementRef;

  constructor(private playersService: PlayerService) {
    // TODO - This may need to be moved back to ngOnInit()
    this.dataSource = new PlayersDataSource(this.playersService);
  }

  ngOnInit(): void {
    this.dataSource.loadPlayers();
  }

  ngAfterViewInit() {
    fromEvent(this.input.nativeElement, 'keyup')
    .pipe(
      debounceTime(150),
      distinctUntilChanged(),
      tap(() => {
        this.paginator.pageIndex = 0;
        this.loadPlayersPage();
      })
    )
    .subscribe();

    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);

    merge(this.sort.sortChange, this.paginator.page)
      .pipe(
        tap(() => this.loadPlayersPage())
      )
      .subscribe();
  }

  loadPlayersPage() {
    this.dataSource.loadPlayers(this.input.nativeElement.value, this.sort.active, this.sort.direction, this.paginator.pageIndex, this.paginator.pageSize);
  }

  /*
      Default args (none specified) indicates return all data
      Specifying filter indicates return all data that matches filter
      Page index and page Size specified indicates return datat seen on current page (filter or no filter)
  */
  downloadCsv(filter = '', pageSize = 0) {
    this.playersService.downloadCsv(filter, this.sort.active, this.sort.direction, this.paginator.pageIndex, pageSize)
    .pipe(
      tap((resp: any) => {
        console.log(resp);
      })
    )
    .subscribe((resp: any) => {
      let blob = new Blob([resp], { type: 'text/csv' });
      let downloadUrl = URL.createObjectURL(blob);

      let a = document.createElement('a');
      a.href = downloadUrl;
      a.download = 'rushing.csv';

      a.click();

      setTimeout(() => {
        URL.revokeObjectURL(downloadUrl);
      }, 1000)
    });
  }

}
