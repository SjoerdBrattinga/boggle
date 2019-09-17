import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BoggleComponent } from './boggle.component';

describe('BoggleComponent', () => {
  let component: BoggleComponent;
  let fixture: ComponentFixture<BoggleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BoggleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BoggleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
