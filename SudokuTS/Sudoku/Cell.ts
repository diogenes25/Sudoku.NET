module Sudoku
{
	export class Cell
	{
		private _digit: number;

		get Digit(): number
		{
			return this._digit;
		}

	}
} 

var cx = new Sudoku.Cell();
console.log(cx.Digit);