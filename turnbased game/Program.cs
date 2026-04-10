// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography;
int hp = 100;
int hpMax = 100;
int kills = 0;
int TotalXp = 0;
List<int> enemyHp = new List<int>();
int damage = 10;
int enemyDamage = 5;
int level = 1;
int enemyLevel = 1;
int xp = 0;
int needXp = 50;
int gold = 10;
int playerChose1;
int playerChose2;
int bowShots = 3;
bool charge = false;
bool denfending = false;
bool changed = false;
List<int> enemyList = new List<int>();
Random rand = new Random();
string[] attacks = { "Sword", "Bow", "Defend", "Charge", "Heal", "Save" };
string[] enemys = { "Skeleton", "Bandit", "Ork" };
string[] enemyMoves = { "Stab", "Light Swing", "Strong Swing", "Run", "Summon" };
string getStats;

Console.WriteLine("Wellcome User have you played before?");
Console.WriteLine("Y/N");
getStats = Console.ReadLine();
if (getStats == "Y" || getStats == "y")
{
    PlayerGetStats();
}
else if (getStats == "N" || getStats == "n")
{
    Console.WriteLine("A Bandit stoped you");
    enemyList.Add(1);
    enemyHp.Add(50);
    PrintHud();
}
while (hp > 0)
{
    PrintHud();
    OnPlayerTurn();
    OnPLayerTurnOver();
}
void PrintHud()
{
    Thread.Sleep(1000);
    Console.Clear();
    Console.WriteLine($"|Level: {level} XP:{xp}/{needXp}| |Hp:{hp}/{100 * level}| |Gold:{gold}| |Damage:({damage}/{10 * level})| |Bow shots:{bowShots}|  |Enemy Level:{enemyLevel}|");
    for (int i = 0; i < 2; i++)
    {
        Console.WriteLine();
    }
}
void PlayerGetStats()
{
    if (File.Exists("hp.txt") && File.Exists("lv.txt") && File.Exists("xp.txt") && File.Exists("gold.txt") && File.Exists("enemylv.txt") && File.Exists("enemylist.txt") && File.Exists("enemyhp.txt"))
    {
        Console.WriteLine("Geting stats.");
        getStats = File.ReadAllText("hp.txt");
        hp = int.Parse(getStats); Console.WriteLine("HP stat set to " + hp);
        getStats = File.ReadAllText("lv.txt");
        level = int.Parse(getStats);
        Console.WriteLine("Leval stat set to " + level);
        getStats = File.ReadAllText("xp.txt");
        xp = int.Parse(getStats);
        Console.WriteLine("XP stat set to " + xp);
        getStats = File.ReadAllText("gold.txt");
        gold = int.Parse(getStats); Console.WriteLine("Gold stat set to " + gold);
        needXp = level * 100; getStats = File.ReadAllText("enemylv.txt");
        enemyLevel = int.Parse(getStats);
        string enemyStats = File.ReadAllText("enemylist.txt");
        string[] enemies = enemyStats.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (string enemy in enemies)
        {
            enemyList.Add(int.Parse(enemy));
        }
        enemyStats = "";
        enemyStats = File.ReadAllText("enemyhp.txt");
        enemies = enemyStats.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (string enemy in enemies)
        {
            enemyHp.Add(int.Parse(enemy));
        }
        PrintHud();
    }
    else
    {
        Console.WriteLine("No save found starting new");
        Console.WriteLine("A Bandit stoped you"); enemyList.Add(1); enemyHp.Add(50); PrintHud();
    }
}
void PlayerSaveStats()
{
    Console.WriteLine("Saving stats...");
    getStats = $"{hp}";
    File.WriteAllText("hp.txt", getStats);
    getStats = File.ReadAllText("hp.txt");
    getStats = $"{level}"; File.WriteAllText("lv.txt", getStats);
    getStats = File.ReadAllText("lv.txt");
    getStats = $"{xp}";
    File.WriteAllText("xp.txt", getStats);
    getStats = File.ReadAllText("xp.txt");
    getStats = $"{gold}";
    File.WriteAllText("gold.txt", getStats);
    getStats = File.ReadAllText("gold.txt");
    getStats = $"{enemyLevel}";
    File.WriteAllText("enemylv.txt", getStats);
    File.WriteAllText("enemyList.txt", string.Join(" ", enemyList));
    File.WriteAllText("enemyhp.txt", string.Join(" ", enemyHp));
    Console.WriteLine("Stats saved.");
}
void OnPLayerTurnOver()
{
    int rndspawn = rand.Next(0, 5);
    if (enemyList.Count == 0 || rndspawn == 1)
    {
        int spawn = rand.Next(0, 3);
        if (spawn == 0)
        {
            Console.WriteLine("A Skelton came out of the ground");
            enemyList.Add(spawn); enemyHp.Add(25);
        }
        else if (spawn == 1)
        {
            Console.WriteLine("A Bandit came out of the Forest");
            enemyList.Add(spawn); enemyHp.Add(50);
        }
        else if (spawn == 2)
        {
            Console.WriteLine("A Ork came out of a dungone");
            enemyList.Add(spawn); enemyHp.Add(100);
        }
    }
    for (int i = enemyList.Count - 1; i >= 0; i--)
    {
        int move = rand.Next(0, 5);
        if (move <= 2 && !denfending)
        {
            Console.WriteLine($"{enemys[enemyList[i]]} used {enemyMoves[move]}");
            hp = hp - (enemyLevel * (enemyDamage * move)); Console.WriteLine($"You took {enemyLevel * (enemyDamage * move)} damage");
        }
        else if (move == 3)
        {
            Console.WriteLine($"{enemys[enemyList[i]]} used {enemyMoves[move]} and got away");
            enemyList.RemoveAt(i);
            enemyHp.RemoveAt(i);
        }
        else if (move == 4 && enemys[enemyList[i]] == "Skeleton")
        {
            Console.WriteLine($"{enemys[enemyList[i]]} used {enemyMoves[move]} and Summoned another {enemys[0]}");
            enemyList.Add(0);
            enemyHp.Add(25);
        }
        else if (move >= 4 || move == 4 && enemys[enemyList[i]] != "Skeleton")
        { Console.WriteLine($"{enemys[enemyList[i]]} did nothing"); }
        else { Console.WriteLine("You defende their attack attempt"); }
    }
    denfending = false;
}
void OnPlayerTurn()
{
    if (changed && !charge)
    { damage -= 10; changed = false; }
    Console.WriteLine("Your turn");
    Console.WriteLine("Your moves");
    for (int i = 0; i < attacks.Length; i++)
    { Console.Write($" ({i + 1} {attacks[i]})"); }
    Console.WriteLine(""); Console.WriteLine("Enemys and their hp");
    for (int i = 0; i < enemyList.Count; i++) { Console.Write($" ({i + 1} {enemys[enemyList[i]]} hp:{enemyHp[i]})"); }
    Console.WriteLine();
    Console.WriteLine("Choose move(max 6) and target choose 0 to skip enemuy like this: 1 [press enter] 1");
    Console.WriteLine("will not use moves 3 to 6 on enemy");
    playerChose1 = int.Parse(Console.ReadLine());
    playerChose2 = int.Parse(Console.ReadLine());
    if (playerChose2 != 0)
    {
        Console.WriteLine($"Using {attacks[playerChose1 - 1]} on {enemys[enemyList[playerChose2 - 1]]} ");
        if (playerChose1 == 1)
        {
            enemyHp[playerChose2 - 1] = enemyHp[playerChose2 - 1] - damage * level;
            ; charge = false;
        }
        else if (playerChose1 == 2) { enemyHp[playerChose2 - 1] -= damage * level * bowShots; charge = false; }
    }
    else if (playerChose1 <= 6)
    {
        Console.WriteLine($"useing move {attacks[playerChose1 - 1]} on self");
        if (playerChose1 == 3) { denfending = true; }
        else if (playerChose1 == 4) { damage += 10; charge = true; changed = true; }
        else if (playerChose1 == 5) { hp += 50; }
        else if (playerChose1 == 6) { PlayerSaveStats(); }
    }
    else if (playerChose1 > 6)
    { Console.WriteLine("No move found skiping turn"); }
    else if (playerChose2 < 0 || playerChose2 >= enemyList.Count)
    { Console.WriteLine("No target found skiping turn"); }
    else { Console.WriteLine("ERROR invalid choice skiping turn"); }
    if (playerChose2 > 0 && playerChose2 <= enemyHp.Count)
    {
        if (enemyHp[playerChose2 - 1] <= 0)
        {
            Console.WriteLine($"You killed {enemys[enemyList[playerChose2 - 1]]} +10xp +5gold");
            xp += 10;
            gold += 5;
            kills++;
            enemyList.RemoveAt(playerChose2 - 1);
            enemyHp.RemoveAt(playerChose2 - 1);
        }
    }
    if (needXp <= xp) { Console.WriteLine("Level up!"); level += 1; TotalXp += xp; xp -= needXp; needXp = 100 * level; hp += 100; if (!charge) { damage += 10 * level; } else { damage -= 10; damage += 10 * level; damage += 10; } enemyLevel++; }
    if (hp <= 0)
    {
        Console.WriteLine($"You lost. Score: (Gold{gold}) (Total XP {TotalXp}) (Enmays killed {kills})");
        Console.WriteLine("Stats are not saved when you lose so can retry from last save"); onlose();
    }
}
void onlose()
{
    Console.WriteLine("choose 1 and enter to retry or just enter to end game");
    playerChose1 = int.Parse(Console.ReadLine());
    if (playerChose1 == 1)
    {
        PlayerGetStats(); playerChose1 = 0;
    }
}