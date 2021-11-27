export interface Player {
    name: string;
    team: string;
    position: string;
    attempts: number;
    attempts_game: number;
    yards: number;
    average: number;
    yards_game: number;
    touchdowns: number;
    longest: LongestRush;
    first: number;
    first_pct: number;
    twenty_plus: number;
    forty_plus: number;
    fumbles: number;
}

export interface LongestRush {
    yards: number;
    touchdown: boolean;
};