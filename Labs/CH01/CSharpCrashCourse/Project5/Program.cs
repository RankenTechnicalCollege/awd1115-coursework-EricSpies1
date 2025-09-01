string ccNum;
do
{
    Console.WriteLine("Enter a CC Num");
    ccNum = Console.ReadLine();
} while (String.IsNullOrEmpty(ccNum));

string maskedNum = String.Empty;

for (int index = 0; index < ccNum.Length; index++)
{
    if (ccNum[index] == '-' || Char.IsWhiteSpace(ccNum[index]) || index >= ccNum.Length - 4)
        maskedNum += ccNum[index];
    else
        maskedNum += 'X';
}

Console.WriteLine(maskedNum);