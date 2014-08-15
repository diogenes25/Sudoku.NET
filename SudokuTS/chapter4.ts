function addSupport<T extends EmailAddresses>(contact: T) : T
{
	for (var n = 0; n < contact.emails.length; n++)
	{
		if (contact.emails[n] === "huhu")
		{
			return contact;
		}
	}
	contact.emails.push("Tach");
	return contact;
}

interface EmailAddresses
{
	emails: Array<string>;
}

class Contact3 implements EmailAddresses
{
	constructor(public firstname: string, public lastname: string, ...public emails: Array<string>)
	{
	}
}

var c1 = new Contact3("Onnen", "Tjark", "tjark@onnen.de", "mas@schmeling");

class Employee implements EmailAddresses
{
	constructor(public firstname: string, ...public emails: Array<string>) { }
}

var e1 = new Employee("Onnen", "tjarkqonnen");
e1 = addSupport(e1);