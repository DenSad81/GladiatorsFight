﻿using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Arena arena = new Arena();
        arena.ChooseFigters();
        arena.Fight();
        arena.ShowWinner();
    }
}

class Arena
{
    private List<Hero> _heros;
    private Hero _leftFighter;
    private Hero _rightFighter;

    public Arena()
    {
        _heros = new List<Hero> { new Ork("Ork", 200, 20, 25),
                                  new Human( "Human",100,5,30),
                                  new Elf("Elf", 100,15, 20 ),
                                  new Gnom("Gnom",100,10,15),
                                  new Demon("Demon", 100,2,20) };
    }

    public void ChooseFigters()
    {
        for (int i = 0; i < _heros.Count; i++)
        {
            Console.Write(i + " ");
            _heros[i].ShowStats();
        }

        int quantityPersonsWithoutOne = _heros.Count - 1;
        _leftFighter = _heros[Utils.ReadInt("Choose left warrior: ", 0, quantityPersonsWithoutOne)].Clone();
        _rightFighter = _heros[Utils.ReadInt("Choose right warrior: ", 0, quantityPersonsWithoutOne)].Clone();
   }

    public void Fight()
    {
        while (_leftFighter.IsAlife && _rightFighter.IsAlife)
        {
            Console.WriteLine();
            _leftFighter.Attack(_rightFighter);
            _rightFighter.Attack(_leftFighter);
            _leftFighter.ShowStats();
            _rightFighter.ShowStats();
            Console.WriteLine();
        }
    }

    public void ShowWinner()
    {
        if (_leftFighter.IsAlife)
            Console.WriteLine("Left fighter is win");
        else if (_rightFighter.IsAlife)
            Console.WriteLine("Right fighter is win");
        else if (_leftFighter.IsAlife == false && _rightFighter.IsAlife == false)
            Console.WriteLine("Both dead");
    }
}

static class Utils
{
    public static int ReadInt(string text = "", int minValue = int.MinValue, int maxValue = int.MaxValue)
    {
        int number;
        Console.Write(text + " ");

        while (int.TryParse(Console.ReadLine(), out number) == false || number > maxValue || number < minValue)
            Console.Write(text + " ");

        return number;
    }

    public static int GenerateRandomNumber(int min, int max)
    {
        Random random = new Random();
        return random.Next(min, max);
    }
}

class Hero
{
    protected string Name;
    protected int Health;
    protected int Armor;
    protected int Damage;

    public Hero(string name, int health, int armor, int damage)
    {
        Name = name;
        Health = health;
        Armor = armor;
        Damage = damage;
    }

    public bool IsAlife => (Health > 0);

    public virtual Hero Clone()
    {
        return new Hero(Name, Health, Armor, Damage);
    }

    public void ShowStats()
    {
        Console.WriteLine($"Name: {Name} Health: {Health} armor: {Armor} damage: {Damage}");
    }

    public virtual void Attack(Hero enemy)
    {
        enemy.TakeDamage(this.GiveDamage());
    }

    public virtual void TakeDamage(int damage)
    {
        int delta = damage - Armor;

        if (delta > 0)
            Health -= delta;
    }

    public virtual int GiveDamage()
    {
        return Damage;
    }
}

class Ork : Hero
{
    private int _ratio = 3;

    public Ork(string name, int health, int armor, int damage) : base(name, health, armor, damage)
    { }

    public override Hero Clone()
    {
        return new Ork(Name, Health, Armor, Damage);
    }

    public override int GiveDamage()
    {
        return Damage * Utils.GenerateRandomNumber(1, _ratio);
    }
}

class Elf : Hero
{
    private int _dubleAttackNumber = 3;
    private int _countOfAttak = 0;

    public Elf(string name, int health, int armor, int damage) : base(name, health, armor, damage)
    { }

    public override Hero Clone()
    {
        return new Elf(Name, Health, Armor, Damage);
    }

    public override void Attack(Hero enemy)
    {
        enemy.TakeDamage(this.GiveDamage());
        _countOfAttak++;

        if (_countOfAttak >= _dubleAttackNumber)
        {
            _countOfAttak = 0;
            enemy.TakeDamage(this.GiveDamage());
            Console.WriteLine("DubleAttack");
        }
    }
}

class Human : Hero
{
    private int _angry = 0;
    private int _angryMaxQuantity = 5;
    private int _remedy = 30;

    public Human(string name, int health, int armor, int damage) : base(name, health, armor, damage)
    { }

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
    private int _antiMana = 20;
    private int _ratio = 2;

    public Gnom(string name, int health, int armor, int damage) : base(name, health, armor, damage)
    { }

    public override Hero Clone()
    {
        return new Gnom(Name, Health, Armor, Damage);
    }

    public override void TakeDamage(int damage)
    {
        Health -= (damage - Armor);
        Armor += _enhancedOfArmor;
        _mana -= _antiMana;
    }

    public override int GiveDamage()
    {
        if (_mana > 0)
            return Damage * _ratio;

        return Damage;
    }
}

class Demon : Hero
{
    private int _ratio = 2;

    public Demon(string name, int health, int armor, int damage) : base(name, health, armor, damage)
    { }

    public override Hero Clone()
    {
        return new Demon(Name, Health, Armor, Damage);
    }

    public override void TakeDamage(int damage)
    {
        Health -= (damage * Utils.GenerateRandomNumber(0, _ratio));
    }
}