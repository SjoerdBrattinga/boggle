import { Component, OnInit } from '@angular/core';
import { BoggleService } from '../boggle.service';
import { BoggleBox } from '../boggle-box';
import { BoggleGame } from '../boggle-game';

@Component({
  selector: 'app-boggle',
  templateUrl: './boggle.component.html',
  styleUrls: ['./boggle.component.css'],
  providers: [BoggleService]
})
export class BoggleComponent implements OnInit {

  boggleBox: BoggleBox;
  boggleGame: BoggleGame;
  foundWords: string[] = [];  
  playing: boolean = false;

  constructor(private _boggleService: BoggleService) { }

  ngOnInit() {
    this.boggleGame = new BoggleGame();    
  }

  startGame(): void{     
    if(this.boggleBox == null){
      this._boggleService.getBoggleBoard().subscribe((response) => {               
        this.boggleBox = response;
        this.boggleGame.boggleBoxId = this.boggleBox.boggleBoxId;
        this.playing = true; 
      });         
    } else {
      this.playing = true;  
    }  
  }

  replayGame(boggleBoxId: string): void{    
    this._boggleService.getBoggleBoardById(boggleBoxId).subscribe((reponse) => {
      this.boggleBox = reponse;          
      this.boggleGame.boggleBoxId = this.boggleBox.boggleBoxId;
      
      this.startGame();
    })
  }
  
  wordFound(word: string): void{
    if(!this.foundWords.includes(word)){
      this.foundWords.push(word);
      this._boggleService.getWordScore(word).subscribe((response) => {
        this.boggleGame.score += response;
      });
    }      
  }

  timerStopped(event): void{     
    this._boggleService.createBoggleGame(this.boggleGame).subscribe((response) => {
      console.log(response);
    });

    this.resetValues();     
  }

  resetValues(): void{
    this.playing = false;
    this.boggleBox = null;
    this.foundWords = [];
    this.boggleGame = new BoggleGame(); 
  }
}
