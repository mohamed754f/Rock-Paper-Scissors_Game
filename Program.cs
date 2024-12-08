using System.Reflection;

namespace Rock_Paper_Scissors_Game
{
    internal class Program
    {
        enum enGameChoice { Stone = 1, Paper = 2, Scissors = 3 };
        enum enWinner { Player = 1 ,Computer = 2, Drow = 3 };

        struct stRoundInfo
        {
            public int RoundNumber;
            public enGameChoice PlayerChoice;
            public enGameChoice ComputerChoice;
            public enWinner Winner;
            public string WinnerName;
        }
        struct stGameResult
        {
            public short GameRounds;
            public short PlayerWinTimes;
            public short ComputerWinTimes;
            public short DrowTimes;
            public enWinner GameWinner;
            public string WinnerName;
        }

        static Random random = new Random();
        static int RandomNumber(int from, int to)
        {
            return random.Next(from, to + 1);
        }

        static enWinner WhoWonTheRound(stRoundInfo RoundInfo)
        {
            if (RoundInfo.PlayerChoice == RoundInfo.ComputerChoice)
            {
                return enWinner.Drow;
            }
            
            switch(RoundInfo.PlayerChoice)
            {
                case enGameChoice.Stone:
                    if(RoundInfo.ComputerChoice == enGameChoice.Paper)
                        return enWinner.Computer;
                    break;
                case enGameChoice.Paper:
                    if(RoundInfo.ComputerChoice == enGameChoice.Scissors)
                        return enWinner.Computer;
                    break;
                case enGameChoice.Scissors:
                    if(RoundInfo.ComputerChoice == enGameChoice.Stone)
                        return enWinner.Computer;
                    break;
                    
            }
            return enWinner.Player;
        }
        static enWinner WhoWonTheGame(int player1WinTimes, int computerWinTimes)
        {
            if (player1WinTimes > computerWinTimes)
                return enWinner.Player;
            else if (computerWinTimes > player1WinTimes)
                return enWinner.Computer;
            else
                return enWinner.Drow;
        }
        static enGameChoice ReadPlayerChoice()
        {
            int choice;
            do
            {
                Console.WriteLine("\nYour Choice: [1]:Stone, [2]:Paper, [3]:Scissors?");
                choice = Convert.ToInt32(Console.ReadLine());
            } while (choice < 1 || choice > 3);

            return (enGameChoice)choice;
        }
        static enGameChoice GetComputerChoice()
        {
            return (enGameChoice)RandomNumber(1, 3);
        }
        static string GetWinnerName(enWinner winner)
        {
            string[] winnerNames = { "Player1", "Computer", "No Winner" };
            return winnerNames[(int)winner - 1];
        }
        static string GetChoiceName(enGameChoice choice)
        {
            string[] gameChoices = { "Stone", "Paper", "Scissors" };
            return gameChoices[(int)choice - 1];
        }
        static void SetWinnerScreenColor(enWinner winner)
        {
            switch (winner)
            {
                case enWinner.Player:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case enWinner.Computer:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Beep();
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
            }
        }
        static void PrintRoundResults(stRoundInfo roundInfo)
        {
            Console.WriteLine($"\n____________Round [{roundInfo.RoundNumber}]____________\n");
            Console.WriteLine($"Player Choice: {GetChoiceName(roundInfo.PlayerChoice)}");
            Console.WriteLine($"Computer Choice: {GetChoiceName(roundInfo.ComputerChoice)}");
            Console.WriteLine($"Round Winner : [{roundInfo.WinnerName}]");
            Console.WriteLine("__________________________________\n");
            SetWinnerScreenColor(roundInfo.Winner);
        }
        static stGameResult FillGameResults(int gameRounds, int playerWinTimes, int computerWinTimes, int drowTimes)
        {
            stGameResult gameResult = new stGameResult();
            gameResult.GameRounds = (short)gameRounds;
            gameResult.PlayerWinTimes = (short)playerWinTimes;
            gameResult.ComputerWinTimes = (short)computerWinTimes;
            gameResult.DrowTimes = (short)drowTimes;
            gameResult.GameWinner = WhoWonTheGame(playerWinTimes, computerWinTimes);
            gameResult.WinnerName = GetWinnerName(gameResult.GameWinner);
            return gameResult;
        }
        static stGameResult PlayGame(int HowManyRounds)
        {
            stRoundInfo RoundInfo = new stRoundInfo();
            int player1WinTimes = 0, computerWinTimes = 0, drawTimes = 0;
            for (int gameRound =1;gameRound<= HowManyRounds;gameRound++)
            {
                Console.WriteLine($"\nRound [{gameRound}] begins:\n");
                RoundInfo.RoundNumber = gameRound;
                RoundInfo.PlayerChoice = ReadPlayerChoice();
                RoundInfo.ComputerChoice = GetComputerChoice();
                RoundInfo.Winner = WhoWonTheRound(RoundInfo);
                RoundInfo.WinnerName = GetWinnerName(RoundInfo.Winner);

                if (RoundInfo.Winner == enWinner.Player)
                    player1WinTimes++;
                else if (RoundInfo.Winner == enWinner.Computer)
                    computerWinTimes++;
                else
                    drawTimes++;
                PrintRoundResults(RoundInfo);
            }
            return FillGameResults(HowManyRounds, player1WinTimes, computerWinTimes, drawTimes);
        }
        static void ShowGameOverScreen()
        {
            Console.WriteLine("\t__________________________________________________________");
            Console.WriteLine("\t+++ G a m e O v e r+++");
            Console.WriteLine("\t__________________________________________________________");
        }

        static void ShowFinalGameResults(stGameResult gameResult)
        {
            Console.WriteLine("\t_____________________ [Game Results]_____________________");
            Console.WriteLine($"\tGame Rounds : {gameResult.GameRounds}");
            Console.WriteLine($"\tPlayer1 won times : {gameResult.PlayerWinTimes}");
            Console.WriteLine($"\tComputer won times : {gameResult.ComputerWinTimes}");
            Console.WriteLine($"\tDraw times : {gameResult.DrowTimes}");
            Console.WriteLine($"\tFinal Winner : {gameResult.WinnerName}");
            Console.WriteLine("\t___________________________________________________________");
            SetWinnerScreenColor(gameResult.GameWinner);
        }

        static int ReadHowManyRounds()
        {
            int gameRounds;
            do
            {
                Console.WriteLine("How Many Rounds 1 to 10?");
                gameRounds = Convert.ToInt32(Console.ReadLine());
            } while (gameRounds < 1 || gameRounds > 10);

            return gameRounds;
        }

        static void ResetScreen()
        {
            Console.Clear();
            Console.ResetColor();
        }

        static void StartGame()
        {
            char playAgain = 'Y';
            do
            {
                ResetScreen();
                stGameResult gameResult = PlayGame(ReadHowManyRounds());
                ShowGameOverScreen();
                ShowFinalGameResults(gameResult);
                Console.WriteLine("\n\tDo you want to play again? Y/N?");
                playAgain = Convert.ToChar(Console.ReadLine().ToUpper());
            } while (playAgain == 'Y');
        }
        static void Main(string[] args)
        {
            StartGame();
        }
    }
}
