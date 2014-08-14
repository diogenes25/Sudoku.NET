var heuteSchoenesWetter: string;
//heuteSchoenesWetter = true;
heuteSchoenesWetter = "klar";
//heuteSchoenesWetter = 1;
enum Orte { Hamburg = 4, Stuttgart= 6, Kpln };
var result = Orte.Hamburg;

function Halllo(): void {
	console.log("hier passiert nichts");
	//return "HAllo";
}

function PrintName(last: string, first: string, ...age: number[]): string {
	//function PrintName(last: string, first: string, age: number = 18): string {
	//function PrintName(last: string, first: string, age?: number): string {
	return first + " " + last + " Alter:" + age.join();
}

PrintName("123", "tjark", 1, 3, 5, 7, 9);

var Contact = {

	lastname: "Onnen",
	firstname: "Tjark",

	getFullName: function () {
		return () => {
			return this.firstname + " " + this.lastname;
		}
	}
}