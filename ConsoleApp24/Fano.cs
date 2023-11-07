using System;
using System.Collections.Generic;
using System.Linq;

public class ShannonFano
{
    public static List<Tuple<char, string, double>> Encode(string input, List<double> probabilities)
    {
        if (input.Length != probabilities.Count)
        {
            throw new ArgumentException("Input length and probabilities count must match.");
        }

        List<Tuple<char, string, double>> symbols = new List<Tuple<char, string, double>>();

        for (int i = 0; i < input.Length; i++)
        {
            symbols.Add(new Tuple<char, string, double>(input[i], "", probabilities[i]));
        }

        symbols = symbols.OrderByDescending(x => x.Item3).ToList();
        EncodeRecursive(symbols, 0, symbols.Count - 1);

        return symbols;
    }

    private static void EncodeRecursive(List<Tuple<char, string, double>> symbols, int start, int end)
    {
        if (start == end)
        {
            return;
        }
        //Сума всіх ймовірностей символів у поточному діапазоні
        double sumProbabilities = symbols.GetRange(start, end - start + 1).Sum(x => x.Item3);
        //Середнє значення суми ймовірностей
        double halfSum = sumProbabilities / 2.0;
        double currentSum = 0.0;
        int splitIndex = -1;
        //Починаючи з початковго символу, ми шукаємо символ, для якого сума ймовірностей до нього перевищує це середнє значення. На цьому символі і буде точка розділення
        for (int i = start; i <= end; i++)
        {
            currentSum += symbols[i].Item3;
            if (currentSum >= halfSum)
            {
                splitIndex = i;
                break;
            }
        }
        //Присвоюємо коди символам: символам до точки розділення додаємо "0", а символам після точки розділення додаємо "1"
        for (int i = start; i <= end; i++)
        {
            if (i <= splitIndex)
            {
                symbols[i] = new Tuple<char, string, double>(symbols[i].Item1, symbols[i].Item2 + "0", symbols[i].Item3);
            }
            else
            {
                symbols[i] = new Tuple<char, string, double>(symbols[i].Item1, symbols[i].Item2 + "1", symbols[i].Item3);
            }
        }
        //Рекурсивно викликаємо нашу функцію для лівої і правої частини
        EncodeRecursive(symbols, start, splitIndex);
        EncodeRecursive(symbols, splitIndex + 1, end);
    }

    public static void Main(string[] args)
    {
        string input = "ABCDEFGHI";
        List<double> probabilities = new List<double> { 0.26, 0.14, 0.05, 0.10, 0.07, 0.11, 0.02, 0.20, 0.05 };

        List<Tuple<char, string, double>> codes = Encode(input, probabilities);

        Console.WriteLine("Character Codes:");
        foreach (var tuple in codes)
        {
            Console.WriteLine($"{tuple.Item1}: {tuple.Item2} (Probability: {tuple.Item3})");
        }
    }
}
