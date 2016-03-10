using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace JR.Solution.MathExpression.Rules
{
    public static class Calc
    {
        public static object Compute(Postfix input)
        {
            Stack stack = new Stack();
            object v1 = null;
            object v2 = null;
            double a = 0.0;
            double b = 0.0;
            bool b1, b2;
            int i1, i2;
            for (int i = 0; i < input.Count; i++)
            {
                try
                {
                    switch (input[i])
                    {
                        case "MAX":
                            v1 = stack.Pop();
                            v2 = stack.Pop();
                            if (double.TryParse(v1.ToString(), out a) && double.TryParse(v2.ToString(), out b))
                                stack.Push(Functions.Max(a, b));
                            break;
                        case "MIN":
                            v1 = stack.Pop();
                            v2 = stack.Pop();
                            if (double.TryParse(v1.ToString(), out a) && double.TryParse(v2.ToString(), out b))
                                stack.Push(Functions.Min(a, b));
                            break;
                        case "ROUND":
                            v1 = stack.Pop();
                            v2 = stack.Pop();
                            if (double.TryParse(v2.ToString(), out a) && int.TryParse(v1.ToString(), out i2))
                                stack.Push(Functions.Round(a, i2));
                            break;
                        case "+":
                            v1 = stack.Pop();
                            v2 = stack.Pop();
                            if (double.TryParse(v1.ToString(), out a) && double.TryParse(v2.ToString(), out b))
                                stack.Push(a + b);
                            else
                                stack.Push(v2.ToString() + v1.ToString());
                            break;
                        case "-":
                            v1 = stack.Pop();
                            v2 = stack.Pop();
                            if (double.TryParse(v1.ToString(), out a) && double.TryParse(v2.ToString(), out b))
                            {                                
                                stack.Push(b - a);
                            }
                            break;
                        case "/":
                            v1 = stack.Pop();
                            v2 = stack.Pop();
                            if (double.TryParse(v1.ToString(), out a) && double.TryParse(v2.ToString(), out b))
                                stack.Push(b / a);
                            break;
                        case "*":
                            v1 = stack.Pop();
                            v2 = stack.Pop();
                            if (double.TryParse(v1.ToString(), out a) && double.TryParse(v2.ToString(), out b))
                                stack.Push(a * b);
                            break;
                        case "ROUNDUP":
                            v1 = stack.Pop();
                            v2 = stack.Pop();
                            if (double.TryParse(v2.ToString(), out a) && int.TryParse(v1.ToString(), out i2))
                                stack.Push(Functions.RoundUp(a, i2));
                            break;
                        case "SIN":
                            v1 = stack.Pop();
                            stack.Push(Functions.Sin(Convert.ToDouble(v1) / 180 * Math.PI));
                            break;
                        case "COS":
                            v1 = stack.Pop();
                            stack.Push(Functions.Cos(Convert.ToDouble(v1) / 180 * Math.PI));
                            break;
                        case "TAN":
                            v1 = stack.Pop();
                            stack.Push(Functions.Tan(Convert.ToDouble(v1) / 180 * Math.PI));
                            break;
                        case ">":
                            a = Convert.ToDouble(stack.Pop());
                            b = Convert.ToDouble(stack.Pop());
                            stack.Push(a < b);
                            break;
                        case "<":
                            a = Convert.ToDouble(stack.Pop());
                            b = Convert.ToDouble(stack.Pop());
                            stack.Push((bool)(a > b));
                            break;
                        case ">=":
                            a = Convert.ToDouble(stack.Pop());
                            b = Convert.ToDouble(stack.Pop());
                            stack.Push((bool)(a <= b));
                            break;
                        case "<=":
                            a = Convert.ToDouble(stack.Pop());
                            b = Convert.ToDouble(stack.Pop());
                            stack.Push((bool)(a >= b));
                            break;
                        case "=":
                            v1 = stack.Pop();
                            v2 = stack.Pop();
                            if (double.TryParse(v1.ToString(), out a) && double.TryParse(v2.ToString(), out b))
                                stack.Push(a==b);
                            else
                                stack.Push(v1.ToString().Replace('\"',' ').Trim()==v2.ToString().Replace('\"',' ').Trim());                            
                            break;
                        case "AND":
                            b1 = Convert.ToBoolean(stack.Pop());
                            b2 = Convert.ToBoolean(stack.Pop());
                            stack.Push((bool)(b1 && b2));
                            break;
                        case "OR":
                            b1 = Convert.ToBoolean(stack.Pop());
                            b2 = Convert.ToBoolean(stack.Pop());
                            stack.Push((bool)(b1 || b2));
                            break;
                        case "ADDITEM":
                            v1 = stack.Pop();
                            v2 = stack.Pop();
                            if (int.TryParse(v1.ToString(), out i2))
                                stack.Push(Functions.AddItem(v2.ToString(), i2));
                            break;
                        case "GOTO":
                            v1 = stack.Pop();
                            stack.Push(Functions.Goto(v1.ToString()));
                            break;
                        case "END":
                            stack.Push(Functions.End());
                            break;
                        default:
                            stack.Push(input[i]);
                            break;
                    }
                } catch (Exception ex)
                {
                    MessageBox.Show("Can not calculate the formular. Maybe parameter was not input " + ex.Message);
                }
                
            }
            return stack.Count == 0 ? "" : stack.Pop();            
        }
    }
}
