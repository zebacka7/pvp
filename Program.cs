using System;
using System.IO;
using System.Collections;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Channels;
using System.Net;
using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations;




namespace project
{

  public class Rand
  {
    public int Run(int min, int max)
    {
      int range = (max - min) + 1;
      Random rng = new Random();
      return min + rng.Next() % range;
    }
  }
  public class Weapon
  {
    public string Name;
    public double Damage;
    public string Type;

    public Weapon(string name, string type, double damage)
    {
      Name = name;
      Damage = damage;
      Type = type;
    }

    public void DisplayWeaponInfo()
    {
      Console.WriteLine($"Weapon: {Name}, Type: {Type}, Damage: {Damage};");
    }
  }
  public class Hero
  {
    public string Name;
    private int Strength;
    private int Dexterity;
    private int Intelligence;
    public double HP;
    public double MP;
    public Weapon EquippedWeapon;
    public static int ValidOption(string prompt, int minOption, int maxOption)
    {
      int opt = 0;
      bool valid = false;

      while (!valid)
      {
        Console.Write(prompt);
        string option = Console.ReadLine().Trim();

        if (int.TryParse(option, out opt) && opt >= minOption && opt <= maxOption)
        {
          valid = true;
        }
        else
        {
          Console.ForegroundColor = ConsoleColor.DarkGray;
          Console.WriteLine($"That's not a valid option! Please choose a number between {minOption} and {maxOption}!");
          Console.ResetColor();
        }
      }
      return opt;
    }
    private void Init(int strength = 10, int dexterity = 10, int intelligence = 10)
    {
      this.Strength = strength;
      this.Dexterity = dexterity;
      this.Intelligence = intelligence;
      HP = 40 + strength;
      MP = 20 + (1.5 * intelligence);
    }

    public int GetStrength() { return this.Strength; }
    public int GetDexterity() { return this.Dexterity; }
    public int GetIntelligence() { return this.Intelligence; }

    public void UpStrength() { this.Strength += 5; this.HP += 5; }
    public void UpDexterity() { this.Dexterity += 5; }
    public void UpIntelligence() { this.Intelligence += 5; this.MP += (3 * this.Intelligence); }

    public Hero(string name, string myclass)
    {
      Name = name;
      switch (myclass)
      {
        case "warrior": Init(15, 10, 5); break;
        case "assassin": Init(5, 15, 10); break;
        case "sorcerer": Init(5, 5, 20); break;
        case "orc": Init(20, 5, 0); break;
        default: Init(); break;
      }
    }

    public static Hero Load(string name)
    {
      name = name + ".json";
      string heroString = File.ReadAllText(name);
      JObject heroJson = JObject.Parse(heroString);

      string heroName = (string)heroJson["Name"];
      string heroClass = (string)heroJson["Class"];

      Hero hero = new Hero(heroName, heroClass);
      hero.Strength = (int)heroJson["Strength"];
      hero.Dexterity = (int)heroJson["Dexterity"];
      hero.Intelligence = (int)heroJson["Intelligence"];

      return hero;
    }

    public void AttackWithWeapon(Hero enemy)
    {
      Rand rand = new Rand();
      double damage = Strength * rand.Run(5, 10) / 10;
      double chance = rand.Run(1, 100);



      if (EquippedWeapon == null)
      {
        Attack(enemy);
      }
      else
      {
        if (EquippedWeapon.Type == "Sword")
        {
          double totaldamage = (damage + EquippedWeapon.Damage);
          Console.ForegroundColor = ConsoleColor.DarkRed;
          Console.WriteLine($"{Name} swings at {enemy.Name} with the {EquippedWeapon.Name}, dealing {totaldamage} HP total!");
          Console.ResetColor();
          enemy.HP -= EquippedWeapon.Damage;
        }
        else if (EquippedWeapon.Type == "Dagger")
        {
          double totaldamage = (damage + EquippedWeapon.Damage);
          Console.ForegroundColor = ConsoleColor.DarkRed;
          Console.WriteLine($"{Name} stabs {enemy.Name} in the thigh with the {EquippedWeapon.Name}, dealing {totaldamage} HP total!");
          Console.ResetColor();
          enemy.HP -= EquippedWeapon.Damage;
        }
        else if (EquippedWeapon.Type == "Bow")
        {
          if (chance >= 24)
          {
            double totaldamage = (damage + EquippedWeapon.Damage);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"{Name} shoots an arrow at {enemy.Name} with the {EquippedWeapon.Name}, dealing {totaldamage} HP total. ");
            Console.ResetColor();
            enemy.HP -= EquippedWeapon.Damage;
          }
          else
            Console.WriteLine($"{Name} attempts to shoot an arrow at {enemy.Name}, but misses!");
        }
        else if (EquippedWeapon.Type == "Staff")
        {
          double totaldamage = (damage + EquippedWeapon.Damage);
          Console.ForegroundColor = ConsoleColor.DarkRed;
          Console.WriteLine($"{Name} hurts {enemy.Name} using magic with their {EquippedWeapon.Name}, dealing {totaldamage} HP total.");
          Console.ResetColor();
          enemy.HP -= EquippedWeapon.Damage;

        }
      }


    }
    public void Attack(Hero enemy)
    {
      Rand rand = new Rand();
      double damage = Strength * rand.Run(5, 10) / 10;
      double chance = rand.Run(1, 100);

      if (rand.Run(0, 100) > enemy.GetDexterity())
      {
        if (chance < 20)
        {
          Console.ForegroundColor = ConsoleColor.DarkRed;
          Console.WriteLine($"{Name} kicks {enemy.Name}! They deal {damage} HP!");
          Console.ResetColor();
          enemy.HP -= damage;
        }
        else if (chance > 20 && chance < 80)
        {
          Console.ForegroundColor = ConsoleColor.DarkRed;
          Console.WriteLine($"{Name} slaps {enemy.Name}! They deal {damage} HP!");
          Console.ResetColor();
          enemy.HP -= damage;

        }
        else
        {
          Console.ForegroundColor = ConsoleColor.DarkRed;
          Console.WriteLine($"{Name} punches {enemy.Name} in the stomach! They deal {damage} HP!");
          Console.ResetColor();
          enemy.HP -= damage;
        }
      }
      else
      {
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine($"{Name} attempts to punch {enemy.Name} in the face, but misses!");
        Console.ResetColor();
      }
    }

    public void LevelUp()
    {
      int opt = ValidOption("1:Strength 2:Dexterity 3:Intelligence ", 1, 3);
      switch (opt)
      {
        case 1:
          UpStrength();
          Console.ForegroundColor = ConsoleColor.Magenta;
          Console.Write($"{Name} levels up strength!");
          Console.ResetColor();
          break;
        case 2:
          UpDexterity();
          Console.ForegroundColor = ConsoleColor.Magenta;
          Console.Write($"{Name} levels up dexterity!");
          Console.ResetColor();
          break;
        case 3:
          UpIntelligence();
          Console.ForegroundColor = ConsoleColor.Magenta;
          Console.Write($"{Name} levels up intelligence!");
          Console.ResetColor(); break;
      }

      Console.WriteLine();
    }
    public static bool orc = false;

    public void CastHeal()
    {
      {
        if (MP >= 6)
        {
          Console.ForegroundColor = ConsoleColor.DarkGreen;
          Console.WriteLine($"{Name} consumes an enchanted tangerine, healing themself. ");
          Console.ResetColor();
          HP += 10;
          MP -= 6;
        }
      }
    }
    public class HeroSelection
    {

      public static Hero SelectHero()
      {
        Console.Write("Welcome to the battlefield, warrior! The fight is just about to begin. What do you want to be known as?   ");
        string heroName = Console.ReadLine();

        Hero hero = new Hero(heroName, "");
        bool classchosen = false;
        while (!classchosen)
        {
          Console.Write("Welcome, ");
          Console.ForegroundColor = ConsoleColor.Cyan;
          Console.Write($"{heroName}");
          Console.ResetColor();
          Console.WriteLine(". Please choose your class by typing in the corresponding number: ");
          Console.Write("1:");
          Console.ForegroundColor = ConsoleColor.Red;
          Console.Write(" Warrior ");
          Console.ResetColor();
          Console.Write("2:");
          Console.ForegroundColor = ConsoleColor.Blue;
          Console.Write(" Assassin ");
          Console.ResetColor();
          Console.Write("3:");
          Console.ForegroundColor = ConsoleColor.Magenta;
          Console.Write(" Sorcerer ");
          Console.ResetColor();
          Console.Write("4:");
          Console.ForegroundColor = ConsoleColor.DarkGreen;
          Console.Write(" Orc   ");
          Console.ResetColor();

          int class2Choice;
          bool validchoice = false;
          while (!validchoice)
          {
            string input = Console.ReadLine();
            if (int.TryParse(input, out class2Choice) && class2Choice >= 1 && class2Choice <= 4)
            {
              validchoice = true;
            }
            else
            {
              Console.ForegroundColor = ConsoleColor.DarkGray;
              Console.WriteLine("Invalid input! Please select a number between 1 and 4.");
              Console.ResetColor();
            }

            switch (class2Choice)
            {
              case 1:
                {
                  Console.ForegroundColor = ConsoleColor.Red;
                  Console.WriteLine("The Warrior thrives on raw strength, using it to overpower enemies in close combat. Their dexterity allows for swift, precise strikes and deft maneuvers despite their heavy armor. While not known for high intelligence, their battlefield instincts and tactical awareness make them formidable in battle. Focused on endurance and power, Warriors dominate through physical prowess.");
                  Console.ResetColor();
                  int opt = ValidOption("Are you sure of chosen class? 1: YES 2: NO, I WANT TO TRY ANOTHER  ", 1, 2);
                  if (opt == 1)
                  {
                    Console.WriteLine("You are now playing as a warrior.");
                    hero.Init(15, 10, 5);
                    classchosen = true;
                  }
                  else if (opt == 2)
                  {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Returning to the choosing page.");
                    Console.ResetColor();
                  }
                  break;
                }
              case 2:
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("The Assassin excels in dexterity, moving with unmatched speed and agility to strike from the shadows. Their intelligence is sharp, allowing them to analyze weaknesses and exploit vulnerabilities. While lacking in raw strength, their precise, lethal attacks can take down enemies swiftly. Stealth, cunning, and strategy define the Assassin's approach to combat.");
                Console.ResetColor();
                int opt3 = ValidOption("Are you sure of chosen class? 1: YES 2: NO, I WANT TO TRY ANOTHER  ", 1, 2);
                if (opt3 == 1)
                {
                  Console.WriteLine("You are now playing as an assassin.");
                  hero.Init(5, 15, 10);
                  classchosen = true;
                }
                else if (opt3 == 2)
                {
                  Console.ForegroundColor = ConsoleColor.DarkGray;
                  Console.WriteLine("Returning to the choosing page.");
                  Console.ResetColor();
                }
                break;
              case 3:
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("The Sorcerer harnesses powerful intelligence to control the arcane, manipulating magic with precision. Their dexterity is often focused on casting spells swiftly, though not necessarily with physical finesse. They may lack raw strength, but their mastery over elemental forces and magical energy makes them formidable. A Sorcerer's strategic mind allows them to dominate from a distance, turning the tide of battle with devastating spells.");
                Console.ResetColor();
                int opt4 = ValidOption("Are you sure of chosen class? 1: YES 2: NO, I WANT TO TRY ANOTHER  ", 1, 2);
                if (opt4 == 1)
                {
                  Console.WriteLine("You are now playing as a mage.");
                  hero.Init(5, 5, 20);
                  classchosen = true;
                }
                else if (opt4 == 2)
                {
                  Console.ForegroundColor = ConsoleColor.DarkGray;
                  Console.WriteLine("Returning to the choosing page.");
                  Console.ResetColor();
                }
                break;
              case 4:
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("An unstoppable juggernaut of pure muscle, the Orc is a living mountain on the battlefield. With the power to crush rocks in his grip and tear through foes with the force of a raging storm, his strength is unmatched. His movements may lack the grace of others, slow and lumbering, but when he strikes, the earth trembles. No spells or clever tricks are needed for this warrior—he relies solely on his raw, brutal might to flatten anything that dares to stand before him. The Orc may lack the finesse of others, but in the art of sheer destruction, he reigns supreme.");
                Console.ResetColor();
                int opt5 = ValidOption("Are you sure of chosen class? 1: YES 2: NO, I WANT TO TRY ANOTHER  ", 1, 2);
                if (opt5 == 1)
                {
                  Console.WriteLine("You are now playing as an orc.");
                  hero.Init(20, 5, 0);
                  classchosen = true;
                  orc = true;

                }
                else if (opt5 == 2)
                {
                  Console.ForegroundColor = ConsoleColor.DarkGray;
                  Console.WriteLine("Returning to the choosing page.");
                  Console.ResetColor();
                }
                break;
            }
          }



        }
        return hero;

      }
      public static bool playeralone = false;
      public static Hero HeroSelectSecondHero()
      {
        int opt1 = ValidOption("Is there an opponent ready to challenge you, or are you fighting solo? 1: YES 2: NO   ", 1, 2);
        string hero2Name = "";
        Rand rand = new Rand();
        Hero hero2 = new Hero(hero2Name, "");
        bool classchosen = false;

        switch (opt1)
        {
          case 2:
            Console.Clear();
            hero2Name = "Brian Egghead";
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Brian Egghead ");
            Console.ResetColor();
            Console.WriteLine("joins the battle!");
            double botclass = rand.Run(1, 4);
            if (botclass == 1)
            {
              hero2.Init(15, 10, 5);
              classchosen = true;
            }
            if (botclass == 2)
            {
              hero2.Init(5, 15, 10);
              classchosen = true;

            }
            if (botclass == 3)
            {
              hero2.Init(5, 5, 20);
              classchosen = true;
            }
            if (botclass == 4)
            {
              hero2.Init(20, 5, 0);
              classchosen = true;
            }
            playeralone = true;
            hero2.Name = hero2Name;
            return hero2;
          case 1:

            Console.Clear();
            Console.Write("Another challenger steps forward! What name will strike fear into your opponent as you prepare for battle?   ");
            hero2.Name = Console.ReadLine();
            break;
        }

        while (!classchosen)
        {
          Console.Write("Welcome, ");
          Console.ForegroundColor = ConsoleColor.Yellow;
          Console.Write($"{hero2.Name}");
          Console.ResetColor();
          Console.WriteLine(".");
          Console.WriteLine("Choose your class by typing in the corresponding number: ");
          Console.Write("1:");
          Console.ForegroundColor = ConsoleColor.Red;
          Console.Write(" Warrior ");
          Console.ResetColor();
          Console.Write("2:");
          Console.ForegroundColor = ConsoleColor.Blue;
          Console.Write(" Assassin ");
          Console.ResetColor();
          Console.Write("3:");
          Console.ForegroundColor = ConsoleColor.Magenta;
          Console.Write(" Sorcerer ");
          Console.ResetColor();
          Console.Write("4:");
          Console.ForegroundColor = ConsoleColor.DarkGreen;
          Console.Write(" Orc   ");
          Console.ResetColor();
          int class2Choice;
          bool validchoice = false;
          while (!validchoice)
          {
            string input = Console.ReadLine();
            if (int.TryParse(input, out class2Choice) && class2Choice >= 1 && class2Choice <= 4)
            {
              validchoice = true;
            }
            else
            {
              Console.ForegroundColor = ConsoleColor.DarkGray;
              Console.WriteLine("Invalid input! Please select a number between 1 and 4.");
              Console.ResetColor();
            }
            switch (class2Choice)
            {
              case 1:
                {
                  Console.ForegroundColor = ConsoleColor.Red;
                  Console.WriteLine("The Warrior thrives on raw strength, using it to overpower enemies in close combat. Their dexterity allows for swift, precise strikes and deft maneuvers despite their heavy armor. While not known for high intelligence, their battlefield instincts and tactical awareness make them formidable in battle. Focused on endurance and power, Warriors dominate through physical prowess.");
                  Console.ResetColor();
                  int opt2 = ValidOption("Are you sure of chosen class? 1: YES 2: NO, I WANT TO TRY ANOTHER ", 1, 2);
                  if (opt2 == 1)
                  {
                    Console.WriteLine("You are now playing as a warrior.");
                    hero2.Init(15, 10, 5);
                    classchosen = true;
                  }
                  else if (opt2 == 2)
                  { Console.WriteLine("Returning to the choosing page."); }
                  break;
                }
              case 2:
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("The Assassin excels in dexterity, moving with unmatched speed and agility to strike from the shadows. Their intelligence is sharp, allowing them to analyze weaknesses and exploit vulnerabilities. While lacking in raw strength, their precise, lethal attacks can take down enemies swiftly. Stealth, cunning, and strategy define the Assassin's approach to combat.");
                Console.ResetColor();
                int opt3 = ValidOption("Are you sure of chosen class? 1: YES 2: NO, I WANT TO TRY ANOTHER ", 1, 2);
                if (opt3 == 1)
                {
                  Console.WriteLine("You are now playing as an assassin.");
                  hero2.Init(5, 15, 10);
                  classchosen = true;
                }
                else if (opt3 == 2)
                { Console.WriteLine("Returning to the choosing page."); }
                break;

              case 3:
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("The Sorcerer harnesses powerful intelligence to control the arcane, manipulating magic with precision. Their dexterity is often focused on casting spells swiftly, though not necessarily with physical finesse. They may lack raw strength, but their mastery over elemental forces and magical energy makes them formidable. A Sorcerer's strategic mind allows them to dominate from a distance, turning the tide of battle with devastating spells.");
                Console.ResetColor();
                int opt4 = ValidOption("Are you sure of chosen class? 1: YES 2: NO, I WANT TO TRY ANOTHER ", 1, 2);
                if (opt4 == 1)
                {
                  Console.WriteLine("You are now playing as a mage.");
                  hero2.Init(5, 5, 20);
                  classchosen = true;
                }
                else if (opt4 == 2)
                { Console.WriteLine("Returning to the choosing page."); }
                break;
              case 4:
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("An unstoppable juggernaut of pure muscle, the Orc is a living mountain on the battlefield. With the power to crush rocks in his grip and tear through foes with the force of a raging storm, his strength is unmatched. His movements may lack the grace of others, slow and lumbering, but when he strikes, the earth trembles. No spells or clever tricks are needed for this warrior—he relies solely on his raw, brutal might to flatten anything that dares to stand before him. The Orc may lack the finesse of others, but in the art of sheer destruction, he reigns supreme.");
                Console.ResetColor();
                Console.WriteLine("Are you sure of chosen class? 1: YES 2: NO, I WANT TO TRY ANOTHER ");
                int opt5 = int.Parse(Console.ReadLine());
                if (opt5 == 1)
                {
                  Console.WriteLine("You are now playing as an orc.");
                  hero2.Init(20, 5, 0);
                  classchosen = true;
                  orc = true;
                }
                else if (opt5 == 2)
                { Console.WriteLine("Returning to the choosing page."); }
                break;

            }
          }

        }




        Console.Clear();
        return hero2;

      }
    }
    public void AutomatedOpponent(Hero enemy)
    {
      Rand rand = new Rand();
      double botchance = rand.Run(1, 9);
      double botchance1 = rand.Run(1, 5);
      double botchance2 = rand.Run(1, 20);
      double chance = rand.Run(1, 101);


      if (HP > 25 && MP >= 15 && orc == false)
      {
        if (botchance == 1)
        {
          this.AttackWithWeapon(enemy);
        }
        else if (botchance == 2)
        {
          if (MP >= 15)
          {
            Console.ForegroundColor = ConsoleColor.Cyan;
            enemy.HP -= Intelligence * rand.Run(7, 13) / 10;
            Console.WriteLine($"{Name} summons a great bubble that lands on {enemy.Name}, dealing" + (Intelligence * rand.Run(7, 13) / 10) + "magic damage!");
            Console.ResetColor();
            MP -= 15;
          }
        }
        else if (botchance == 3)
        {
          if (MP >= 6)
          {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"{Name} consumes an enchanted tangerine, healing themself for 10HP.");
            Console.ResetColor();
            HP += 10;
            MP -= 6;
          }
        }
        else if (botchance == 4)
        {
          if (MP >= 8)
          {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"{Name} summons a fairy that shields them! They earn 10 dexterity.");
            Console.ResetColor();
            Dexterity += 10;
            MP -= 8;
          }
        }
        else if (botchance == 5)
        {
          if (MP >= 11)
          {
            if (chance < 5)
            {
              Console.ForegroundColor = ConsoleColor.Magenta;
              Console.WriteLine($"{Name} projects a force in {enemy.Name}'s direction, charming them! The opponent loses 10 dexterity.");
              Console.ResetColor();
              enemy.Dexterity -= 10;
            }
            else
            {
              Console.ForegroundColor = ConsoleColor.Magenta;
              Console.WriteLine($"{Name} tries to charm {enemy.Name}, but the spell backfires! Now both {Name} and {enemy.Name} are under its spell, staring at each other like long-lost soulmates!");
              Console.ResetColor();
              Console.ForegroundColor = ConsoleColor.Red;
              Console.WriteLine("888                                                   d8b                   888 ");
              Console.WriteLine("888                                                   Y8P                   888 ");
              Console.WriteLine("888                                                                         888 ");
              Console.WriteLine("888      .d88b.  888  888  .d88b.       888  888  888 888 88888b.  .d8888b  888 ");
              Console.WriteLine("888     d88**88b 888  888 d8P  Y8b      888  888  888 888 888 *88b 88K      888 ");
              Console.WriteLine("888     888  888 Y88  88P 88888888      888  888  888 888 888  888 *Y8888b. Y8P");
              Console.WriteLine("888     Y88..88P  Y8bd8P  Y8b.          Y88b 888 d88P 888 888  888      X88  *  ");
              Console.WriteLine("88888888 *Y88P*    Y88P    *Y8888        *Y8888888P*  888 888  888  88888P' 888 ");
              Console.ResetColor();
              Environment.Exit(2000);
            }
          }
        }
        else if (botchance == 6)
        {
          Console.ForegroundColor = ConsoleColor.Magenta;
          Console.WriteLine($"{Name} levels up strength!");
          Console.ResetColor();
          Strength += 5;
        }
        else if (botchance == 7)
        {
          Console.ForegroundColor = ConsoleColor.Magenta;
          Console.WriteLine($"{Name} levels up dexterity!");
          Console.ResetColor();
          Dexterity += 5;
        }
        else if (botchance == 8)
        {
          Console.ForegroundColor = ConsoleColor.Magenta;
          Console.WriteLine($"{Name} levels up intelligence!");
          Console.ResetColor();
          Intelligence += 5;
        }
        else if (botchance == 9)
        {
          Console.ForegroundColor = ConsoleColor.DarkGray;
          Console.WriteLine($"{Name} is looking for a weapon...");
          Console.ResetColor();
          this.LookforWeapon();
        }
      }
      else if ((HP > 25 && MP < 15) || (HP > 25 && orc == true))
      {
        if (botchance1 == 1)
        {
          this.AttackWithWeapon(enemy);
        }
        else if (botchance1 == 2)
        {
          Console.ForegroundColor = ConsoleColor.Magenta;
          Console.WriteLine($"{Name} levels up strength!");
          Console.ResetColor();
          Strength += 5;
        }
        else if (botchance1 == 3)
        {
          Console.ForegroundColor = ConsoleColor.Magenta;
          Console.WriteLine($"{Name} levels up dexterity!");
          Console.ResetColor();
          Dexterity += 5;
        }
        else if (botchance1 == 4)
        {
          Console.ForegroundColor = ConsoleColor.Magenta;
          Console.WriteLine($"{Name} levels up intelligence!");
          Console.ResetColor();
          Intelligence += 5;
        }
        else if (botchance1 == 5)
        {
          Console.ForegroundColor = ConsoleColor.DarkGray;
          Console.WriteLine($"{Name} is looking for a weapon...");
          this.LookforWeapon();
        }
      }
      else if (HP <= 25)
      {
        if (botchance1 >= 13)
          this.Rest();
        else if (botchance1 >= 4 && botchance1 <= 12)
          this.CastHeal();
        else if (botchance1 <= 3)
          this.Escape();
      }
    }

    public void Spell(Hero enemy)
    {
      if (orc == false)
      {
        int opt = ValidOption(" 1:Aqua prison  2:Charm  3:Heal   4:Shield ", 1, 4);
        Rand rand = new Rand();
        double aquadamage = Intelligence * rand.Run(7, 13) / 10;
        double heal = Intelligence * rand.Run(5, 15) / 10;
        double chance = rand.Run(1, 101);


        switch (opt)
        {
          case 1:
            if (MP >= 15)
            {
              Console.ForegroundColor = ConsoleColor.Cyan;
              enemy.HP -= aquadamage;
              Console.WriteLine($"{Name} summons a great bubble that lands on {enemy.Name}, dealing {aquadamage} magic damage! ");
              MP -= 15;
              Console.ResetColor();
              break;
            }
            else
            {
              Console.ForegroundColor = ConsoleColor.DarkGray;
              Console.WriteLine(" To cast this spell you need " + (15 - MP) + " more mana points! ");
              Console.ResetColor();
              break;

            }

          case 3:
            if (MP >= 6)
            {
              Console.ForegroundColor = ConsoleColor.DarkGreen;
              Console.WriteLine($"{Name} consumes an enchanted tangerine, healing themself for 10 HP. ");
              Console.ResetColor();
              HP += 10;
              MP -= 6;
              break;
            }
            else
            {
              Console.ForegroundColor = ConsoleColor.DarkGray;
              Console.WriteLine(" To cast this spell you need " + (6 - MP) + " mana points! ");
              Console.ResetColor();
              break;
            }

          case 4:
            if (MP >= 8)
            {
              Console.ForegroundColor = ConsoleColor.DarkMagenta;
              Console.WriteLine($"{Name} summons a fairy that shields them! They earn 10 dexterity. ");
              Dexterity += 10;
              MP -= 8;
              Console.ResetColor();
              break;

            }
            else
            {
              Console.ForegroundColor = ConsoleColor.DarkGray;
              Console.WriteLine(" To cast this spell you need " + (8 - MP) + " more mana points! ");
              Console.ResetColor();
              break;
            }
          case 2:
            if (MP >= 11)
            {
              if (chance > 5)
              {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"{Name} projects a force in {enemy.Name} direction, charming them! Their opponent loses 10 dexterity.");
                enemy.Dexterity -= 10;
                Console.ResetColor();
                break;
              }
              else
              {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"{Name} tries to charm {enemy.Name}, but the spell backfires! Now both {Name} and {enemy.Name} are under its spell, staring at each other like long-lost soulmates!");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("888                                                   d8b                   888 ");
                Console.WriteLine("888                                                   Y8P                   888 ");
                Console.WriteLine("888                                                                         888 ");
                Console.WriteLine("888      .d88b.  888  888  .d88b.       888  888  888 888 88888b.  .d8888b  888 ");
                Console.WriteLine("888     d88**88b 888  888 d8P  Y8b      888  888  888 888 888 *88b 88K      888 ");
                Console.WriteLine("888     888  888 Y88  88P 88888888      888  888  888 888 888  888 *Y8888b. Y8P");
                Console.WriteLine("888     Y88..88P  Y8bd8P  Y8b.          Y88b 888 d88P 888 888  888      X88  *  ");
                Console.WriteLine("88888888 *Y88P*    Y88P    *Y8888        *Y8888888P*  888 888  888  88888P' 888 ");
                Console.ResetColor();


                Environment.Exit(2000);
                break;

              }

            }
            else
            {
              Console.ForegroundColor = ConsoleColor.DarkGray;
              Console.WriteLine(" To cast this spell you need " + (11 - MP) + " more mana points! ");
              Console.ResetColor();
              break;
            }
        }
      }
      else
      {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("You're an orc! You cant cast spells!");
        Console.ResetColor();
      }

    }

    public void Regeneration()
    {
      double initialHP = HP;
      double initialMP = MP;
      HP += Math.Ceiling(Strength / 4.0);
      MP += Math.Ceiling(Intelligence / 4.0);

      Console.ForegroundColor = ConsoleColor.DarkGreen;
      Console.WriteLine($"{Name} due to passive regeneration has gained " + (HP - initialHP) + "HP and " + (MP - initialMP) + "MP");
      Console.ResetColor();
    }



    public void Actions(Hero enemy)
    {
      int opt = ValidOption(" 1:Level up 2:Rest 3: Look for weapons 4:Escape ", 1, 4);
      switch (opt)
      {
        case 1:
          LevelUp();
          break;

        case 2:
          Rest();
          break;

        case 3:
          LookforWeapon();
          break;

        case 4:
          Escape();
          break;
      }

    }

    public void LookforWeapon()
    {
      Rand rand = new Rand();
      double chance = rand.Run(1, 10);


      if (chance < 3)
      {
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine($"{Name} found nothing!");
        Console.ResetColor();
        EquippedWeapon = null;
      }
      else if (chance == 4)
      {
        Weapon newWeapon = new Weapon("Ashbringer", "Sword", rand.Run(5, 10));
        EquippedWeapon = newWeapon;
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine($"{Name} equipped the {newWeapon.Name}, a legendary sword that shines with radiant light, forged to purify evil and cut through darkness!");
        Console.ResetColor();
      }
      else if (chance == 5)
      {
        Weapon newWeapon = new Weapon("Windrunner", "Bow", rand.Run(5, 10));
        EquippedWeapon = newWeapon;
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine($"{Name} equipped the {newWeapon.Name}, a lightweight bow, swift as the breeze, delivering arrows with unmatched speed and precision! ");
        Console.ResetColor();
      }
      else if (chance == 6)
      {
        Weapon newWeapon = new Weapon("Divinity", "Dagger", rand.Run(5, 10));
        EquippedWeapon = newWeapon;
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine($"{Name} equipped the {newWeapon.Name}, a gleaming dagger infused with celestial power, striking with divine precision and unmatched lethality! ");
        Console.ResetColor();
      }
      else if (chance == 7)
      {
        Weapon newWeapon = new Weapon("Lightbane", "Staff", rand.Run(5, 10));
        EquippedWeapon = newWeapon;
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine($"{Name} equipped the {newWeapon.Name}, a dark, imposing staff that absorbs light and channels shadows, unleashing devastating forces of darkness! ");
        Console.ResetColor();
      }
      else if (chance == 8)
      {
        Weapon newWeapon = new Weapon("Rod of ages", "Staff", rand.Run(5, 10));
        EquippedWeapon = newWeapon;
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine($"{Name} equipped the {newWeapon.Name}, an ancient staff, infused with timeless magic, granting its wielder power that grows stronger with each passing year. ");
        Console.ResetColor();
      }
      else if (chance == 9)
      {
        Weapon newWeapon = new Weapon("Maple bow", "Bow", rand.Run(5, 10));
        EquippedWeapon = newWeapon;
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine($"{Name} equipped the {newWeapon.Name}, crafted from the finest maple wood!");
        Console.ResetColor();
      }
      else if (chance == 10)
      {
        Weapon newWeapon = new Weapon("Rune-Forged stilleto", "Dagger", rand.Run(5, 10));
        EquippedWeapon = newWeapon;
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine($"{Name} equipped the {newWeapon.Name}, a sleek, enchanted dagger infused with ancient runes striking with precision and mystical energy! ");
        Console.ResetColor();
      }
      else
      {
        Weapon newWeapon = new Weapon("War axe", "Sword", rand.Run(5, 10));
        EquippedWeapon = newWeapon;
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine($"{Name} equipped the {newWeapon.Name}, once used by Kratos himself! ");
        Console.ResetColor();
      }
    }
    public void Escape()
    {
      Console.ForegroundColor = ConsoleColor.DarkRed;
      Console.WriteLine($"{Name} flees the battlefield, handing the victory to his opponent!");
      Console.ResetColor();
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine(" ██████╗  █████╗ ███╗   ███╗███████╗      ██████╗ ██╗   ██╗███████╗██████╗ ");
      Console.WriteLine("██╔════╝ ██╔══██╗████╗ ████║██╔════╝     ██╔═══██╗██║   ██║██╔════╝██╔══██╗");
      Console.WriteLine("██║  ███╗███████║██╔████╔██║█████╗       ██║   ██║██║   ██║█████╗  ██████╔╝");
      Console.WriteLine("██║   ██║██╔══██║██║╚██╔╝██║██╔══╝       ██║   ██║██║   ██║██╔══╝  ██╔══██╗");
      Console.WriteLine("╚██████╔╝██║  ██║██║ ╚═╝ ██║███████╗     ╚██████╔╝╚██████╔╝███████╗██║  ██║");
      Console.WriteLine(" ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═╝╚══════╝      ╚══▀▀═╝  ╚═════╝ ╚══════╝╚═╝  ╚═╝");
      Console.ResetColor();
      Environment.Exit(200);
    }

    public void Rest()
    {
      Rand rand = new Rand();
      double resthp = 6 * rand.Run(7, 13) / 10;
      double restmp = 6 * rand.Run(5, 15) / 10;
      double initialHP = HP;
      double initialMP = MP;
      HP += resthp;
      MP += restmp;

      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine($"{Name} has decided to rest this round. They restore " + (resthp) + "HP and " + (restmp) + "MP");
      Console.ResetColor();

    }

  }


  class Program
  {
    public static int ValidOption(string prompt, int minOption, int maxOption)
    {
      int opt1 = 0;
      bool valid = false;

      while (!valid)
      {
        Console.Write(prompt);
        string option = Console.ReadLine();

        if (int.TryParse(option, out opt1) && opt1 >= minOption && opt1 <= maxOption)
        {
          valid = true;
        }
        else
        {
          Console.ForegroundColor = ConsoleColor.DarkGray;
          Console.WriteLine($"That's not a valid option! Please choose a number between {minOption} and {maxOption}!");
          Console.WriteLine("");
          Console.ResetColor();
        }
      }

      return opt1;
    }
    static void Main(string[] args)
    {

      int tour = 1;

      Console.Write("");
      Hero hero1 = Hero.HeroSelection.SelectHero();
      Hero hero2 = Hero.HeroSelection.HeroSelectSecondHero();


      Console.WriteLine();

      while (hero1.HP > 0 && hero2.HP > 0)
      {
        if (Hero.HeroSelection.playeralone == true)
        {
          if (tour == 1)
          {


            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(hero1.Name);
            Console.ResetColor();
            Console.WriteLine("  Str: {0}   Dex: {1}   Int: {2}   Weapon: {3}   HP: {4}   MP: {5}  ", hero1.GetStrength(), hero1.GetDexterity(), hero1.GetIntelligence(), hero1.EquippedWeapon != null ? hero1.EquippedWeapon.Name : "None", hero1.HP, hero1.MP);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(hero2.Name);
            Console.ResetColor();
            Console.WriteLine("  Str: {0}   Dex: {1}   Int: {2}   Weapon: {3}   HP: {4}   MP: {5}  ", hero2.GetStrength(), hero2.GetDexterity(), hero2.GetIntelligence(), hero2.EquippedWeapon != null ? hero2.EquippedWeapon.Name : "None", hero2.HP, hero2.MP);
            Console.WriteLine();
            Console.Write("Your Turn, ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(hero1.Name);
            Console.ResetColor();
            Console.WriteLine(".");

            int opt = ValidOption("1:Attack, 2:Spell, 3:Other actions ", 1, 3);
            switch (opt)
            {
              case 1:
                if (tour == 1)
                { hero1.AttackWithWeapon(hero2); }
                else { hero2.AttackWithWeapon(hero1); }
                break;

              case 2:
                if (tour == 1) { hero1.Spell(hero2); }
                else { hero2.Spell(hero1); }
                break;

              case 3:
                if (tour == 1) { hero1.Actions(hero2); }
                else { hero2.Actions(hero1); }
                break;

            }
          }

          else
          {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(hero1.Name);
            Console.ResetColor();
            Console.WriteLine("  Str: {0}   Dex: {1}   Int: {2}   Weapon: {3}   HP: {4}   MP: {5}  ", hero1.GetStrength(), hero1.GetDexterity(), hero1.GetIntelligence(), hero1.EquippedWeapon != null ? hero1.EquippedWeapon.Name : "None", hero1.HP, hero1.MP);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(hero2.Name);
            Console.ResetColor();
            Console.WriteLine("  Str: {0}   Dex: {1}   Int: {2}   Weapon: {3}   HP: {4}   MP: {5}  ", hero2.GetStrength(), hero2.GetDexterity(), hero2.GetIntelligence(), hero2.EquippedWeapon != null ? hero2.EquippedWeapon.Name : "None", hero2.HP, hero2.MP);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Its your opponents turn.");
            Console.ResetColor();
            hero2.AutomatedOpponent(hero1);
          }
        }


        else
        {
          if (tour == 1)
          {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(hero1.Name);
            Console.ResetColor();
            Console.WriteLine("  Str: {0}   Dex: {1}   Int: {2}   Weapon: {3}   HP: {4}   MP: {5}  ", hero1.GetStrength(), hero1.GetDexterity(), hero1.GetIntelligence(), hero1.EquippedWeapon != null ? hero1.EquippedWeapon.Name : "None", hero1.HP, hero1.MP);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(hero2.Name);
            Console.ResetColor();
            Console.WriteLine("  Str: {0}   Dex: {1}   Int: {2}   Weapon: {3}   HP: {4}   MP: {5}  ", hero2.GetStrength(), hero2.GetDexterity(), hero2.GetIntelligence(), hero2.EquippedWeapon != null ? hero2.EquippedWeapon.Name : "None", hero2.HP, hero2.MP);
            Console.WriteLine();
            Console.Write("Your Turn, ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(hero1.Name);
            Console.ResetColor();
            Console.WriteLine(".");


          }
          else
          {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(hero1.Name);
            Console.ResetColor();
            Console.WriteLine("  Str: {0}   Dex: {1}   Int: {2}   Weapon: {3}   HP: {4}   MP: {5}  ", hero1.GetStrength(), hero1.GetDexterity(), hero1.GetIntelligence(), hero1.EquippedWeapon != null ? hero1.EquippedWeapon.Name : "None", hero1.HP, hero1.MP);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(hero2.Name);
            Console.ResetColor();
            Console.WriteLine("  Str: {0}   Dex: {1}   Int: {2}   Weapon: {3}   HP: {4}   MP: {5}  ", hero2.GetStrength(), hero2.GetDexterity(), hero2.GetIntelligence(), hero2.EquippedWeapon != null ? hero2.EquippedWeapon.Name : "None", hero2.HP, hero2.MP);
            Console.WriteLine();
            Console.Write("Your Turn, ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(hero2.Name);
            Console.ResetColor();
            Console.WriteLine(".");

          }

          int opt1 = ValidOption("1:Attack, 2:Spell, 3:Other actions ", 1, 3);
          switch (opt1)
          {
            case 1:
              if (tour == 1)
              {
                hero1.AttackWithWeapon(hero2);
                Console.WriteLine();
              }
              else { hero2.AttackWithWeapon(hero1); Console.WriteLine(); }
              break;

            case 2:
              if (tour == 1) { hero1.Spell(hero2); Console.WriteLine(); }
              else { hero2.Spell(hero1); Console.WriteLine(); }
              break;

            case 3:
              if (tour == 1) { hero1.Actions(hero2); Console.WriteLine(); }
              else { hero2.Actions(hero1); Console.WriteLine(); }
              break;

          }
        }
        if (tour == 1) hero1.Regeneration();
        else hero2.Regeneration();




        tour++;
        if (tour > 2) tour = 1;
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("Press any key to continue...");
        ConsoleKeyInfo key = Console.ReadKey();
        Console.ResetColor();
        Console.Clear();



      }
      if (Hero.HeroSelection.playeralone == false)
      {
        if (hero1.HP <= 0 || hero2.HP <= 0)
        {
          Console.ForegroundColor = ConsoleColor.DarkGreen;
          Console.WriteLine($"{hero1.Name} emerged victorious in their fight against {hero2.Name}");
          Console.WriteLine(" CCCC    OOOOO  NN   NN   GGGG  RRRRRR    AAA   TTTTTTT UU   UU LL        AAA   TTTTTTT IIIII  OOOOO  NN   NN  SSSSS         YY   YY  OOOOO  UU   UU    WW      WW IIIII NN   NN !!! ");
          Console.WriteLine("CC      OO   OO NNN  NN  GG  GG RR   RR  AAAAA    TTT   UU   UU LL       AAAAA    TTT    III  OO   OO NNN  NN SS             YY   YY OO   OO UU   UU    WW      WW  III  NNN  NN !!! ");
          Console.WriteLine("CC      OO   OO NN N NN GG      RRRRRR  AA   AA   TTT   UU   UU LL      AA   AA   TTT    III  OO   OO NN N NN  SSSSS          YYYYY  OO   OO UU   UU    WW   W  WW  III  NN N NN !!! ");
          Console.WriteLine("CC    C OO   OO NN  NNN GG   GG RR  RR  AAAAAAA   TTT   UU   UU LL      AAAAAAA   TTT    III  OO   OO NN  NNN      SS  ,       YYY   OO   OO UU   UU     WW WWW WW  III  NN  NNN     ");
          Console.WriteLine(" CCCCC   OOOO0  NN   NN  GGGGGG RR   RR AA   AA   TTT    UUUUU  LLLLLLL AA   AA   TTT   IIIII  OOOO0  NN   NN  SSSSS  ,,,      YYY    OOOO0   UUUUU       WW   WW  IIIII NN   NN !!! ");
          Console.WriteLine("                                                                                                                      ,,                                                             ");
          Console.ResetColor();
        }
      }
      else
      {
        if (hero1.HP <= 0)
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine($"{hero2.Name} has secured a clear victory upon {hero1.Name}!");
          Console.WriteLine(" ██████╗  █████╗ ███╗   ███╗███████╗      ██████╗ ██╗   ██╗███████╗██████╗ ");
          Console.WriteLine("██╔════╝ ██╔══██╗████╗ ████║██╔════╝     ██╔═══██╗██║   ██║██╔════╝██╔══██╗");
          Console.WriteLine("██║  ███╗███████║██╔████╔██║█████╗       ██║   ██║██║   ██║█████╗  ██████╔╝");
          Console.WriteLine("██║   ██║██╔══██║██║╚██╔╝██║██╔══╝       ██║   ██║██║   ██║██╔══╝  ██╔══██╗");
          Console.WriteLine("╚██████╔╝██║  ██║██║ ╚═╝ ██║███████╗     ╚██████╔╝╚██████╔╝███████╗██║  ██║");
          Console.WriteLine(" ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═╝╚══════╝      ╚══▀▀═╝  ╚═════╝ ╚══════╝╚═╝  ╚═╝");
          Console.ResetColor();
        }
        else if (hero2.HP <= 0)
        {
          Console.ForegroundColor = ConsoleColor.DarkGreen;
          Console.WriteLine($"{hero1.Name} emerged victorious in their fight against {hero2.Name}");
          Console.WriteLine(" CCCC    OOOOO  NN   NN   GGGG  RRRRRR    AAA   TTTTTTT UU   UU LL        AAA   TTTTTTT IIIII  OOOOO  NN   NN  SSSSS         YY   YY  OOOOO  UU   UU    WW      WW IIIII NN   NN !!! ");
          Console.WriteLine("CC      OO   OO NNN  NN  GG  GG RR   RR  AAAAA    TTT   UU   UU LL       AAAAA    TTT    III  OO   OO NNN  NN SS             YY   YY OO   OO UU   UU    WW      WW  III  NNN  NN !!! ");
          Console.WriteLine("CC      OO   OO NN N NN GG      RRRRRR  AA   AA   TTT   UU   UU LL      AA   AA   TTT    III  OO   OO NN N NN  SSSSS          YYYYY  OO   OO UU   UU    WW   W  WW  III  NN N NN !!! ");
          Console.WriteLine("CC    C OO   OO NN  NNN GG   GG RR  RR  AAAAAAA   TTT   UU   UU LL      AAAAAAA   TTT    III  OO   OO NN  NNN      SS  ,       YYY   OO   OO UU   UU     WW WWW WW  III  NN  NNN     ");
          Console.WriteLine(" CCCCC   OOOO0  NN   NN  GGGGGG RR   RR AA   AA   TTT    UUUUU  LLLLLLL AA   AA   TTT   IIIII  OOOO0  NN   NN  SSSSS  ,,,      YYY    OOOO0   UUUUU       WW   WW  IIIII NN   NN !!! ");
          Console.WriteLine("                                                                                                                      ,,                                                             ");
          Console.ResetColor();
        }
      }
    }
  }
}















// minimum: 
//- 3 spelle                                                              | DONE
//- dodac regeneracje na runde                                            | DONE
//- dodac ekran wygranej                                                  | DONE

// pomysly:
// nowe klasy                                                             | DONE
// bronie, itemy                                                          | DONE
// mozliwosc wyboru nazwy postaci/klasy przez uzytkownika                 | DONE
// ladniejszy hud                                                         | DONE
// odpoczynek  na runde                                                   | DONE
// ukryty ending (nieudany spell)                                         | DONE
// mozliwosc ucieczki z walki                                             | DONE
// usuwajace sie wiadomosci z konsoli                                     | DONE
// wiecej opisow walki                                                    | DONE
// opisy klas bohaterow                                                   | DONE
// poszukiwanie broni w otoczeniu                                         | DONE
// DODAC ZAPIS, 13. JSON NA GITHUB                                        |
// zautomatyzowany przeciwnik                                             | DONE
