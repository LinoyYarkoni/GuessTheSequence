namespace A22_Ex02
{
    public static class NumberService
    {
        public static bool IsNumberInRange(int i_Number, int i_Min, int i_Max)
        {
            bool isNumberInRangeResult = i_Number >= i_Min && i_Number <= i_Max;

            return isNumberInRangeResult;
        }
    }
}