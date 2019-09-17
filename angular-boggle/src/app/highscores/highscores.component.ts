import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { BoggleService } from '../boggle.service';
import { BoggleGame } from '../boggle-game';


@Component({
  selector: 'app-highscores',
  templateUrl: './highscores.component.html',
  styleUrls: ['./highscores.component.css'],
  providers: [BoggleService]
})
export class HighscoresComponent implements OnInit {

  @Output() 
  gameSelected: EventEmitter<string> = new EventEmitter<string>();

  highscores: BoggleGame[];
  game: BoggleGame;

  constructor(private _boggleService: BoggleService) { }

  ngOnInit() {  
    this._boggleService.getHighscores().subscribe((response) => {
      this.highscores = response;      
    });
  }

  playSelectedGame(index){    
    this.game = this.highscores[index];    
    this.gameSelected.emit(this.game.boggleBoxId);
  }
}
