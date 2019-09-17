import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Die }  from '../die';
import { BoggleService } from '../boggle.service';
import * as _ from 'lodash';
import { BoggleBox } from '../boggle-box';

@Component({
  selector: 'app-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.css'],
  providers: [BoggleService]
})
export class BoardComponent implements OnInit {
  @Input()
  boggleBox: BoggleBox;

  @Output() 
  wordFound: EventEmitter<string> = new EventEmitter<string>();
  
  selectedDie: Die;  
  rows: number = 4; 
  board: Die[][];
  word: string = '';
  usedDice: Die[];  

  constructor(private _boggleService: BoggleService) { 
    this.board = [];  
    this.usedDice = [];  
  }  

  ngOnInit() {    
    this.board = this.boggleBox.dice;
    console.log(this.board);

    for(let i = 0; i < this.boggleBox.dice.length; i++){
      for(let j = 0; j < this.boggleBox.dice.length; j++){  
        this.board[i][j].id = i * 4 + j;
        this.board[i][j].selected = false;
      }
    }
  }

  onDieClick(die:Die): void {   
    this.selectedDie = die;
    this.word = '';
    let validDie: boolean;
    
    if(this.selectedDie.selected){
      if (this.selectedDie.id == this.usedDice[this.usedDice.length - 1].id){
        this.selectedDie.selected = false; 
        this.usedDice.splice(-1,1); 
      }
    } else {
      if(this.usedDice.length == 0){
        validDie = true;  
      } else {
        validDie  = this.checkIfDieCanBeUsed();
      }
      if(validDie){        
        this.selectedDie.selected = true; 
        this.usedDice.push(this.selectedDie);        
      } 
    }
    
    for(let i = 0; i < this.usedDice.length; i++){
      this.word += this.usedDice[i].value;
    }  
  }

  checkIfDieCanBeUsed(): boolean {
    if(!this.selectedDie.selected){
      if(this.checkIfDiceAreConnected()){
        if(this.checkIfDieIsAlreadyUsed()){        
          console.log('this die is already used');
          return false;
        } else {
          return true;
        }
      }  
    }
    return false;
  }

  checkIfDiceAreConnected(): boolean {   
    let lastUsedDieIndex: number = this.usedDice[this.usedDice.length-1].id;
    let connectedDice = this.getConnectedDice(lastUsedDieIndex);

    if(connectedDice.includes(this.selectedDie.id)){      
      return true
    }      

    return false;
  }

  checkIfDieIsAlreadyUsed(): boolean { 
    return _.includes(this.usedDice,this.selectedDie);
  }

  getConnectedDice(index): number[]{
    let connectedDice = [];

    switch (index) {
      case 0: connectedDice.push(1, 4, 5);                      break;
      case 1: connectedDice.push(0, 2, 4, 5, 6);                break;
      case 2: connectedDice.push(1, 3, 5, 6, 7);                break;
      case 3: connectedDice.push(2, 6, 7);                      break;
      case 4: connectedDice.push(0, 1, 5, 8, 9);                break;
      case 5: connectedDice.push(0, 1, 2, 4, 6, 8, 9, 10);      break;
      case 6: connectedDice.push(1, 2, 3, 5, 7, 9, 10, 11);     break;
      case 7: connectedDice.push(2, 3, 6, 10, 11);              break;
      case 8: connectedDice.push(4, 5, 9, 12, 13);              break;
      case 9: connectedDice.push(4, 5, 6, 8, 10, 12, 13, 14);   break;
      case 10: connectedDice.push(5, 6, 7, 9, 11, 13, 14, 15);  break;
      case 11: connectedDice.push(6, 7, 10, 14, 15);            break;
      case 12: connectedDice.push(8, 9, 13);                    break;
      case 13: connectedDice.push(8, 9, 10, 12, 14);            break;
      case 14: connectedDice.push(9, 10, 11, 13, 15);           break;
      case 15: connectedDice.push(10, 11, 14);                  break; 
      default: connectedDice.push();                            break;      
    }

    return connectedDice;
  }

  submitWord(): void{   
    let valid: boolean; 
    
    if(this.word.length < 3) return;

    this._boggleService.validateWord(this.boggleBox.boggleBoxId, this.word).subscribe((response) => {                        
      valid = response;
      console.log(response);

      if(valid){
        this.wordFound.emit(this.word);    
      }

      this.clearWord();
    });   
  }

  clearWord(): void{
    this.usedDice = [];
    this.word = '';
    
    for(let i = 0; i < this.board.length; i++){
      for(let j=0; j < this.board.length;j++){
        this.board[i][j].selected = false;
      }
    }    
  }
}

