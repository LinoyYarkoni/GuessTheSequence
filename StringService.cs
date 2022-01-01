using System.Text;

namespace A22_Ex02
{
    public static class StringService
    {
        public static string GenerateSeparatedStringRepeat(
            string i_Content,
            string i_Separator,
            int i_RepeatFactor,
            bool i_IsEndingSeparator = false)
        {
            StringBuilder result = new StringBuilder();
            int numberOfRepeats = (i_RepeatFactor * 2) - 1;

            for(int i = 0; i < numberOfRepeats; i++)
            {
                string stringToAppend = i % 2 == 0 ? i_Content : i_Separator;
                result.Append(stringToAppend);
            }

            if(i_IsEndingSeparator)
            {
                result.Append(i_Separator);
            }

            return result.ToString();
        }

        public static string AddSpaces(string i_Content, int i_Length)
        {
            StringBuilder result = new StringBuilder();
            int spacesLength = i_Length - i_Content.Length;
            string spaces = new string(' ', spacesLength);
            result.Append(i_Content + spaces);
            return result.ToString();
        }

        public static string GenerateResultStringFormatFromInt(int[] i_Result)
        {
            StringBuilder result = new StringBuilder();
            bool isV = i_Result[0] != 0;
            bool isX = i_Result[1] != 0;
            result.Append(StringService.GenerateSeparatedStringRepeat("V", " ", i_Result[0], isV));
            result.Append(StringService.GenerateSeparatedStringRepeat("X", " ", i_Result[1], isX));
            result.Append(StringService.GenerateSeparatedStringRepeat(" ", " ", i_Result[2], true));
            return result.ToString();
        }

        public static int GetNumberOfCorrectLetterAndPlace(string i_StringToCheck, string i_SourceString)
        {
            int stringLength = i_StringToCheck.Length;
            int numberOfMatches = 0;
            for(int i = 0; i < stringLength; i++)
            {
                if(i_StringToCheck[i] == i_SourceString[i])
                {
                    numberOfMatches++;
                }
            }

            return numberOfMatches;
        }

        public static int GetNumberOfMatchingCharsInWrongPlaces(string i_StringToCheck, string i_SourceString)
        {
            int stringLength = i_StringToCheck.Length;
            int numberOfMatches = 0;
            for(int i = 0; i < stringLength; i++)
            {
                for(int j = 0; j < stringLength; j++)
                {
                    if(i != j && i_StringToCheck[i] == i_SourceString[j])
                    {
                        numberOfMatches++;
                    }
                }
            }

            return numberOfMatches;
        }

        public static string GenerateRangeString(char i_StartIndex, char i_EndIndex, string i_Beginning, string i_Ending)
        {
            string result = i_Beginning;
            int sequenceLength = (2 * (i_EndIndex - i_StartIndex)) - 1;
            for(int i = 0; i < sequenceLength; i++)
            {
                if(i % 2 == 0)
                {
                    char currentChar = (char)((int)(i_StartIndex)+i / 2);
                    result += currentChar;
                }
                else
                {
                    result += " ";
                }
            }

            result += i_Ending;

            return result;
        }

        public static string GenerateSeparatedLetters(string i_StringToSeparate, string i_Separator)
        {
            char[] cArray = i_StringToSeparate.ToCharArray();
            string result = string.Join(i_Separator, cArray);
            return result;
        }

        public static bool IsDuplicateChar(string i_StringToCheck)
        {
            bool isDuplicated = false;
            int stringLength = i_StringToCheck.Length;
            for(int i = 0; i < stringLength - 1; i++)
            {
                for(int j = i + 1; j < stringLength; j++)
                {
                    if(i_StringToCheck[i] == i_StringToCheck[j])
                    {
                        isDuplicated = true;
                    }
                }
            }

            return isDuplicated;
        }
    }
}