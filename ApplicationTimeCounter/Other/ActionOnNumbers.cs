using System;
namespace ApplicationTimeCounter.Other
{
    static class ActionOnNumbers
    {
        static public int DivisionI(int number1, int number2)
        {
            if (number1 != 0)
                return number1 / number2;
            else return 0;
        }

        static public double DivisionD(int number1, int number2)
        {
            if (number1 != 0)
                return (double)(number1) / (double)(number2);
            else return 0;
        }

        static public double DivisionD(double number1, double number2)
        {
            if (number1 != 0)
                return number1 / number2;
            else return 0;
        }

        static public int [] EqualizePercentages(int [] valueTable, int sumValue)
        {
            double[] restPercent = new double[4];
            int[] percentTable = new int[4];
            for (int i = 0; i < 4; i++)
            {
                restPercent[i] = (ActionOnNumbers.DivisionD(valueTable[i], sumValue) * 100) % 1.0;
                percentTable[i] = (int)(ActionOnNumbers.DivisionD(valueTable[i], sumValue) * 100);
            }
            int allPercent = percentTable[0] + percentTable[1] + percentTable[2] + percentTable[3];
            int numberOfRepetitions = Math.Abs(100 - allPercent);
            while (numberOfRepetitions != 0)
            {
                int maxIndex = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (restPercent[i] > restPercent[maxIndex])
                        maxIndex = i;
                }
                percentTable[maxIndex]++;
                numberOfRepetitions--;
                restPercent[maxIndex] = 0.0;
            }

            return percentTable;
        }
    }
}
