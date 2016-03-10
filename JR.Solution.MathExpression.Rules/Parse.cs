using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace JR.Solution.MathExpression.Rules
{
    // contains operators definitions
    public enum FuntionConstant
    {
        MAX, MIN, ROUND, ROUNDUP, COS, SIN, TAN, ADDITEM, END
    }
    public enum OperatorConstant
    {
        Greater, Less, GE, LE, EQ, AND, OR, MUL, ADD, SUB, DIV
    }
    public class OperatorsCollection : List<char>
    {

    };

    /// <summary>
    /// Parse the input to postfix style to compute value easily    
    /// </summary>
    public class Parse
    {
        protected string input = "";

        protected Postfix result = new Postfix(); // result is stored as postfix
        // Max(a+b,c) => a b + c Max

        protected OperatorsCollection operators = new OperatorsCollection(); // operators
        protected List<string> logicOps = new List<string>(); // >, <, >=, <=, AND, OR
        protected List<string> functions = new List<string>(); // MAX, MIN, ROUND, ...
        protected List<int> precedents = new List<int>(); // (+, -) < (*, /)

        protected Stack<string> stack = new Stack<string>(); // the temporary to store functions and ops

        public OperatorsCollection Operators
        {
            get { return this.operators; }
        }
        public List<string> Functions
        {
            get { return functions; }
        }
        public List<string> LogicOperators
        {
            get { return logicOps; }
        }
        public Postfix Result
        {
            get { return this.result; }
        }

        public string Input
        {
            get { return input; }
            set { this.input = value; }
        }
        protected void Init(string input)
        {
            this.input = input;
            char[] o = { '+', '-', '*', '/' };
            int[] p = { 1, 1, 2, 2 };
            string[] lo = { ">", "=", "<", "<=", ">=", "AND", "OR" };
            logicOps.AddRange(lo);
            string[] f = { "MAX", "MIN", "ROUNDUP", "ADDITEM", "GOTO", "ROUND", "SIN", "COS", "TAN", "END" };
            functions.AddRange(f);
            operators.AddRange(o);
            precedents.AddRange(p);
        }
        public Parse(string input)
        {
            Init(input);
        }

        public Parse()
        {
            Init("");
        }
        // Check the valid character
        protected bool Check(char c)
        {
            if (c == 'Ø' || c=='\"')
                return true;
            if (c >= 'A' && c <= 'Z')
                return true;
            if (c == '#')
                return true;
            if (c >= 'a' && c <= 'z')
                return true;
            if (c >= '0' && c <= '9')
                return true;
            return false;
        }

        public void ParseIt(string input)
        {
            this.input = input;
            Run();
        }

        // this function will parse "A=200 B=900 C=400" to (#A,200), (#B,900), (#C,400)
        public PropertyValueCollection ParseValue(string formatValue)
        {
            formatValue = formatValue.TrimStart(" ".ToCharArray());
            formatValue = formatValue.Trim();
            PropertyValueCollection rs = new PropertyValueCollection();
            string[] propertiesValueString = formatValue.Split(' ');
            for (int i = 0; i < propertiesValueString.Length; i++)
            {
                string[] entry = propertiesValueString[i].Split('=');
                if (entry.Length < 2 || entry[1] == null || entry[1] == "")
                    return rs;
                PropertyValue pv = new PropertyValue(entry[0].ToUpper(), entry[1]);
                rs.Add(pv);
            }
            return rs;
        }

        // do the parsing
        protected void Run()
        {
            result.Clear(); // clear the result            
            string item = ""; // save the item such as KA, KA-, '-', 1,2... #A, #B
            char o = ' '; // character indices operator + - * /
            int index = -1; // to store the index in collection
            bool isOpen = false; // true if there is ' ... '
            bool isAdd = false; // a flag to show in 3+2*5 + < *
            int i = 0;
            while (i < input.Length) // loop through the input
            {
                char c = input[i];
                if (c == '\'') // if there is a '
                {
                    item += c.ToString();
                    if (isOpen)// if a ' already existed
                    {
                        result.Add(item); // add to the result
                        item = "";
                        isOpen = false; // close the '
                    }
                    else
                    {
                        isOpen = true;
                    }
                    i++; // do nothing else
                    continue;
                }
                else
                    if ((c == '>' || c == '<' || c == '=')) // if a logic is read
                    {
                        if (item != "") // add to the result if there is an item. Eg #A<2: item=#A
                        {
                            result.Add(item);
                            item = "";
                        }

                        if (i + 1 < input.Length && input[i + 1] == '=') //check for <=, >=
                        {
                            item = c.ToString() + "=";
                            i += 2;
                        }
                        else
                        {
                            item = c.ToString();
                            i++;
                        }
                    }
                    else
                        if (Check(c)) // is c a valid character
                        {
                            item += c.ToString(); // save to item
                            i++;
                            continue;
                        }
                index = operators.IndexOf(c); // find the operators : +, -, *, /
                int fIndex = -1; // another index 
                if (index >= 0) // operator found
                {
                    o = operators[index]; // get it
                    if (isOpen) // if there is a tag '
                    {
                        item += o; // save the operator to item. Eg: 'T-1'
                        i++; // do nothing
                        continue;
                    }
                    if (stack.Count > 0) // stack is empty?
                    {
                        string oldFunction = stack.Peek();
                        fIndex = operators.IndexOf(oldFunction[0]); // find the previous operators
                        if (fIndex >= 0)
                        {
                            if (precedents[fIndex] <= precedents[index]) // if prev precedent>current precedent
                            {
                                stack.Push(o.ToString()); // add to stack
                            }
                            else // otherwise swap them in the stack
                            {
                                oldFunction = stack.Pop();
                                stack.Push(o.ToString());
                                stack.Push(oldFunction);
                                isAdd = true; // turn on the flag
                            }
                        }
                        else
                            stack.Push(o.ToString()); // if there no prev operator, just push to stack
                    }
                    else
                        stack.Push(o.ToString());// if the stack is empty, push the operator to it
                }
                // if the item valid

                index = logicOps.IndexOf(item); // find logical operator
                if (index >= 0)
                {
                    // finish the stack until the closest (                    
                    while (stack.Count > 0 && (stack.Peek())[0] != '(')
                        result.Add(stack.Pop());
                    if (stack.Count > 0 && stack.Peek()[0] == '(')
                        stack.Pop(); // remove the ( in stack    
                    if (result.Count>0 &&
                        (result[result.Count - 1] == "AND" || result[result.Count - 1] == "OR")
                        && item != result[result.Count - 1])
                    {
                        stack.Push(result[result.Count - 1]);
                        stack.Push(item);                        
                        result.RemoveAt(result.Count - 1);
                    } 
                    else
                    stack.Push(item);
                    item = "";
                    // do nothing
                    continue;
                }

                if (item != "")
                {
                    fIndex = functions.IndexOf(item); // check if it is a function
                    if (fIndex == -1) // if not
                    {
                        result.Add(item); // add to result    
                        if (isAdd) // get the operator from the stack. This solve 2+3*5 problem
                        {
                            isAdd = false;
                            result.Add(stack.Pop());
                        }
                    }
                    else // yeah, it is a function
                    {
                        stack.Push(functions[fIndex]);                         // add to stack
                    }
                    item = ""; // reset the item
                }
                if (isAdd) // get the operator from the stack. This solve 2+3*5 problem
                {
                    isAdd = false;
                    result.Add(stack.Pop());
                }
                if (c == '(') // if it a (, pust to stack as a flag
                {
                    stack.Push(c.ToString());
                }

                if (c == ',' && stack.Count > 0 && (stack.Peek())[0] != '(') // for function has comma in it EG: MAX(a,b)
                {
                    // finish the stack until the closest (
                    // (stack.Peek())[0] != '(' must be true for this case: MAX(MIN(B,C), A)
                    while (stack.Count > 0 && functions.IndexOf(stack.Peek()) == -1 && (stack.Peek())[0] != '(')
                    {
                        result.Add(stack.Pop());
                    }

                    //if (stack.Count > 0 && stack.Peek()[0] == '(')
                    //    stack.Pop(); // remove the ( in stack                  

                }
                else
                    if (c == ')') // if a )
                    {
                        // finish the stack until the closest (                    
                        while (stack.Count > 0 && (stack.Peek())[0] != '(')
                        {
                            result.Add(stack.Pop());
                        }
                        if (stack.Count > 0 && stack.Peek()[0] == '(')
                            stack.Pop(); // remove the ( in stack
                        if (stack.Count > 0 && functions.IndexOf(stack.Peek()) >= 0)
                            result.Add(stack.Pop()); // find the function with its (), so put the function to result
                    }
                i++;
            }
            if (item != "")
                result.Add(item);
            // finish all left operators
            while (stack.Count > 0)
            {
                result.Add(stack.Pop());
            }
            if (item == "END")
                result.Add("END");
            // remove ( from the result
            result.Remove('('.ToString());
        }

        public Postfix ReplaceValue(Postfix result, PropertyValueCollection pv)
        {
            Postfix a = new Postfix();
            a = result;
            for (int i = 0; i < pv.Count; i++)
            {
                int index = -1;
                while ((index = result.IndexOf("#" + pv[i].Name)) != -1)
                {
                    a[index] = pv[i].Value;
                }
            }
            return a;
        }
    }
}
