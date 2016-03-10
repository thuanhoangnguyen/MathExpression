using JR.Solution.MathExpression.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace JR.Solution.MathExpression
{
    public partial class Form1 : Form
    {
        Parse parser;
        List<string> results = new List<string>();
        public Form1()
        {
            InitializeComponent();
            string[] maths = {
                "#A*2+#B-10*(#C+#D)",
                "MAX(#A,#B)*MIN(#C,#D)+2*#E",
                "MAX(3*#A+sin(#B),MIN(200,#C))-cos(#D)",
                "#A+#B+2*#C <= #D",
                "(#A-#B+3*#C > #D) AND (#E>=100)",
                "(#A=\"SA\") AND (#B=\"\")",
                "MAX(MAX(#A,#A2),MAX(#B,#B2)) < 400",
            };
            string[] replaces = {
                "A=2 B=5 C=3 D=4",
                "A=1 B=3 C=2 D=5 E=10",
                "A=2 B=90 C=100 D=60",
                "A=2 B=3 C=4 D=10",
                "A=1 B=2 C=4 D=5 E=101",
                "A=\"SA\" B=\"\"",
                "A=300 B=200 A2=500 B2=300"
            };
            txtExpression.Lines = maths;
            txtReplaceValue.Lines = replaces;
            parser = new Parse();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            results.Clear();
            PropertyValueCollection pv = null;
            Postfix rs = null;
            for (int i = 0; i < txtExpression.Lines.Length; i++)
            {                
                string str = txtExpression.Lines[i].ToUpper();
                string formatValue = txtReplaceValue.Lines[i].ToUpper();
                pv = parser.ParseValue(formatValue);
                parser.ParseIt(str);
                rs = parser.ReplaceValue(parser.Result, pv);
                results.Add(Calc.Compute(rs).ToString());
            }
            txtResult.Lines = results.ToArray();
        }
    }
}