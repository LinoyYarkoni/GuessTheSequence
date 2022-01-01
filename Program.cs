using System;

namespace A22_Ex02
{
    public class Program
    {
        public static void Main()
        {
            Run();
        }

        public static void Run()
        {
            bool isAnotherGame;
            do
            {
                eGameStatuses gameResult = RunGame();
                string isAnotherGameInput = string.Empty;
                if(gameResult != eGameStatuses.Quit)
                {
                    isAnotherGameInput = UI.StartAgainResult();
                }

                isAnotherGame = isAnotherGameInput.Equals("Y") || isAnotherGameInput.Equals("y");
            }
            while(isAnotherGame);
        }

        public static eGameStatuses RunGame()
        { 
            Console.Clear(); //// if the user plays another game
                                              
            const int k_StartOfGuessRange = 4;
            const int k_EndOfGuessRange = 10;
            const int k_MaxLengthOfComputerSequence = 4;
            const char k_StartOfGuessSeqRange = 'A';
            const char k_EndOfGuessSeqRange = 'H';

            int userNumberOfGuessInput = UI.GetUserNumberInRange(k_StartOfGuessRange, k_EndOfGuessRange, "Please enter the number of guesses");

            GameBoard game = new GameBoard(
                k_MaxLengthOfComputerSequence,
                k_StartOfGuessSeqRange,
                k_EndOfGuessSeqRange,
                userNumberOfGuessInput);
            UI.DisplayTable(game.NumberOfGuesses, game.UserGuessAndResult, game.MaxLengthOfComputerSequence);

            while(game.GameStatus == eGameStatuses.Playing)
            {
                char endExampleChar = (char)((int)game.StartOfGuessSeqRange + game.MaxLengthOfComputerSequence);
                string formatOfInput = StringService.GenerateRangeString(
                    game.StartOfGuessSeqRange,
                    endExampleChar,
                    "<",
                    ">");
                string requestGuessMessage = string.Format("Please enter your next guess {0} or 'Q' to quit", formatOfInput);
                string userGuessInput = UI.GetUserInput(requestGuessMessage);

                Console.Clear();
                if (userGuessInput.Equals("Q") || userGuessInput.Equals("q"))
                {
                    game.QuitGame();
                    break;
                }

                eValidStatuses validStatuses = game.AddGuess(userGuessInput);

                UI.DisplayTable(game.NumberOfGuesses, game.UserGuessAndResult, game.MaxLengthOfComputerSequence);

                if(validStatuses != eValidStatuses.Valid)
                {
                    string incorrectMessage = "Incorrect Input. ";
                    switch(validStatuses)
                    {
                        case eValidStatuses.NotInRange:
                            endExampleChar = (char)((int)game.StartOfGuessSeqRange + (game.EndOfGuessSeqRange - game.StartOfGuessSeqRange) + 1);
                            string possibleRangeString = StringService.GenerateRangeString(
                                game.StartOfGuessSeqRange,
                                endExampleChar,
                                "{",
                                "}");
                            incorrectMessage += string.Format("Input must be in range {0}", possibleRangeString);
                            break;
                        case eValidStatuses.ContainsNonLetters:
                            incorrectMessage += "Input must only contain letters.";
                            break;
                        case eValidStatuses.ContainsNonUpperCase:
                            incorrectMessage += "Input must only contain UPPERCASE letters.";
                            break;
                        case eValidStatuses.IncorrectFormat:
                            incorrectMessage += string.Format("Input must be in format: {0}", formatOfInput);
                            break;
                        case eValidStatuses.TooLong:
                            incorrectMessage += string.Format("Too long. Input must contain {0} letters", game.MaxLengthOfComputerSequence);
                            break;
                        case eValidStatuses.TooShort:
                            incorrectMessage += string.Format("Too short. Input must contain {0} letters", game.MaxLengthOfComputerSequence);
                            break;
                        case eValidStatuses.LettersAppearMoreThanOnce:
                            incorrectMessage += "The input should not have duplicated chars";
                            break;
                        case eValidStatuses.AlreadyTried:
                            incorrectMessage += "The input have been inserted before";
                            break;
                    }

                    UI.DisplayMessage(incorrectMessage);
                }
            }

            Console.Clear();

            string message;
            switch(game.GameStatus)
            {
                case eGameStatuses.Win:
                    int numberOfTurnsPlayed = game.NumberOfTurnsPlayed();
                    string stepOrSteps = numberOfTurnsPlayed == 1 ? "step" : "steps";
                    UI.DisplayTable(game.NumberOfGuesses, game.UserGuessAndResult, game.MaxLengthOfComputerSequence);
                    message = string.Format("You guessed after {0} {1}!", numberOfTurnsPlayed, stepOrSteps);
                    UI.DisplayMessage(message);
                    break;
                case eGameStatuses.Lose:
                    UI.DisplayTable(game.NumberOfGuesses, game.UserGuessAndResult, game.MaxLengthOfComputerSequence, game.ComputerSequence);
                    UI.DisplayMessage("No more guesses allowed. You Lost.");
                    break;
                case eGameStatuses.Quit:
                    UI.DisplayMessage("Goodbye!");
                    break;
            }

            return game.GameStatus;
        }
    }
}