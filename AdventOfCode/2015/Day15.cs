using System;

namespace AdventOfCode._2015;

public class Day15 : ISolution
{
    private static readonly string filePath = $"lib\\2015\\Day15-input.txt";
    private static readonly string inputText = File.ReadAllText(filePath);

    private class Ingredient(string name, int capacity, int durability, int flavor, int texture, int calories)
    {
        public string Name { get; init; } = name;
        public int Capacity { get; init; } = capacity;
        public int Durability { get; init; } = durability;
        public int Flavor { get; init; } = flavor;
        public int Texture { get; init; } = texture;
        public int Calories { get; init; } = calories;
    }

    private static Dictionary<string, Ingredient> Init()
    {
        Dictionary<string, Ingredient> ingredients = [];
        string[] lines = inputText.Split(Environment.NewLine);
        foreach (string s in lines)
        {
            string line = s.Replace(",", "").Replace(":", "");
            string[] tokens = line.Split(' ');

            string name = tokens[0];
            int capacity = int.Parse(tokens[2]);
            int durability = int.Parse(tokens[4]);
            int flavor = int.Parse(tokens[6]);
            int texture = int.Parse(tokens[8]);
            int calories = int.Parse(tokens[10]);

            ingredients.Add(name, new Ingredient(name, capacity, durability, flavor, texture, calories));
        }
        return ingredients;
    }

    private static (Dictionary<string, int>, int) CalculateBestCookie(Dictionary<string, Ingredient> ingredients, bool isCalorieExact = false, int calorieAmount = 0)
    {
        int totalTsp = 100;
        List<string> names = [.. ingredients.Keys];
        int bestScore = 0;
        Dictionary<string, int> bestCombo = [];

        for (int a = 0; a < totalTsp; a++)
        {
            for (int b = 0; b < totalTsp - (a); b++) 
            {
                for (int c = 0; c < totalTsp - (a + b); c++)
                {
                    int d = totalTsp - (a + b + c);
                    Dictionary<string, int> combo = new()
                    {
                        { names[0], a },
                        { names[1], b },
                        { names[2], c },
                        { names[3], d }
                    };

                    int calories = 0;
                    foreach (string s in names) 
                    {
                        calories += ingredients[s].Calories * combo[s];
                    }
                    if ((isCalorieExact is true && calories == calorieAmount) || isCalorieExact is false)
                    {
                        int capacityScore = 0;
                        int durabilityScore = 0;
                        int flavorScore = 0;
                        int textureScore = 0;

                        foreach (string s in names) 
                        {
                            capacityScore += ingredients[s].Capacity * combo[s];
                            durabilityScore += ingredients[s].Durability * combo[s];
                            flavorScore += ingredients[s].Flavor * combo[s];
                            textureScore += ingredients[s].Texture * combo[s];
                        }

                        int score = (capacityScore < 0 || durabilityScore < 0 || flavorScore < 0 || textureScore < 0) ? 0 : capacityScore * durabilityScore * flavorScore * textureScore;

                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestCombo = combo;
                        }
                    }
                }
            }
        }
        return (bestCombo, bestScore);
    }

    public string Answer()
    {
        Dictionary<string, Ingredient> ingredients = Init();

        // part 1
        (Dictionary<string, int> teaspoons1, int score1) = CalculateBestCookie(ingredients);

        // part 2
        (Dictionary<string, int> teaspoons2, int score2) = CalculateBestCookie(ingredients, true, 500);

        return $"the best cookie made from {string.Join(", ", teaspoons1)} = {score1} and the best cookie with 500 calories is made from {string.Join(", ", teaspoons2)} = {score2}";
    }

}
