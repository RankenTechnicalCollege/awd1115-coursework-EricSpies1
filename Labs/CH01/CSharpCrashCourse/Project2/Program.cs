Console.WriteLine("What is the upper limit of our Fizz Buzz?");
int upperLimit = Convert.ToInt32(Console.ReadLine());

for (int i = 1; i <= upperLimit; i++)
{
    if(i % 3 == 0 && i % 5 == 0)
    {
        Console.WriteLine("Fizzbuzz");
    }else if(i % 3 == 0)
    {
        Console.WriteLine("Fizz");
    }else if(i % 5 == 0)
    {
        Console.WriteLine("Buzz");
    }
    else
    {
        Console.WriteLine($"{i}");
    }


}