using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        Arena arena = new Arena();
        arena.ChouseFigters();
        arena.Fight();
        arena.ShowWinner();
    }
}

class Arena
{
    private List<Hero> _heros = new List<Hero> { new Ork(), new Human(), new Elf(), new Gnom(), new Demon() };
    private Hero _leftFighter;
    private Hero _rightFighter;

    public void ChouseFigters()
    {
        for (int i = 0; i < _heros.Count; i++)
        {
            Console.Write(i + " ");
            _heros[i].ShowStats();
        }

        _leftFighter = new Hero(_heros[Utils.ReadInt("Choose left warrior: ", 0, _heros.Count - 1)]);
        _rightFighter = new Hero(_heros[Utils.ReadInt("Choose right warrior: ", 0, _heros.Count - 1)]);
    }

    public void Fight()
    {
        while (_leftFighter.IsAlife() && _rightFighter.IsAlife())
        {
            Console.WriteLine();
            _leftFighter.TakeDamage(_rightFighter.GiveDamage());
            _rightFighter.TakeDamage(_leftFighter.GiveDamage());
            _leftFighter.ShowStats();
            _rightFighter.ShowStats();
            Console.WriteLine();
        }
    }

    public void ShowWinner()
    {
        if (_leftFighter.IsAlife())
            Console.WriteLine("Left fighter is win");
        else if (_rightFighter.IsAlife())
            Console.WriteLine("Right fighter is win");
        else if (_leftFighter.IsAlife() == false && _rightFighter.IsAlife() == false)
            Console.WriteLine("Both dead");
    }
}

static class Utils
{
    public static int ReadInt(string text = "", int minValue = 0, int maxValue = 0)
    {
        int digitToOut = 0;
        bool isRun = true;

        while (isRun)
        {
            Console.Write(text + " ");
            string digitFromConsole = Console.ReadLine();

            if (int.TryParse(digitFromConsole, out digitToOut))
                isRun = false;

            if (isRun == false && (digitToOut <= maxValue && digitToOut >= minValue))
                isRun = false;
            else
                isRun = true;
        }

        return digitToOut;
    }
}

class Hero
{
    protected string Name;
    protected int Health;
    protected int Armor;
    protected int Damage;
    protected int CountOfAttak;
    protected Random random = new Random();

    public Hero()
    {
    }

    public Hero(Hero hero)
    {
        Name = hero.Name;
        Health = hero.Health;
        Armor = hero.Armor;
        Damage = hero.Damage;
        CountOfAttak = hero.CountOfAttak;
    }

    public void ShowStats()
    {
        Console.WriteLine($"Name: {Name} Health: {Health} armor: {Armor} damage: {Damage}");
    }

    public bool IsAlife()
    {
        return (Health > 0);
    }

    public virtual void TakeDamage(int damage)
    {
        Health -= (damage - Armor);
    }

    public virtual int GiveDamage()
    {
        return Damage;
    }
}

class Ork : Hero
{
    public Ork() : base()
    {
        Name = "Ork";
        Health = 200;
        Armor = 20;
        Damage = 25;
    }

    public override int GiveDamage()
    {
        return Damage * random.Next(1, 3);
    }
}

class Elf : Hero
{
    private int _enhancedAttackNumber = 3;
    private int _enhancedAttackCoefficient = 2;
    public Elf() : base()
    {
        Name = "Elf";
        Health = 100;
        Armor = 15;
        Damage = 20;
        CountOfAttak = 0;
    }

    public override int GiveDamage()
    {
        CountOfAttak++;

        if (CountOfAttak >= _enhancedAttackNumber)
        {
            CountOfAttak = 0;
            return Damage * _enhancedAttackCoefficient;
        }

        return Damage;
    }
}

class Human : Hero
{
    private int _angry = 0;
    private int _angryMaxQuantity = 5;
    private int _remedy = 30;

    public Human() : base()
    {
        Name = "Human";
        Health = 100;
        Armor = 5;
        Damage = 30;
    }

    public override void TakeDamage(int damage)
    {
        _angry++;

        if (_angry >= _angryMaxQuantity)
        {
            _angry = 0;
            Health += _remedy;
        }

        Health -= damage;
    }
}

class Gnom : Hero
{
    private int _enhancedOfArmor = 3;
    private int _mana = 100;

    public Gnom() : base()
    {
        Name = "Gnom";
        Health = 100;
        Armor = 10;
        Damage = 15;
    }

    public override void TakeDamage(int damage)
    {
        Health -= (damage - Armor);
        Armor += _enhancedOfArmor;
        _mana = -20;
    }

    public override int GiveDamage()
    {
        if (_mana > 0)
            return Damage * 2;

        return Damage;
    }
}

class Demon : Hero
{
    public Demon() : base()
    {
        Name = "Demon";
        Health = 100;
        Armor = 2;
        Damage = 20;
    }

    public override void TakeDamage(int damage)
    {
        Health -= (damage * random.Next(0, 2));
    }
}