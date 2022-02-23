using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using WpfCalc.Models;

namespace WpfCalc.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        //реализация интерфейса INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        //закрытое поле - хранит символ послед. нажатой клавиши операции
        private string lastOperation;
        //закрытое поле хранит логическое значение, при вводе следующего символа дисплей должен быть предварительно очищен
        private bool newDisplayRequired = false;
        //закрытое поле - хранит выражение вычислений
        private string expression;
        //закрытое поле - хранит значение отображаемое на калькуляторе
        private string display = "0";
        //экземпляр калькулятора
        private CalculatorModel calculator = new CalculatorModel();
        //свойство через которое осуществляется взаимодействие с полем FirstOperand модели калькулятора
        public string FirstOperand      
        {
            get => calculator.FirstOperand;
            set => calculator.FirstOperand = value;
        }
        //свойство через которое осуществляется взаимодействие с полем SecondOperand модели калькулятора
        public string SecondOperand      
        {
            get => calculator.SecondOperand;
            set => calculator.SecondOperand = value;
        }
        //свойство через которое осуществляется взаимодействие с полем Operation модели калькулятора
        public string Operation              
        {
            get => calculator.Operation;
            set => calculator.Operation = value;
        }
        //свойство через которое осуществляется взаимодействие с полем lastOperation
        public string LastOperation      
        {
            get => lastOperation;
            set => lastOperation = value;
        }
        //свойство через которое осуществляется взаимодействие с полем Result
        public string Result       
        {
            get => calculator.Result;
        }
        //свойство через которое осуществляется взаимодействие с полем Display
        public string Display       
        {
            get
            {
                if (display.Length > 11)
                    return display.Substring(0, 11);
                else
                    return display;
            }
            set
            {
                display = value;
                OnPropertyChanged();
            }
        }
        //свойство через которое осуществляется взаимодействие с полем Еxpression
        public string Expression       
        {
            get => expression;
            set
            {
                expression = value;
                OnPropertyChanged();
            }
        }
        //команда для клавиши Clear
        public ICommand ClearButtonPressCommand { get; }        

        public void OnClearButtonPressCommandExecute(object p)
        {
            Display = "0";
            FirstOperand = string.Empty;
            SecondOperand = string.Empty;
            Operation = string.Empty;
            LastOperation = string.Empty;
            Expression = string.Empty;
            newDisplayRequired = false;
        }

        public bool CanClearButtonPressCommandExecuted(object p)
        {
            return true;
        }
        //команда для клавиши Delete
        public ICommand DeleteButtonPressCommand { get; }        

        public void OnDeleteButtonPressCommandExecute(object p)
        {
            if (Display.Length > 1)
                Display = Display.Substring(0, Display.Length - 1);
            else
                Display = "0";
        }

        public bool CanDeleteButtonPressCommandExecuted(object p)
        {
            if (Display == "ERROR")
                return false;
            else
                return true;
        }
        //команда нажатия кнопок
        public ICommand DigitButtonPressCommand { get; }        

        public void OnDigitButtonPressCommandExecute(object p)
        {
            string button = (string)p;
            switch (button)
            {
                case "+/-":
                    if (Display.Contains("-"))
                        Display = Display.Remove(Display.IndexOf("-"), 1);
                    else
                        Display = "-" + Display;
                    break;
                case ",":
                    if (newDisplayRequired)
                        Display = "0,";
                    else if (!Display.Contains(","))
                        Display += ",";
                    break;
                default:
                    if (Display == "0" || newDisplayRequired)
                        Display = button;
                    else
                        Display += button;
                    break;
            }
            newDisplayRequired = false;
        }

        public bool CanDigitButtonPressCommandExecuted(object p)
        {
            if (Display == "ERROR")
                return false;
            else
                return true;
        }
        //команда нажатия кнопок операций, выполняемых с одним аргументом операции
        public ICommand SingleOperationButtonPressCommand { get; }      

        public void OnSingleOperationButtonPressCommandExecute(object p)
        {
            string operation = (string)p;
            try
            {
                FirstOperand = Display;
                Operation = operation;
                calculator.CalculateResult();
                Expression = Operation + "(" + Math.Round(Convert.ToDouble(FirstOperand), 5) + ") = ";
                LastOperation = "=";
                Display = Result;
                FirstOperand = Display;
                newDisplayRequired = true;
            }
            catch (Exception)
            {
                Display = "ERROR";
            }
        }

        public bool CanSingleOperationButtonPressCommandExecuted(object p)
        {
            if (Display == "ERROR")
                return false;
            else
                return true;
        }
        //команда нажатия клавиш операций, выполняемых с двумя аргументами операции
        public ICommand DoubleOperationButtonPressCommand { get; }      

        public void OnDoubleOperationButtonPressCommandExecute(object p)
        {
            string operation = (string)p;
            try
            {
                if (FirstOperand == string.Empty || LastOperation == "=")
                {
                    FirstOperand = Display;
                    LastOperation = operation;
                }
                else
                {
                    SecondOperand = Display;
                    Operation = LastOperation;
                    calculator.CalculateResult();
                    Expression = Math.Round(Convert.ToDouble(FirstOperand), 5) + " "
                        + Operation + " " + Math.Round(Convert.ToDouble(SecondOperand), 5) + " = ";
                    LastOperation = operation;
                    Display = Result;
                    FirstOperand = Display;
                }
                newDisplayRequired = true;
            }
            catch (Exception)
            {
                Display = "ERROR";
            }
        }

        public bool CanDoubleOperationButtonPressCommand(object p)
        {
            if (Display == "ERROR")
                return false;
            else
                return true;
        }

        public MainWindowViewModel()
        {
            //передача управления командами в RelayCommand
            DigitButtonPressCommand = new RelayCommand(OnDigitButtonPressCommandExecute, CanDigitButtonPressCommandExecuted);
            SingleOperationButtonPressCommand = new RelayCommand(OnSingleOperationButtonPressCommandExecute, CanSingleOperationButtonPressCommandExecuted);
            DoubleOperationButtonPressCommand = new RelayCommand(OnDoubleOperationButtonPressCommandExecute, CanDoubleOperationButtonPressCommand);
            ClearButtonPressCommand = new RelayCommand(OnClearButtonPressCommandExecute, CanClearButtonPressCommandExecuted);
            DeleteButtonPressCommand = new RelayCommand(OnDeleteButtonPressCommandExecute, CanDeleteButtonPressCommandExecuted);
        }
    }
}