export interface Player {
    player: string;
    team: string;
    position: string;
    attempts: number;
    attempts_game: number;
    yards: number;
    average: number;
    yards_game: number;
    td: number;
    longest: string | number;
    first: number;
    first_pct: number;
    twenty_plus: number;
    forty_plus: number;
    fumbles: number;
}
