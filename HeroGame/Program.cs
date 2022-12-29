﻿//поле - двумерный массив размера N на M
//портал - объект, пренадлежит полю
//стены - объекты, прендалежат полю, по краям поля сделать стены
//герой - объект, НЕ пренадлежит полю, рисуется поверх полю

//1) генерация поля и стен на нём
//2) генерация портала в поле (перандомировать поле если мы оказались заперты)
//3) передвижение героя по полю
//4) вход героя в портал и переход к шагу 1


using HeroGame;

Random random = new Random();

int currentLevel = 1;
int rows, cols;
Cell[,] field;

int iHero, jHero;
int hpHero = 1;
char skinHero = (char)Constants.AliveHeroSkin;
// int iDog, jDog;

bool heroInAdventure;

int currentWallPercent = (int)Constants.WallPercent;

int[,] dogs = new int[10, 2];

while (true)
{
    rows = random.Next((int)Constants.MinRows, (int)Constants.MaxRows - 1);
    cols = random.Next((int)Constants.MinCols, (int)Constants.MaxCols - 1);

    field = new Cell[rows, cols];

    for (int i = 0; i < rows; i++)
    {
        for (int j = 0; j < cols; j++)
        {
            field[i, j] = Cell.Empty;
        }
    }

    for (int i = 0; i < rows; i++)
    {
        field[i, 0] = Cell.Bound;
        field[i, cols - 1] = Cell.Bound;
    }

    for (int j = 0; j < cols; j++)
    {
        field[0, j] = Cell.Bound;
        field[rows - 1, j] = Cell.Bound;
    }


    iHero = (int)Constants.StartIHero;
    jHero = (int)Constants.StartJHero;

    for (int k = 0; k < dogs.GetLength(0); k++)
    {
        dogs[k, 0] = random.Next(2, rows - 2);
        dogs[k, 1] = random.Next(2, cols - 2);
    }

    // iDog = (int)Constants.StartIDog;
    // jDog = (int)Constants.StartJDog;

    int iPortal, jPortal;
    do
    {
        iPortal = random.Next(1, rows - 1);
        jPortal = random.Next(1, cols - 1);
    } while (iPortal == iHero && jPortal == jHero);

    field[iPortal, jPortal] = Cell.Portal;

    int countWalls = (int)((rows - 2) * (cols - 2) * currentWallPercent / 100.0);
    for (int i = 0; i < countWalls; i++)
    {
        int iWall, jWall;
        do
        {
            iWall = random.Next(1, rows - 1);
            jWall = random.Next(1, cols - 1);
        } while (iWall == iHero && jWall == jHero
                 || field[iWall, jWall] == Cell.Portal
                 || field[iWall, jWall] == Cell.Wall);

        field[iWall, jWall] = Cell.Wall;
    }

    heroInAdventure = true;
    while (heroInAdventure)
    {
        Console.Clear();

        Console.ResetColor();

        Console.WriteLine($"Current Level = {currentLevel} HP hero = {hpHero}");

        bool findDog;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                findDog = false;
                for (int k = 0; k < dogs.GetLength(0); k++)
                {
                    if (i == dogs[k, 0] && j == dogs[k, 1])
                    {
                        findDog = true;
                        break;
                    }
                }

                if (findDog)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write((char)Constants.DogSkin);
                }
                else if (i == iHero && j == jHero)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(skinHero);
                }
                else
                {
                    switch (field[i, j])
                    {
                        case Cell.Empty:
                            Console.ForegroundColor = ConsoleColor.Gray;
                            break;
                        case Cell.Wall:
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            break;
                        case Cell.Portal:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        case Cell.Bound:
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                    }

                    Console.Write((char)field[i, j]);
                }
            }

            Console.WriteLine();
        }

        ConsoleKey key = Console.ReadKey(false).Key;
        switch (key)
        {
            case ConsoleKey.A:
                if (field[iHero, jHero - 1] == Cell.Empty || field[iHero, jHero - 1] == Cell.Portal)
                {
                    jHero--;
                    //jDog = random.Next(1, cols - 1);
                }

                break;

            case ConsoleKey.W:
                if (field[iHero - 1, jHero] == Cell.Empty || field[iHero - 1, jHero] == Cell.Portal)
                {
                    iHero--;
                    //iDog = random.Next(1, rows - 1);
                }

                break;

            case ConsoleKey.D:
                if (field[iHero, jHero + 1] == Cell.Empty || field[iHero, jHero + 1] == Cell.Portal)
                {
                    jHero++;
                    //jDog = random.Next(1, cols - 1);
                }

                break;

            case ConsoleKey.S:
                if (field[iHero + 1, jHero] == Cell.Empty || field[iHero + 1, jHero] == Cell.Portal)
                {
                    iHero++;
                    //iDog = random.Next(1, rows - 1);
                }

                break;

            case ConsoleKey.R:
                currentLevel = 0;
                heroInAdventure = false;
                break;
        }

        for (int k = 0; k < dogs.GetLength(0); k++)
        {
            dogs[k, 0] = random.Next(2, rows - 2);
            dogs[k, 1] = random.Next(2, cols - 2);
        }

        if (field[iHero, jHero] == Cell.Portal)
        {
            currentLevel++;
            //currentWallPercent += 5;
            heroInAdventure = false;
        }

        for (int k = 0; k < dogs.GetLength(0); k++)
        {
            if (iHero == dogs[k, 0] && jHero == dogs[k, 1])
            {
                hpHero--;
                break;
            }
        }

        if (hpHero == 0)
        {
            skinHero = (char)Constants.DeadHeroSkin;
        }
        
    }
}