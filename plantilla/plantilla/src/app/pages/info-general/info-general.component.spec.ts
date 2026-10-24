import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InfoGeneralComponent } from './info-general.component';

describe('InfoGeneralComponent', () => {
  let component: InfoGeneralComponent;
  let fixture: ComponentFixture<InfoGeneralComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InfoGeneralComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InfoGeneralComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
