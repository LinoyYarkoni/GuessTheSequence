using System;
using System.Collections.Generic;
using System.Linq;

namespace A22_Ex02
{
    public enum eGameStatuses
    {
        Playing,
        Win,
        Lose,
        Quit,
    }

    public enum eValidStatuses
    {
        Valid,
        TooShort,
        TooLong,
        ContainsNonLetters,
        ContainsNonUpperCase,
        NotInRange,
        IncorrectFormat,
        LettersAppearMoreThanOnce,
        AlreadyTried,
    }

    public class GameBoard
    {
        private readonly List<Tuple<string, int[]>> r_UserGuessAndResult = new List<Tuple<string, int[]>>();
        private readonly int r_MaxLengthOfComputerSequence;
        private readonly char r_StartOfGuessSeqRange;
        private readonly char r_EndOfGuessSeqRange;
        private int m_NumberOfGuesses;
        private string m_ComputerSequence;
        private eGameStatuses m_Status;

        public GameBoard(
            int i_MaxLengthOfComputerSequence,
            char i_StartOfGuessSeqRange,
            char i_EndOfGuessSeqRange,
            int i_NumberOfGuesses)
        {
            this.r_MaxLengthOfComputerSequence = i_MaxLengthOfComputerSequence;
            this.r_StartOfGuessSeqRange = i_StartOfGuessSeqRange;
            this.r_EndOfGuessSeqRange = i_EndOfGuessSeqRange;
            this.m_NumberOfGuesses = i_NumberOfGuesses;
            this.m_Status = eGameStatuses.Playing;
            this.generateRandomCharSequence();
        }

        public List<Tuple<string, int[]>> UserGuessAndResult
        {
            get
            {
                return this.r_UserGuessAndResult;
            }
        }

        public int NumberOfGuesses
        {
            get
            {
                return this.m_NumberOfGuesses;
            }

            set
            {
                this.m_NumberOfGuesses = value;
            }
        }

        public int MaxLengthOfComputerSequence
        {
            get
            {
                return this.r_MaxLengthOfComputerSequence;
            }
        }

        public string ComputerSequence
        {
            get
            {
                return this.m_ComputerSequence;
            }
        }

        public char StartOfGuessSeqRange
        {
            get
            {
                return this.r_StartOfGuessSeqRange;
            }
        }

        public char EndOfGuessSeqRange
        {
            get
            {
                return this.r_EndOfGuessSeqRange;
            }
        }

        public int NumberOfTurnsPlayed()
        {
            int numberOfTurnsPlayed = 0;
            if(this.r_UserGuessAndResult != null)
            {
                numberOfTurnsPlayed = this.r_UserGuessAndResult.Count();
            }

            return numberOfTurnsPlayed;
        }

        public eGameStatuses GameStatus
        {
            get
            {
                return this.m_Status;
            }
        }

        public eValidStatuses AddGuess(string i_UserGuessInput)
        {
            eValidStatuses isValidFormat = this.isCurrentGuessValidFormat(i_UserGuessInput);
            if(isValidFormat == eValidStatuses.Valid)
            {
                string formattedGuess = i_UserGuessInput.Replace(" ", string.Empty);
                int[] resultsOfCurrentGuess = this.calculateGuessResult(formattedGuess);
                this.addToUserGuessList(i_UserGuessInput, resultsOfCurrentGuess);
                this.updateGameStatus();
            }

            return isValidFormat;
        }

        public void QuitGame()
        {
            this.m_Status = eGameStatuses.Quit;
        }

        private void updateGameStatus()
        {
            if(this.r_UserGuessAndResult.Last().Item2[0] == MaxLengthOfComputerSequence)
            {
                this.m_Status = eGameStatuses.Win;
            }
            else if(this.NumberOfTurnsPlayed() >= this.NumberOfGuesses)
            {
                this.m_Status = eGameStatuses.Lose;
            }
        }

        private eValidStatuses isCurrentGuessValidFormat(string i_UserGuessInput)
        {
            eValidStatuses isValidGuess = eValidStatuses.Valid;
            string guess = i_UserGuessInput.Replace(" ", string.Empty);
            int guessLength = guess.Length;
            bool isDuplicatedChar = StringService.IsDuplicateChar(guess);
            bool isDuplicatedInput = checkDuplicatedInput(i_UserGuessInput);

            if(!guess.All(char.IsLetter))
            {
                isValidGuess = eValidStatuses.ContainsNonLetters;
            }
            else if(!guess.All(char.IsUpper))
            {
                isValidGuess = eValidStatuses.ContainsNonUpperCase;
            }
            else if(guessLength < this.r_MaxLengthOfComputerSequence)
            {
                isValidGuess = eValidStatuses.TooShort;
            }
            else if(i_UserGuessInput.Length > (this.r_MaxLengthOfComputerSequence * 2) - 1)
            {
                isValidGuess = eValidStatuses.TooLong;
            }
            else if(isDuplicatedChar)
            {
                isValidGuess = eValidStatuses.LettersAppearMoreThanOnce;
            }
            else if(isDuplicatedInput)
            {
                isValidGuess = eValidStatuses.AlreadyTried;
            }
            else
            {
                int currentSequenceLength = i_UserGuessInput.Length;
                for (int i = 0; i < currentSequenceLength; i++)
                {
                    if(i % 2 == 0)
                    {
                        if(!(i_UserGuessInput[i] >= this.r_StartOfGuessSeqRange
                              && i_UserGuessInput[i] <= this.r_EndOfGuessSeqRange))
                        {
                            isValidGuess = eValidStatuses.NotInRange;
                            break;
                        }
                    }
                    else
                    {
                        if(i_UserGuessInput[i] != ' ')
                        {
                            isValidGuess = eValidStatuses.IncorrectFormat;
                            break;
                        }
                    }
                }
            }

            return isValidGuess;
        }

        private bool checkDuplicatedInput(string i_StringToCheck)
        {
            bool isDuplicated = false;
            if(r_UserGuessAndResult != null)
            {
                for(int i = 0; i < r_UserGuessAndResult.Count; i++)
                {
                    if(i_StringToCheck == r_UserGuessAndResult[i].Item1)
                    {
                        isDuplicated = true;
                    }
                }
            }

            return isDuplicated;
        }

        private int[] calculateGuessResult(string i_GuessInputToCheck)
        {
            int numberOfMatchingPlaces = StringService.GetNumberOfCorrectLetterAndPlace(i_GuessInputToCheck, this.m_ComputerSequence);
            int numberOfMatchingCharsInWrongPlaces =
                StringService.GetNumberOfMatchingCharsInWrongPlaces(i_GuessInputToCheck, this.m_ComputerSequence);
            int numberOfIncorrectChars = this.MaxLengthOfComputerSequence - numberOfMatchingPlaces
                                                                          - numberOfMatchingCharsInWrongPlaces;

            int[] result = new int[3] { numberOfMatchingPlaces, numberOfMatchingCharsInWrongPlaces, numberOfIncorrectChars };

            return result;
        }

        private void generateRandomCharSequence()
        {
            this.m_ComputerSequence = string.Empty;
            Random rnd = new Random();
            while(this.m_ComputerSequence.Length < this.r_MaxLengthOfComputerSequence)
            {
                char randomChar = (char)rnd.Next(this.r_StartOfGuessSeqRange, this.r_EndOfGuessSeqRange);
                if(checkComputerGuess(this.m_ComputerSequence, randomChar))
                {
                    this.m_ComputerSequence += randomChar;
                }
            }
        }

        private bool checkComputerGuess(string i_ComputerSequence, char i_CurrentRandomChar)
        {
            bool checkGuessResult = true;
            int currentSequenceLength = i_ComputerSequence.Length;
            for(int i = 0; i < currentSequenceLength; i++)
            {
                if(i_CurrentRandomChar == i_ComputerSequence[i])
                {
                    checkGuessResult = false;
                }
            }

            return checkGuessResult;
        }

        private void addToUserGuessList(string i_Guess, int[] i_Result)
        {
            Tuple<string, int[]> itemToAdd = new Tuple<string, int[]>(i_Guess, i_Result);
            r_UserGuessAndResult.Add(itemToAdd);
        }
    }
}