using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace A22_Ex02
{
    public static class UI
    {
        public static string StartAgainResult()
        {
            string[] possibleOptions = new string[4] { "Y", "y", "N", "n" };
            string userInputToCheck;
            bool isValid = false;
            do
            {
                Console.WriteLine("Would you like to start a new game? <Y/N>");
                userInputToCheck = Console.ReadLine();
                for(int i = 0; i < 4; i++)
                {
                    if(userInputToCheck == possibleOptions[i])
                    {
                        isValid = true;
                        break;
                    }
                }
            }
            while(!isValid);

            string userInput = userInputToCheck;

            return userInput;
        }

        public static int GetUserNumberInRange(int i_MinNumber, int i_MaxNumber, string i_RequestedInput)
        {
            bool isValidResult = true;
            bool isInRangeResult = true;
            string userInput;

            do
            {
                userInput = UI.GetUserInput(i_RequestedInput);

                isValidResult = UI.isInputContainsOnlyNumbers(userInput);
                if(!isValidResult)
                {
                    Console.WriteLine("The input must have only digits and no other characters");
                }
                else if(int.TryParse(userInput, out int userInputInt))
                {
                    isInRangeResult = NumberService.IsNumberInRange(userInputInt, i_MinNumber, i_MaxNumber);
                    if(!isInRangeResult)
                    {
                        Console.WriteLine(
                            "The number is not valid. Please enter a number between {0} to {1}",
                            i_MinNumber,
                            i_MaxNumber);
                    }
                }
            }
            while(!(isValidResult && isInRangeResult));

            return int.Parse(userInput);
        }

        public static void DisplayTable(
            int i_NumberOfGuesses,
            List<Tuple<string, int[]>> i_UserGuessList,
            int i_ComputerGuessLength,
            string i_Placeholder = null)
        {
            StringBuilder tableBody = new StringBuilder();
            string pinsString = StringService.AddSpaces("Pins:", (i_ComputerGuessLength * 2) - 1);
            string resultString = StringService.AddSpaces("Result:", (i_ComputerGuessLength * 2) - 1);
            string lineBreak = StringService.GenerateSeparatedStringRepeat("=", "=", i_ComputerGuessLength);
            string placeholder;
            if(i_Placeholder == null)
            {
                placeholder = StringService.GenerateSeparatedStringRepeat("#", " ", i_ComputerGuessLength);
            }
            else
            {
                placeholder = StringService.GenerateSeparatedLetters(i_Placeholder, " ");
            }

            string lineSpaces = StringService.GenerateSeparatedStringRepeat(" ", " ", i_ComputerGuessLength);
            string tableHead = string.Format(
@"Current board status:
|{0}  |{1}|
|={2}=|{2}|
| {3} |{4}|
|={2}=|{2}|
",
                pinsString,
                resultString,
                lineBreak,
                placeholder,
                lineSpaces);

            tableBody.Append(tableHead);
            for(int i = 0; i < i_NumberOfGuesses; i++)
            {
                string resultToPrint;
                string guessToPrint;
                if(i_UserGuessList != null && i < i_UserGuessList.Count)
                {
                    guessToPrint = i_UserGuessList[i].Item1;
                    resultToPrint = StringService.GenerateResultStringFormatFromInt(i_UserGuessList[i].Item2);
                    resultToPrint = resultToPrint.Substring(0, (i_ComputerGuessLength * 2) - 1);
                }
                else
                {
                    guessToPrint = lineSpaces;
                    resultToPrint = lineSpaces;
                }

                tableBody.AppendFormat("| {0} |{1}|{2}", guessToPrint, resultToPrint, Environment.NewLine);
                tableBody.AppendFormat("|={0}=|{0}|{1}", lineBreak, Environment.NewLine);
            }

            Console.WriteLine(tableBody);
        }

        public static string GetUserInput(string i_RequestedInput)
        {
            Console.WriteLine(i_RequestedInput);
            string userInput = Console.ReadLine();
            return userInput;
        }

        public static void DisplayMessage(string i_Message)
        {
            Console.WriteLine(i_Message);
        }

        private static bool isInputContainsOnlyNumbers(string i_UserInputNumberOfGuesses)
        {
            bool isValidInputResult = i_UserInputNumberOfGuesses.All(char.IsDigit);

            return isValidInputResult;
        }
    }
}