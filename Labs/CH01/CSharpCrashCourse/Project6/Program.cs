int[] testScores = [100, 90, 30, 88, 75, 93];
int bestScore = 0;
int worstScore = 100;
int sum = 0;
for (int i = 0; i < testScores.Length; i++)
{
    if (testScores[i] > bestScore)
    {
        bestScore = testScores[i];
    }
    if (testScores[i] < worstScore)
    {
        worstScore = testScores[i];
    }
    sum += testScores[i];
}
double average = sum / testScores.Length;
Console.WriteLine($"Best score: {bestScore}");
Console.WriteLine($"Worst score: {worstScore}");
Console.WriteLine($"Average score: {average}");
Console.WriteLine($"Sum of scores: {sum}");