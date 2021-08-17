import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FaceReaderComponent } from './face-reader.component';

describe('FaceReaderComponent', () => {
  let component: FaceReaderComponent;
  let fixture: ComponentFixture<FaceReaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FaceReaderComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FaceReaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
