using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfCalc.Models
{
    public class CalculatorModel
    {
        private string result;      //поле результата

        public string FirstOperand { get; set; }    //автосвойство 1 числа

        public string SecondOperand { get; set; }     //автосвойство 2 числа

        public string Operation { get; set; }       //автосвойство операции

        public string Result { get => result; } //результат

        public CalculatorModel()
        {
            FirstOperand = string.Empty;
            SecondOperand = string.Empty;
            Operation = string.Empty;
            result = string.Empty;
        }
        //публичный метод вычислений
        public void CalculateResult()       
        {
            ValidateOperation();
            try
            {
                switch (Operation)
                {
                    case ("+"):
                        result = (Convert.ToDouble(FirstOperand) + Convert.ToDouble(SecondOperand)).ToString();
                        break;

                    case ("-"):
                        result = (Convert.ToDouble(FirstOperand) - Convert.ToDouble(SecondOperand)).ToString();
                        break;

                    case ("*"):
                        result = (Convert.ToDouble(FirstOperand) * Convert.ToDouble(SecondOperand)).ToString();
                        break;

                    case ("/"):
                        result = (Convert.ToDouble(FirstOperand) / Convert.ToDouble(SecondOperand)).ToString();
                        break;

                    case ("%"):
                        result = (Convert.ToDouble(FirstOperand) / Convert.ToDouble(SecondOperand)).ToString();
                        break;

                    case ("1/x"):
                        if (Convert.ToDouble(FirstOperand) != 0)
                            result = (1 / (Convert.ToDouble(FirstOperand))).ToString();
                        else
                            throw new Exception();
                        break;

                    case ("sqr"):
                        result = Math.Pow(Convert.ToDouble(FirstOperand), 2).ToString();
                        break;

                    case ("sqrt"):
                        if (Convert.ToDouble(FirstOperand) >= 0)
                            result = Math.Sqrt(Convert.ToDouble(FirstOperand)).ToString();
                        else
                            throw new Exception();
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        //метод проверяет корректность аргументов
        private void ValidateOperand(string operand)        
        {
            try
            {
                Convert.ToDouble(operand);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //метод проверяет корректность выполняемых действий
        private void ValidateOperation()         
        {
            switch (Operation)
            {
                case "%":
                case "/":
                case "*":
                case "-":
                case "+":
                    ValidateOperand(FirstOperand);
                    ValidateOperand(SecondOperand);
                    break;
                case "1/x":
                case "sqr":
                case "sqrt":

                    ValidateOperand(FirstOperand);
                    break;
                default:
                    throw new Exception();
            }
        }
    }
}