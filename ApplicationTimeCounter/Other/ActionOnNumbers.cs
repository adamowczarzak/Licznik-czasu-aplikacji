using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTimeCounter
{
    static class ActionOnNumbers
    {
        static public int DivisionI(int number1, int number2)
        {
            if (number1 != 0)
                return (number1) / (number2);
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
                return (double)(number1) / (double)(number2);
            else return 0;
        }
    }
}
