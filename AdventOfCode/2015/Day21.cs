using System;
using System.Security.Cryptography;

namespace AdventOfCode._2015;

public class Day21 : ISolution
{
    private static readonly string filePath = $"lib\\2015\\Day21-input.txt";
    private static readonly string inputText = File.ReadAllText(filePath);
    private static readonly HashSet<Item> shop = InitShop();

    private enum ItemType
    {
        Weapon,
        Armor,
        Ring
    }

    private class Item(string name, ItemType type, int cost, int damage, int armor)
    {
        public string Name { get; init; } = name;
        public ItemType Type { get; init; } = type;
        public int Cost { get; init; } = cost;
        public int Damage { get; init; } = damage;
        public int Armor { get; init; } = armor;
    }

    private class Entity(string name, int hp, int damage, int armor)
    {
        public string Name { get; init; } = name;
        public int MaxHP { get; init; } = hp;
        public int HP { get; set; } = hp;
        public int Damage { get; init; } = damage;
        public int Armor { get; init; } = armor;
    }

    private static HashSet<Item> InitShop()
    {
        HashSet<Item> shop = [];
        HashSet<Item> weapons =
        [
            new Item("Dagger", ItemType.Weapon, 8, 4, 0),
            new Item("Shortsword", ItemType.Weapon, 10, 5, 0),
            new Item("Warhammer", ItemType.Weapon, 25, 6, 0),
            new Item("Longsword", ItemType.Weapon, 40, 7, 0),
            new Item("Greataxe", ItemType.Weapon, 74, 8, 0)
        ];
        shop.UnionWith(weapons);

        HashSet<Item> armor =
        [
            new Item("no armor", ItemType.Armor, 0, 0, 0),
            new Item("Leather", ItemType.Armor, 13, 0, 1),
            new Item("Chainmail", ItemType.Armor, 31, 0, 2),
            new Item("Splintmail", ItemType.Armor, 53, 0, 3),
            new Item("Bandedmail", ItemType.Armor, 75, 0, 4),
            new Item("Platemail", ItemType.Armor, 102, 0, 5)
        ];
        shop.UnionWith(armor);

        HashSet<Item> rings =
        [
            new Item("no ring1", ItemType.Ring, 0, 0, 0),
            new Item("no ring2", ItemType.Ring, 0, 0, 0),
            new Item("Damage +1", ItemType.Ring, 25, 1, 0),
            new Item("Damage +2", ItemType.Ring, 50, 2, 0),
            new Item("Damage +3", ItemType.Ring, 100, 3, 0),
            new Item("Defense +1", ItemType.Ring, 20, 0, 1),
            new Item("Defense +2", ItemType.Ring, 40, 0, 2),
            new Item("Defense +3", ItemType.Ring, 80, 0, 3)
        ];
        shop.UnionWith(rings);

        // var test = shop.Where(x => x.Type == ItemType.Weapon);
        return shop;
    }

    private static Entity InitBoss()
    {
        string[] lines = inputText.Split(Environment.NewLine);
        string[] hp = lines[0].Split(' ');
        string[] damage = lines[1].Split(' ');
        string[] armor = lines[2].Split(' ');

        Entity boss = new("boss", int.Parse(hp[2]), int.Parse(damage[1]), int.Parse(armor[1]));

        return boss;
    }

    private static int CalculateOptimalShopping(Entity player, Entity enemy, bool isOptimal = true, bool isWin = true)
    {
        HashSet<Item> weapons = shop.Where(x => x.Type == ItemType.Weapon).ToHashSet();
        HashSet<Item> armor = shop.Where(x => x.Type == ItemType.Armor).ToHashSet();
        List<Item> rings = shop.Where(x => x.Type == ItemType.Ring).ToList();
        int finalGold = isOptimal ? int.MaxValue : 0;
        HashSet<Item> finalEquipment = [ ];

        foreach (var w in weapons)
        {
            int gold = 0;
            int damageMod = 0;
            int armorMod = 0;

            foreach (var a in armor) 
            {
                for (int i = 0; i < rings.Count - 1; i++)
                {
                    Item r1 = rings[i];

                    for (int j = i + 1; j < rings.Count; j++)
                    {
                        Item r2 = rings[j];
                        gold = w.Cost + a.Cost + r1.Cost + r2.Cost;

                        if (isOptimal ? gold < finalGold : gold > finalGold)
                        {
                            damageMod = w.Damage + a.Damage + r1.Damage + r2.Damage;
                            armorMod = w.Armor + a.Armor + r1.Armor + r2.Armor;
                            Entity equippedPlayer = new(player.Name, player.MaxHP, player.Damage + damageMod, player.Armor + armorMod);

                            // fight!
                            if (DoesPlayerWinFight(equippedPlayer, enemy) == isWin)
                            {
                                finalEquipment = [ w, a, r1, r2 ];
                                finalGold = gold;
                            }
                        }
                    }
                }
            }
        }
        return finalGold;
    }

    private static bool DoesPlayerWinFight(Entity player, Entity enemy)
    {
        player.HP = player.MaxHP;
        enemy.HP = enemy.MaxHP;

        int playerDamage = (player.Damage - enemy.Armor) < 1 ? 1 : player.Damage - enemy.Armor;
        int enemyDamage = (enemy.Damage - player.Armor) < 1 ? 1 : enemy.Damage - player.Armor;

        while (true)
        {
            if (enemy.HP <= 0)
            {
                return true;
            } 
            else if (player.HP <= 0)
            {
                return false;
            }
            else
            {
                enemy.HP -= playerDamage;
                player.HP -= enemyDamage;
            }
        }
    }

    public string Answer()
    {
        int hp = 100;
        Entity boss = InitBoss();
        Entity player = new("player", hp, 0, 0);

        // part 1
        int gold1 = CalculateOptimalShopping(player, boss);

        // part 2
        int gold2 = CalculateOptimalShopping(player, boss, false, false);

        return $"the least of amount of gold you can spend and still win the fight = {gold1} and the most amount of gold you can spend and still lost the fight = {gold2}";
    }
}
