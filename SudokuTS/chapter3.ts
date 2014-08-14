
var theContact = {
    firstname: "Tjark",
    lastname: "Onnen",
    //age: 41
};

function printContact(contact: IContact): string {
    if (contact.age) {
        return contact.lastname + ", " + contact.firstname + " AGE:" + contact.age;
    } else {
        return contact.lastname + ", " + contact.firstname;
    }
}

printContact(theContact);

interface IContact {
    firstname: string;
    lastname: string;
    age?: number

}

interface IPrintContact {
    (contact: IContact): string;
}

var contactPrinter: IPrintContact;

contactPrinter = function (contact: IContact): string {
    return contact.lastname + " " + contact.lastname;
}

interface IContactArray {
    [index: number]: IContact;
}

var contactArray: IContactArray;
contactArray = [theContact];

interface IPrintCurrentContact {
    print(): string;
}

class CContact implements IContact, IPrintCurrentContact {
    firstname: string;
    lastname: string;

    constructor(first: string, last: string) {
        this.firstname = first;
        this.lastname = last;
    }

    print(): string {
        return this.lastname + " " + this.firstname;
    }
}

var c = new CContact("Max", "Müerller");
c.print();

class ContactWithAge extends CContact {
    age: number;

    constructor(first: string, last: string, age: number) {
        super(first, last);
        this.age = age;
    }
}

var n = 0;

class Game {
    static copyright = "XXX";
    //title: string;
    //publisher: string;

    private copyAvailable: boolean;
    constructor(public title: string, public publisher: string) {
    }

    get copy(): boolean {
        if (this.title === "hallo") {
            return true;
        } else {
            return this.copyAvailable;
        }
    }

    set copy(value: boolean) {
        this.copyAvailable = value;
    }
}

var g = new Game("hallo", "ich");
var cr = Game.copyright;
//g.copyAvailable = true;