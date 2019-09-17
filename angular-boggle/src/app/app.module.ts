import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms'; 
import { HttpModule } from '@angular/http';
import { HttpClientModule }    from '@angular/common/http';

import { AppComponent } from './app.component';
import { BoggleComponent } from './boggle/boggle.component';
import { BoardComponent } from './board/board.component';
import { ScoreComponent } from './score/score.component';
import { HighscoresComponent } from './highscores/highscores.component';
import { TimerComponent } from './timer/timer.component';
import { FormatTimePipe } from './timer/format-time.pipe';


@NgModule({
  declarations: [
    AppComponent,
    BoggleComponent,
    BoardComponent,
    ScoreComponent,
    HighscoresComponent,
    TimerComponent,
    FormatTimePipe
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
