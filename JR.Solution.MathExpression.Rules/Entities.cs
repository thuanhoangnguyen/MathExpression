using System;
using System.Collections.Generic;
using System.Text;

namespace JR.Solution.MathExpression.Rules
{    
    public class PropertyValue
    {
        protected string name;
        protected string value;
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
        public PropertyValue(string name, string value)
        {
            this.value = value;
            this.name = name;
        }
    };
    public class PropertyValueCollection : List<PropertyValue>
    {

    };
    // the result after parse in postfix style
    public class Postfix : List<string>
    {

    };
}
