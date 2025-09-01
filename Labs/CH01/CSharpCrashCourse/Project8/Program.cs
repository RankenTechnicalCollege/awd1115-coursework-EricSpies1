using Project8;



Cart cart1 = new Cart("1234");
cart1.AddItem("Lollypop", 2.5);
cart1.AddItem("Gum", 1.5);
cart1.AddItem("Soda", 3.75);
Console.WriteLine(cart1);

cart1.RemoveItem("Gum");
Console.WriteLine(cart1);

Cart cart2 = new Cart("5678");
cart2.AddItem("Milk", 2.5);
cart2.AddItem("Bread", 1.5);
cart2.AddItem("Eggs", 3.75);
Console.WriteLine(cart2);
