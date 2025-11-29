import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TransactionsEdit } from './transactions-edit';

describe('TransactionsEdit', () => {
  let component: TransactionsEdit;
  let fixture: ComponentFixture<TransactionsEdit>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TransactionsEdit]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TransactionsEdit);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
