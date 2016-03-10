using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace JR.Solution.MathExpression.Rules
{            
    public static class Functions
    {
        public static double Max(double a, double b)
        {
            return a > b ? a : b;
        }
        public static double Min(double a, double b)
        {            
            return a < b ? a : b;
        }
        public static double RoundUp(double a, int v)
        {
            return v*((int)a / v +1);
        }
        public static double Round(double a, int v)
        {
            return Math.Round(a, v);
        }
        public static double Sin(double a)
        {
            return Math.Sin(a);
        }
        public static double Cos(double a)
        {
            return Math.Cos(a);
        }
        public static double Tan(double a)
        {
            return Math.Tan(a);
        }
        public static string Goto(string where)
        {
            int i = where.IndexOf('\'');
            while (i != -1)
            {
                where = where.Remove(i, 1);
                i = where.IndexOf('\'');
            }
            return "GOTO"+Delimiter+where;
        }
        public static string AddItem(string item, int number)
        {            
            int i=item.IndexOf('\'');
            while(i!=-1)
            {
                item = item.Remove(i, 1);
                i = item.IndexOf('\'');
            }
            return "ADDITEM"+Delimiter+item+Delimiter+number.ToString();
        }
        public static string End()
        {
            return "END";
        }
        public const string Delimiter = "$";
    };
}
