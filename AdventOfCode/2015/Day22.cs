using System;

namespace AdventOfCode._2015;

public class Day22 : ISolution
{
    private static readonly string filePath = $"lib\\2015\\Day22-input.txt";
    private static readonly string inputText = File.ReadAllText(filePath);
    private static readonly List<Spell> spellList = InitSpells();
    private static Dictionary<(int playerHP, int mana, int spentMana, int bossHP, int pDuration, int sDuration, int rDuration, SpellType type, bool isHard), int>? cache;

    private enum SpellType
    {
        Magic_Missile,
        Drain,
        Shield,
        Poison,
        Recharge
    }
    private enum SpellDuration
    {
        Instant,
        Duration
    }
    private class Spell(string name, SpellType type, SpellDuration duration, int manaCost, int damage = 0, int heal = 0, int maxDuration = 0, int armor = 0, int manaRestored = 0)
    {
        public string Name { get; init; } = name;
        public SpellType Type { get; init; } = type;
        public SpellDuration Duration { get; init; } = duration;
        public int ManaCost { get; init; } = manaCost;
        public int Damage { get; init; } = damage;
        public int Heal { get; init; } = heal;
        public int MaxDuration { get; init; } = maxDuration;
        public int RemainingDuration { get; set; } = 0; // unused
        public int Armor { get; init; } = armor;
        public int ManaRestored { get; init; } = manaRestored;
        public bool IsReady()
        {
            if (Duration == SpellDuration.Instant)
            {
                return true;
            }
            else if (Duration == SpellDuration.Duration && RemainingDuration <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryCast(ref int mana) 
        {
            if (IsReady() && mana >= ManaCost)
            {
                mana -= ManaCost;
                return true;
            }
            else
            {
                return false;
            }
        }
        public int Cast(int mana)
        {
            if (IsReady() && mana >= ManaCost)
            {
                return mana - ManaCost;
            }
            else
            {
                throw new Exception("On cooldown or not enough mana");
            }
        }
    }
    private struct Entity(string name, int hp, int damage, int armor = 0, int mana = 0)
    {
        public string Name { get; init; } = name;
        public int MaxHP { get; init; } = hp;
        public int HP { get; set; } = hp;
        public int Damage { get; init; } = damage;
        public int Armor { get; set; } = armor;
        public int Mana { get; set; } = mana;
    }

    private static List<Spell> InitSpells()
    {
        List<Spell> list = new()
        {
            { new Spell("Magic Missile", SpellType.Magic_Missile, SpellDuration.Instant, 53, 4) },
            { new Spell("Drain", SpellType.Drain, SpellDuration.Instant, 73, 2, 2) },
            { new Spell("Shield", SpellType.Shield, SpellDuration.Duration, 113, 0, 0, 6, 7) },
            { new Spell("Poison", SpellType.Poison, SpellDuration.Duration, 173, 3, 0, 6) },
            { new Spell("Recharge", SpellType.Recharge, SpellDuration.Duration, 229, 0, 0, 5, 0, 101) }
        };

        return list;
    }
    private static Entity InitBoss()
    {
        string[] lines = inputText.Split(Environment.NewLine);
        string[] hp = lines[0].Split(' ');
        string[] damage = lines[1].Split(' ');

        Entity boss = new("boss", int.Parse(hp[2]), int.Parse(damage[1]));

        return boss;
    }

    private static int GenerateOptimalCombat(Entity player, Entity boss, bool isHard = false)
    {
        cache = [];
        int bestMana = int.MaxValue;
        Dictionary<SpellType, int> castSpells = [];

        foreach (Spell spell in spellList)
        {
            castSpells.Add(spell.Type, 0);
        }

        foreach (Spell spell in spellList)
        {
            Dictionary<SpellType, int> newCastSpells = castSpells.ToDictionary();
            newCastSpells[spell.Type]++;
            var result = NextCombatRound(player, boss, 0, 0, 0, 0, spell, ref bestMana, isHard, newCastSpells);
        }

        return bestMana;
    }

    private static int NextCombatRound(Entity player, Entity boss, int spentMana, int d_Shield, int d_Poison, int d_Recharge, Spell nextSpell, ref int bestMana, bool isHard, Dictionary<SpellType, int> castSpells)
    {
        // check if result is cached
        if (cache!.TryGetValue((player.HP, player.Mana, spentMana, boss.HP, d_Shield, d_Poison, d_Recharge, nextSpell.Type, isHard), out int returnValue))
        {
            return returnValue;
        }
        
        if (spentMana > bestMana)
        {
            // if not optimal, return early
            return spentMana;
        }
        else
        {
            // take damage if hard mode
            if (isHard)
            {
                player.HP--;

                // check if combat is over
                if (CheckCombatEnd(player, boss, spentMana, ref bestMana) is not CombatResult.Continue)
                {
                    return bestMana;
                }
            }

            // resolve new spell
            spentMana += nextSpell.ManaCost;
            player.Mana = nextSpell.Cast(player.Mana);
            if (nextSpell.Duration == SpellDuration.Instant)
            {
                player.HP += nextSpell.Heal;
                boss.HP -= CalculateDamage(nextSpell.Damage, boss.Armor);
            }
            else if (nextSpell.Duration == SpellDuration.Duration)
            {
                switch (nextSpell.Type)
                {
                    case SpellType.Shield:
                        d_Shield = nextSpell.MaxDuration;
                        break;
                    case SpellType.Poison:
                        d_Poison = nextSpell.MaxDuration;
                        break;
                    case SpellType.Recharge:
                        d_Recharge = nextSpell.MaxDuration;
                        break;
                }
            }

            // resolve start-of-turn effects
            CheckSpellEffects(ref player, ref boss, ref d_Shield, ref d_Poison, ref d_Recharge);

            // check if combat is over
            if (CheckCombatEnd(player, boss, spentMana, ref bestMana) is not CombatResult.Continue)
            {
                return bestMana;
            }

            // resolve boss turn
            player.HP -= CalculateDamage(boss.Damage, player.Armor + (d_Shield > 0 ? spellList[(int)SpellType.Shield].Armor : 0));

            // check if combat is over
            if (CheckCombatEnd(player, boss, spentMana, ref bestMana) is not CombatResult.Continue)
            {
                return bestMana;
            }

            // resolve start-of-turn effects
            CheckSpellEffects(ref player, ref boss, ref d_Shield, ref d_Poison, ref d_Recharge);

            // check if combat is over
            if (CheckCombatEnd(player, boss, spentMana, ref bestMana) is not CombatResult.Continue)
            {
                return bestMana;
            }

            int nextBestMana = int.MaxValue;

            // choose next spell
            foreach (Spell spell in spellList)
            {
                if (player.Mana > spell.ManaCost)
                {
                    int n_Shield = d_Shield;
                    int n_Poison = d_Poison;
                    int n_Recharge = d_Recharge;

                    switch (spell.Type)
                    {
                        case SpellType.Shield:
                            if (d_Shield > 0)
                            {
                                continue;
                            }
                            n_Shield = spell.MaxDuration;
                            break;
                        case SpellType.Poison:
                            if (d_Poison > 0)
                            {
                                continue;
                            }
                            n_Poison = spell.MaxDuration;
                            break;
                        case SpellType.Recharge:
                            if (d_Recharge > 0)
                            {
                                continue;
                            }
                            n_Recharge = spell.MaxDuration;
                            break;
                        default:
                            // do nothing
                            break;
                    }

                    Dictionary<SpellType, int> newCastSpells = castSpells.ToDictionary();
                    newCastSpells[spell.Type]++;
                    
                    // start the next round with the new spell
                    int result = NextCombatRound(player, boss, spentMana, n_Shield, n_Poison, n_Recharge, spell, ref bestMana, isHard, newCastSpells);

                    // cache the result
                    cache[(player.HP, player.Mana, spentMana, boss.HP, n_Shield, n_Poison, n_Recharge, spell.Type, isHard)] = result;
                    
                    if (result < nextBestMana && result is not -1)
                    {
                        nextBestMana = result;
                    }
                }
            }
            return nextBestMana;
        }
    }

    private static void CheckSpellEffects(ref Entity player, ref Entity boss, ref int d_Shield, ref int d_Poison, ref int d_Recharge)
    {
        // check active duration spells
        foreach (var spell in spellList.Where(x => x.Duration == SpellDuration.Duration))
        {
            switch (spell.Type)
            {
                case SpellType.Shield:
                    if (d_Shield > 0) 
                    {
                        d_Shield--;
                    }
                    break;
                case SpellType.Poison:
                    if (d_Poison > 0) 
                    {
                        boss.HP -= CalculateDamage(spell.Damage, boss.Armor);
                        d_Poison--;
                    }
                    break;
                case SpellType.Recharge:
                    if (d_Recharge > 0) 
                    {
                        player.Mana += spell.ManaRestored;
                        d_Recharge--;
                    }
                    break;
                default:
                    throw new Exception("Spell not supported");
            };
        }
    }

    private enum CombatResult
    {
        Win,
        Lose,
        Continue
    }

    private static CombatResult CheckCombatEnd(Entity player, Entity boss, int spentMana, ref int bestMana)
    {
        if (boss.HP <= 0)
        {
            if (spentMana < bestMana) 
            {
                bestMana = spentMana;
            }
            return CombatResult.Win;
        }
        else if (player.HP <= 0 || player.Mana <= 0 )
        {
            return CombatResult.Lose;
        }
        else
        {
            return CombatResult.Continue;
        }
    }

    private static int CalculateDamage(int damage, int armor)
    {
        return (damage - armor) < 1 ? 1 : damage - armor;
    }

    public string Answer()
    {
        int hp = 50;
        int manaStart = 500;
        Entity boss = InitBoss();
        Entity player = new("player", hp, 0, 0, manaStart);

        // part 1
        int mana1 = GenerateOptimalCombat(player, boss);

        // part 2
        int mana2 = GenerateOptimalCombat(player, boss, true);
        
        return $"the least of amount of man you can spend and still win the fight = {mana1}; the least of amount of man you can spend and still win the fight on \x1b[1mhard\x1b[0m = {mana2}";
    }
}
