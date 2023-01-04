// the starting grids
char[,] compGrid = { { 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o' }, { 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o' }, { 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o' }, { 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o' }, { 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o' }, { 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o' }, { 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o' }, { 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o' } };
char[,] playerGrid = { { 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o' }, { 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o' }, { 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o' }, { 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o' }, { 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o' }, { 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o' }, { 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o' }, { 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o' } };
// the grid that the player sees to help them make their moves:
char[,] tryingGrid = { { 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o' }, { 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o' }, { 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o' }, { 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o' }, { 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o' }, { 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o' }, { 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o' }, { 'o', 'o', 'o', 'o', 'o', 'o', 'o', 'o' } };
// the coordinates that have been tried
List<string> compTries = new List<string>();
List<string> playerTries = new List<string>();
// the coordinates that have been picked
List<string> playerBoatCoors = new List<string>();
List<string> computerBoatCoors = new List<string>();

void Main()
{
    // options to start a new game, resume a game, read the instructions or quit the game
    Console.WriteLine("Enter your option using the corresponding number");
    Console.WriteLine("1: New Game\n2: Resume a game\n3: Read Instructions\n4: Quit Game");
    try 
    { 
        int option = Convert.ToInt32(Console.ReadLine());
        if (option == 1) { NewGame(); }
        else if (option == 2) { ResumeGame(); }
        else if (option == 3) { ReadInstructions(); }
        else if (option == 4) { QuitGame(); }
        else { Console.WriteLine("Invalid option"); Main(); }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Invalid option");
        Main();
    }
}
void NewGame()
{
    GenerateGame();
    Boolean winnerYet = Winner();
    Console.WriteLine("Your chosen coordinates have been stored and the computer has chosen their target coordinates. Time to make your first attack!\nDisplayed below is a representation of the computer's grid.\nIf you hit one of its boats the 'o' will be replaced with an 'H'\nIf you miss, the 'o' will be replaced with an 'M'");
    while(winnerYet == false)
    {
        P1Turn();
        winnerYet = Winner();
        CompTurn();
        winnerYet = Winner();
        save();
    }
    Main();
}
void P1Turn()
{
    // display the trying board
    Console.WriteLine(" ABCDEFGH");
    for(int i = 0; i < tryingGrid.GetLength(0); i++)
    {
        Console.Write(i + 1);
        for(int j = 0; j < tryingGrid.GetLength(1); j++)
        {
            if (tryingGrid[i, j] == 'o') { Console.BackgroundColor = ConsoleColor.Blue; }
            else if (tryingGrid[i, j] == 'B') { Console.BackgroundColor = ConsoleColor.Gray; Console.ForegroundColor = ConsoleColor.Black; }
            else if (tryingGrid[i, j] == 'H') { Console.BackgroundColor = ConsoleColor.Green; Console.ForegroundColor = ConsoleColor.Black; }
            else if (tryingGrid[i, j] == 'M') { Console.BackgroundColor = ConsoleColor.Red; Console.ForegroundColor = ConsoleColor.Black; }
            Console.Write(tryingGrid[i, j]);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }
        Console.WriteLine();
    }
    // ask for target coors, if coors already chosen ask again
    string target;
    int checker = 0; // if checker is > 0 then the coordinates have been entered already
    do
    {
        if(checker > 0) { Console.WriteLine("Invalid coordinates. Enter a different coordinate"); }
        Console.WriteLine("Enter your target coordinates: ");
        target = Console.ReadLine();
        if(target == "quit") { QuitGame(); }
        checker++;
    } while (target.Length != 2 || playerTries.Contains(String.Concat(Convert.ToInt16(target[0]) - 65, Convert.ToInt16(target[1]) - 49)) || Convert.ToInt16(target[0]) - 65 < 0 || Convert.ToInt16(target[0]) - 65 > 7 || Convert.ToInt16(target[1]) - 49 < 0 || Convert.ToInt16(target[1]) - 49 > 7);
    // the coordinate is given as a letter and needs to be used as a number, this converts the letter to a number
    int targetNumberConvert = Convert.ToInt16(target[0]) - 65;
    string targetNumber = targetNumberConvert.ToString() + (Convert.ToInt16(target[1]) - 49);
    playerTries.Add(targetNumber);
    checker = 0;

    // comparing player tries to the computer chosen coors
    bool hit = false;
    foreach(string actualCoors in computerBoatCoors)
    {
        if(actualCoors == playerTries[playerTries.Count - 1])
        {
            hit = true;
            computerBoatCoors.Remove(actualCoors); // remove the coordinate from the list of coordinates of the computer's boat. if this list is empty the player has won
            break;
        }
    }
    if (hit)
    {
        tryingGrid[Convert.ToInt16(playerTries[playerTries.Count - 1][1]) - 48, Convert.ToInt16(playerTries[playerTries.Count - 1][0]) - 48] = 'H';
        Console.WriteLine("You hit a boat!");
    }
    else
    {
        tryingGrid[Convert.ToInt16(playerTries[playerTries.Count - 1][1]) - 48, Convert.ToInt16(playerTries[playerTries.Count - 1][0]) - 48] = 'M';
        Console.WriteLine("You missed the boats");
    }
}
Boolean Winner() {
    // set boolean variables to true. if there are any boats left ('B') on a player's opponent's board, they haven't won so change to false  
    Boolean compWin = false;
    Boolean playerWin = false;
    if (computerBoatCoors.Count == 0)
    {
        playerWin = true;
    }
    else if(playerBoatCoors.Count == 0)
    {
        compWin = true;
    }
    if (playerWin)
    {
        Console.WriteLine("You won!");
        return true;
    }
    if (compWin)
    {
        Console.WriteLine("The computer wins, you have lost");
        return true;
    }
    return false;
}

void CompTurn()
{
    // generate the computer's guess for coordinates
    Random rand = new Random();
    string targetCoors;
    do
    {
        targetCoors = string.Concat(rand.Next(0, 8), rand.Next(0, 8));
    }while(compTries.Contains(targetCoors));
    compTries.Add(targetCoors);
    Console.WriteLine($"The computer chose these coordinates: {Convert.ToChar(Convert.ToInt16(targetCoors[0]) + 17)}{targetCoors[1] - 47}");

    // check if this is a hit or a miss
    bool hit = false;
    foreach(string actualCoors in playerBoatCoors)
    {
        if(actualCoors == compTries[compTries.Count - 1])
        {
            hit = true;
            playerBoatCoors.Remove(actualCoors); // the same principle as in the P1 turn method
            break;
        }
    }
    if (hit)
    {
        playerGrid[Convert.ToInt16(compTries[compTries.Count - 1][1]) - 48, Convert.ToInt16(compTries[compTries.Count - 1][0]) - 48] = 'H';
        Console.WriteLine("The computer hit your boat");
    }
    else
    {
        Console.WriteLine("The computer missed your boat");
    }
}
void GenerateGame()
{
    // choosing the player's 5 coordinates
    string coors;
    int entered = 0;
    for (int i = 0; i < 5; i++) // repeats 5 times so that 5 coordinates are chosen
    {
        do
        {
            if (entered > 0) { Console.WriteLine("Invalid coordinates. Enter different coordinates"); }
            Console.WriteLine($"Enter boat {i + 1} coordinates");
            coors = Console.ReadLine();
            // if entered is bigger than zero then the coordinates have already been entered
            entered++;
        } while (coors.Length != 2 || playerBoatCoors.Contains(String.Concat(Convert.ToInt16(coors[0]) - 65, Convert.ToInt16(coors[1]) - 49)) || Convert.ToInt16(coors[0]) - 65 < 0 || Convert.ToInt16(coors[0]) - 65 > 7 || Convert.ToInt16(coors[1]) - 49 < 0 || Convert.ToInt16(coors[1]) - 49 > 7);
        // the coordinate is given as a letter and needs to be used as a number, this converts the letter to a number
        int coorsNumberConvert = Convert.ToInt16(coors[0]) - 65;
        string coorsNumber = coorsNumberConvert.ToString() + (Convert.ToInt16(coors[1]) - 49);
        playerBoatCoors.Add(coorsNumber);
        entered = 0;
    }
    
    // choosing the computer's 5 coordinates
    Random rand = new Random();
    for(int i = 0; i < 5; i++)
    {
        do
        {
            coors = string.Concat(rand.Next(0,8), rand.Next(0,8));
        } while (computerBoatCoors.Contains(coors));
        computerBoatCoors.Add(coors);
    }

    // put the player's boats onto the board
    foreach (string coor in playerBoatCoors)
    {
        playerGrid[Convert.ToInt16(coor[1]) - 48, Convert.ToInt16(coor[0]) - 48] = 'B';
    }
    // put the computer's boats onto the board
    foreach(string coor in computerBoatCoors)
    {
        compGrid[Convert.ToInt16(coor[1]) - 48, Convert.ToInt16(coor[0]) - 48] = 'B';
    }
    Console.WriteLine("This is your grid:");
    Console.WriteLine(" ABCDEFGH");
    for(int i = 0; i < playerGrid.GetLength(0); i++)
    {
        Console.Write(i + 1);
        for(int j = 0; j < playerGrid.GetLength(1); j++)
        {
            if (playerGrid[i, j] == 'o') { Console.BackgroundColor = ConsoleColor.Blue; }
            else if(playerGrid[i, j] == 'B') { Console.BackgroundColor = ConsoleColor.Gray; Console.ForegroundColor = ConsoleColor.Black; }
            Console.Write(playerGrid[i, j]);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }
        Console.WriteLine();
    }
}
void ResumeGame()
{
    Console.WriteLine("gathering data from previous game...");
    string[] data = File.ReadAllLines("storage.txt"); // puts all lines of file into an array
    for (int i = 0; i < 64; i++)
    {
        compGrid[i / 8, i % 8] = data[0][i]; // floor division to find 1st dimension, modulus to find 2nd as increments each time by 1 from 0 to 7
        playerGrid[i / 8, i % 8] = data[1][i];
        tryingGrid[i / 8, i % 8] = data[2][i];
    }
    compTries = data[3].Split(" ").ToList(); // splits data into a list using a space as the delimiter
    playerTries = data[4].Split(" ").ToList();
    playerBoatCoors = data[5].Split(" ").ToList();
    computerBoatCoors = data[6].Split(" ").ToList();
    Console.WriteLine("Here is your game: ");
    // go back into game
    bool winnerYet = Winner();
    while (winnerYet == false)
    {
        P1Turn();
        winnerYet = Winner();
        CompTurn();
        winnerYet = Winner();
        save();
    }
    Main();
}
void ReadInstructions()
{
    Console.WriteLine("The aim of this game is to hit all of your opponent's boats on their 8x8 grid. \nYou will guess where the boats are by inputting coordinates \nFor example, the first square has coordinates A0.\nYour opponent will also be guessing where your boats are. The winner is the player that hits all of the other player's boats first.\nThere is an example of the grid below\nIf you want to quit during the game, typing 'quit'");
    Console.WriteLine(" ABCDEFGH\n1oooooooo\n2oooooooo\n3oooooooo\n4oooooooo\n5oooooooo\n6oooooooo\n7oooooooo\n8oooooooo");
    Main();
}
void QuitGame()
{
    save();
    Environment.Exit(0);
}

void save()
{
    File.WriteAllText("storage.txt", $"{String.Join("", compGrid.Cast<char>())}\n");
    File.AppendAllText("storage.txt", $"{String.Join("", playerGrid.Cast<char>())}\n");
    File.AppendAllText("storage.txt", $"{String.Join("", tryingGrid.Cast<char>())}\n");
    File.AppendAllText("storage.txt", $"{String.Join(" ", compTries)}\n");
    File.AppendAllText("storage.txt", $"{String.Join(" ", playerTries)}\n");
    File.AppendAllText("storage.txt", $"{String.Join(" ", playerBoatCoors)}\n");
    File.AppendAllText("storage.txt", $"{String.Join(" ", computerBoatCoors)}\n");

}

Main();
