int rows, cols;
bool isValid;

do
{
    Console.WriteLine("how many rows should the table have?");
    isValid = int.TryParse(Console.ReadLine(), out rows);
}  while (!isValid);


do
{
    Console.WriteLine("How many columns should the table have?");
    isValid = int.TryParse(Console.ReadLine(), out cols);
} while (!isValid);

for (int row = 0; row <= rows; row++)
{
    if(row == 0)
    {
        Console.Write($"{"",6} |");
        for (int col = 1; col <= cols; col++)
        {
            Console.Write($"{col,6} |");
        }
        Console.WriteLine();
        Console.Write(new string('-', 8 * (cols + 1)));
        Console.WriteLine();
    }
    else
    {
        Console.Write($"{row,6} |");
        for (int col = 1; col <= cols; col++)
        {
            Console.Write($"{row * col,6} |");
        }
        Console.WriteLine();
    }
    
}