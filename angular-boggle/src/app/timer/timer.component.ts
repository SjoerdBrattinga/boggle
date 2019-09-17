import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Observable, Subscription } from 'rxjs/Rx';
import { take, map } from 'rxjs/operators';

@Component({
  selector: 'app-timer',
  templateUrl: './timer.component.html',
  styleUrls: ['./timer.component.css']
})
export class TimerComponent implements OnInit {
  
  @Output() 
  timerStopped: EventEmitter<string> = new EventEmitter<string>();

  subscription: Subscription;
  count: number = 3 * 60;
 
  constructor() { }

  ngOnInit() {
    this.startTimer();    
  }

  startTimer():void{
    let countDownTimer = Observable.timer(1000,1000);
    
    this.subscription = countDownTimer.subscribe(t => {   
      this.count--;

      if(this.count == 0)
        this.stopTimer();
    });  
  }

  stopTimer():void{    
    this.subscription.unsubscribe();    
    this.timerStopped.emit('stopped');
  }  

  ngOnDestroy(){
    this.subscription.unsubscribe();
  }
}

