using System.ComponentModel.Design;

bool exit = false;
Dictionary<string, string> phoneBook = new();
phoneBook.Add("Evan", "123-456-7890");
phoneBook.Add("John", "098-765-4321");
phoneBook.Add("Jane", "321-654-0987");

do
{
    Console.WriteLine("\n 1. Add Contact \n 2. View Contact \n 3. Update Contact \n 4. Delete Contact \n 5. List All Contacts \n 6. Exit \n");
    Console.Write("Enter Choice: ");
    string? choice = Console.ReadLine();
    if (choice.Equals("6"))
    {
        exit = true;
    }
    else if (choice.Equals("1"))
    {
        Console.Write("Enter Name: ");
        string name = Console.ReadLine();
        Console.Write("Enter Phone Number: ");
        string phoneNumber = Console.ReadLine();
        phoneBook.Add(name, phoneNumber);
    }
    else if (choice.Equals("2"))
    {
        Console.Write("Enter Name: ");
        string name = Console.ReadLine();
        if (phoneBook.ContainsKey(name))
        {
            Console.WriteLine($"\n Name: {name} \n Phone Number: {phoneBook[name]}");
        }
        else
        {
            Console.WriteLine("Contact Not Found");
        }
    }
    else if (choice.Equals("3"))
    {
        Console.Write("Enter Name: ");
        string name = Console.ReadLine();
        if (phoneBook.ContainsKey(name))
        {
            Console.Write("Enter New Phone Number: ");
            string phoneNumber = Console.ReadLine();
            phoneBook[name] = phoneNumber;
        }
        else
        {
            Console.WriteLine("Contact Not Found");
        }
    }
    else if (choice.Equals("4"))
    {
        Console.Write("Enter Name: ");
        string name = Console.ReadLine();
        if (phoneBook.ContainsKey(name))
        {
            phoneBook.Remove(name);
        }
        else
        {
            Console.WriteLine("Contact Not Found");
        }
    }
    else if (choice.Equals("5"))
    {
        foreach (KeyValuePair<string, string> contact in phoneBook)
        {
            Console.WriteLine($"----------------------\n Name: {contact.Key} \n Phone Number: {contact.Value} ");
            Console.WriteLine("----------------------");
        }
    }

} while (exit == false);