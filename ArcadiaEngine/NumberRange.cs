using System;

namespace SomewhatGeeky.Arcadia.Engine
{
    public class NumberRange
    {
        private System.Text.RegularExpressions.Regex regex;
        private int minimum;
        private int maximum;
        private bool isSet;

        public NumberRange(int minimum, int maximum)
        {
            this.minimum = minimum;
            this.maximum = maximum;
        }
        public NumberRange(int value)
            : this(value, value)
        {
        }

        public NumberRange()
            : this(-1)
        {
            isSet = false;
        }
        public NumberRange(string range)
            : this()
        {
            if (String.IsNullOrEmpty(range))
                return;

            if (regex == null)
                regex = new System.Text.RegularExpressions.Regex("[0-9][0-9]*");
            int matchNumber = 1;
            foreach (System.Text.RegularExpressions.Match match in regex.Matches(range))
            {
                switch (matchNumber)
                {
                    case 1:
                        minimum = maximum = System.Convert.ToInt32(match.Value);
                        break;
                    case 2:
                        maximum = System.Convert.ToInt32(match.Value);
                        break;
                    default:
                        throw new Exception("Invalid number range \"" + range + "\".");
                }
                matchNumber++;
            }

            if(matchNumber == 1)
                throw new Exception("Invalid number range \"" + range + "\".");


            isSet = true;
        }

        public int Minimum
        {
            get
            {
                return minimum;
            }
            set
            {
                SetBounds(maximum, value);
            }
        }

        public int Maximum
        {
            get
            {
                return maximum;
            }
            set
            {
                SetBounds(minimum, value);
            }
        }

        public void SetBounds(int val1, int val2)
        {
            if (val1 > val2)
            {
                maximum = val1;
                minimum = val2;
            }
            else
            {
                maximum = val2;
                minimum = val1;
            }
            isSet = true;
        }

        public bool IsSet
        {
            get
            {
                return isSet;
            }
            set
            {
                isSet = value;
                if (!value)
                {
                    minimum = maximum = -1;
                }
            }
        }

        public override string ToString()
        {
            if (!isSet)
                return "";
            if (minimum == maximum)
                return minimum.ToString();
            return minimum + "-" + maximum;
        }

        public override bool Equals(object obj)
        {
            NumberRange that = (NumberRange)obj;
            return this.minimum == that.minimum
                && this.maximum == that.maximum
                && this.isSet == that.isSet;
        }

        public override int GetHashCode()
        {
            return minimum.GetHashCode() / 3 + maximum.GetHashCode() / 3 + isSet.GetHashCode() / 3;
        }

    }
}
