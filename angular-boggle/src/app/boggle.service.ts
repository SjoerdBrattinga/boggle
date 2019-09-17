import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'; 
import { Observable } from 'rxjs';
import { of } from 'rxjs/observable/of';
import { catchError, map, tap } from 'rxjs/operators';

import { BoggleBox } from './boggle-box';
import { BoggleGame } from './boggle-game';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()
export class BoggleService {
  constructor(private http: HttpClient) {}

  getBoggleBoard (): Observable<BoggleBox> {
    return this.http.get<BoggleBox>("http://localhost:52356/api/BoggleBox/GetBoggleBox")
  }

  createBoggleGame (game: BoggleGame): Observable<BoggleGame> {
    return this.http.post<BoggleGame>("http://localhost:52356/api/BoggleBox/CreateBoggleGame", game, httpOptions).pipe(
      tap((game:BoggleGame) => this.log(`BoggleGame Created w/ id=${game.boggleBoxId}`)),
        catchError(this.handleError<BoggleGame>('createBoggleGame'))
    );
  }

  getBoggleBoardById(boggleBoxId:string): Observable<BoggleBox>{      
    return this.http.get<BoggleBox>(`http://localhost:52356/api/BoggleBox/GetBoggleBox/${boggleBoxId}`, httpOptions).pipe(
      tap((result: BoggleBox) => this.log(`Word validated`)),
        catchError(this.handleError<BoggleBox>('getBoggleBoxById'))
    );      
  }

  validateWord(boggleBoxId:string, word:string): Observable<boolean>{      
    return this.http.get<boolean>(`http://localhost:52356/api/BoggleBox/IsValidWord/${boggleBoxId}/${word}`, httpOptions).pipe(
      tap((result: boolean) => this.log(`Word validated`)),
        catchError(this.handleError<boolean>('validateWord'))
    );      
  }

  getWordScore(word:string): Observable<number>{      
    return this.http.get<number>(`http://localhost:52356/api/BoggleBox/ScoreWord/${word}`, httpOptions).pipe(
      tap((result: number) => this.log(`Word validated`)),
        catchError(this.handleError<number>('getWordScore'))
    );      
  }

  getHighscores(): Observable<BoggleGame[]>{      
    return this.http.get<BoggleGame[]>("http://localhost:52356/api/BoggleBox/GetHighscores", httpOptions).pipe(
      tap((result: BoggleGame[]) => this.log(`Word validated`)),
        catchError(this.handleError<BoggleGame[]>('getHighscores'))
    );      
  }

  private handleError<T> (operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {     
      console.error(error);
      this.log(`${operation} failed: ${error.message}`);
      return of(result as T);
    };
  }
 

  private log(message: string) {
   
  }

}