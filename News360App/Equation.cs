using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace News360App
{
    public class Equation
    {
        private string _equation;
        private Regex _variableRegex = new Regex(@"(?<op>[-+])*(?<num>\d+([.]\d+)*)*(?<var>\w+([\^]\d+)*)*");

        public Equation(string equation)
        {
            if (string.IsNullOrEmpty(equation))
            {
                throw new ArgumentException("Empty equation");
            }

            if (!equation.Contains('='))
            {
                throw new ArgumentException("Invalid equation");
            }

            _equation = equation.Replace(" ", "");

            if (Regex.IsMatch(_equation, "(^[=])|([=]$)"))
            {
                throw new ArgumentException("Invalid equation");
            }
        }

        public string Canonicalize()
        {
            if (Regex.IsMatch(_equation, "=0$"))
            {
                return _equation;
            }

            var equation = MoveAllVariablesLeft(_equation);
            equation = AddSameVariables(equation);
            return equation + "=0";
        }

        private string MoveAllVariablesLeft(string equation)
        {
            var parts = equation.Split('=');
            var leftSide = parts[0];
            var rightSide = parts[1];
            var rightSideVars = _variableRegex.Matches(rightSide);

            foreach (var variable in rightSideVars)
            {
                leftSide += "-" + variable;
            }
            leftSide = Regex.Replace(leftSide, "[-][+]", "-");
            leftSide = Regex.Replace(leftSide, "[-][-]", "+");

            return leftSide;
        }

        private string AddSameVariables(string equation)
        {
            var variables = _variableRegex.Matches(equation);
            var varDic = new Dictionary<string, decimal>();

            foreach (Match m in variables)
            {
                var plusMinusValue = m.Groups["op"].Value;
                var numValue = m.Groups["num"].Value;
                numValue = string.IsNullOrEmpty(numValue) ? "1" : numValue;
                var num = decimal.Parse(plusMinusValue + numValue);
                var variable = m.Groups["var"].Value;

                if (varDic.ContainsKey(variable))
                {
                    varDic[variable] += num;
                }
                else
                {
                    varDic.Add(variable, num);
                }
            }

            string newEquation = string.Empty;

            foreach (var v in varDic)
            {
                if (v.Value == 0)
                {
                    continue;
                }

                decimal constant = 0;
                if (decimal.TryParse(v.Key, out constant))
                {
                    newEquation += constant * v.Value;
                    continue;
                }

                if (v.Value == 1)
                {
                    newEquation += "+" + v.Key;
                    continue;
                }

                if (v.Value == -1)
                {
                    newEquation += "-" + v.Key;
                    continue;
                }

                if (v.Value < 0)
                {
                    newEquation += v.Value + v.Key;
                }
                else
                {
                    newEquation += "+" + v.Value + v.Key;
                }
            }

            //ugly way of removing first + sign
            newEquation = Regex.Replace(newEquation, "^[+]", "");

            return newEquation;
        }
    }
}
