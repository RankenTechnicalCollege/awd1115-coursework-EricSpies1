using Project9;

Leaf leaf = new();
Pancake pancake = new();
Corner corner = new();
Page page = new();

List<ITurnable> turnables = new List<ITurnable> { leaf, pancake, corner, page };

static void Turning(List<ITurnable> t)
{
    foreach (ITurnable turn in t)
    {
        Console.WriteLine(turn.Turn());
    }
}

Turning(turnables);