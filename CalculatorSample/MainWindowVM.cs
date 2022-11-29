using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorSample
{
    class MainWindowVM : BindableBase
    {
        public enum CalcModeEnum
        {
            None,
            Add,
            Sub,
            Times,
            Div,
        }

        private bool _isPostEqual = false;

        private decimal? _leftOperand = null;
        public decimal? LeftOperand
        {
            get { return _leftOperand; }
            set
            {
                if (_leftOperand != value)
                {
                    _leftOperand = value;
                    OnPropertyChanged();
                }
            }
        }

        private decimal? _rightOperand = null;
        public decimal? RightOperand
        {
            private get { return _rightOperand; }
            set
            {
                if (_rightOperand != value)
                {
                    _rightOperand = value;
                    OnPropertyChanged();
                }
            }
        }

        private CalcModeEnum _calcMode = CalcModeEnum.None;
        public CalcModeEnum CalcMode
        {
            get
            {
                return _calcMode;
            }
            set
            {
                if (_calcMode != value)
                {
                    _calcMode = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _displayText = "";
        public string DisplayText
        {
            get
            {
                return _displayText;
            }
            set
            {
                if (_displayText != value)
                {
                    _displayText = value;
                    OnPropertyChanged();
                }
            }
        }

        public DelegateCommand<string> InputNumCommand { get; private set; }

        public DelegateCommand<string> SetCalcModeCommand { get; private set; }

        public DelegateCommand GetResultCommand { get; private set; }

        public DelegateCommand ClearCommand { get; private set; }

        public MainWindowVM()
        {
            InputNumCommand = new DelegateCommand<string>(InputNum);
            SetCalcModeCommand = new DelegateCommand<string>(SetCalcMode);
            GetResultCommand = new DelegateCommand(GetResult);
            ClearCommand = new DelegateCommand(Clear);
        }

        private void InputNum(string in_str)
        {
            try
            {
                if (_isPostEqual)
                {
                    DisplayText = "";
                }

                _isPostEqual = false;

                string tmp_text = DisplayText;

                if (in_str == ".")
                {
                    DisplayText = tmp_text + in_str;
                    return;
                }

                tmp_text = DisplayText + in_str;

                if (decimal.TryParse(tmp_text, out decimal dec_num))
                {
                    DisplayText = dec_num.ToString("N0");
                }
                else
                {
                    DisplayText = "Err";
                }
            }
            catch (Exception)
            {
                DisplayText = "Err";
            }
        }

        private void SetCalcMode(string mode_string)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(DisplayText))
                {
                    if (LeftOperand == null || RightOperand != null)
                    {
                        Clear();
                        return;
                    }

                    switch (mode_string)
                    {
                        case "+":
                            CalcMode = MainWindowVM.CalcModeEnum.Add;
                            break;
                        case "-":
                            CalcMode = MainWindowVM.CalcModeEnum.Sub;
                            break;
                        case "*":
                            CalcMode = MainWindowVM.CalcModeEnum.Times;
                            break;
                        case "/":
                            CalcMode = MainWindowVM.CalcModeEnum.Div;
                            break;
                        default:
                            CalcMode = MainWindowVM.CalcModeEnum.None;
                            break;
                    }
                    return;
                }

                if (!decimal.TryParse(DisplayText, out decimal disp_dec))
                {
                    Clear();
                    return;
                }

                if (LeftOperand == null && RightOperand != null)
                {
                    Clear();
                    return;
                }

                if (LeftOperand == null && RightOperand == null)
                {
                    LeftOperand = disp_dec;
                    DisplayText = "";
                }

                if (LeftOperand != null && RightOperand == null)
                {
                    RightOperand = disp_dec;
                }

                switch (mode_string)
                {
                    case "+":
                        CalcMode = MainWindowVM.CalcModeEnum.Add;
                        break;
                    case "-":
                        CalcMode = MainWindowVM.CalcModeEnum.Sub;
                        break;
                    case "*":
                        CalcMode = MainWindowVM.CalcModeEnum.Times;
                        break;
                    case "/":
                        CalcMode = MainWindowVM.CalcModeEnum.Div;
                        break;
                    default:
                        CalcMode = MainWindowVM.CalcModeEnum.None;
                        break;
                }

                if (_isPostEqual)
                {
                    DisplayText = "";
                }

                _isPostEqual = false;
            }
            catch (Exception)
            {
                DisplayText = "Err";
            }
        }

        private void GetResult()
        {
            try
            {
                if (!decimal.TryParse(DisplayText, out _) && LeftOperand != null && RightOperand == null)
                {
                    DisplayText = LeftOperand.Value.ToString("N0");
                    return;
                }

                if (decimal.TryParse(DisplayText, out decimal disp_dec) && LeftOperand != null)
                {
                    RightOperand = disp_dec;

                    switch (CalcMode)
                    {
                        case CalcModeEnum.Add:
                            DisplayText = (LeftOperand.Value + RightOperand.Value).ToString("N0");
                            LeftOperand = (LeftOperand.Value + RightOperand.Value);
                            break;
                        case CalcModeEnum.Sub:
                            DisplayText = (LeftOperand.Value - RightOperand.Value).ToString("N0");
                            LeftOperand = (LeftOperand.Value - RightOperand.Value);
                            break;
                        case CalcModeEnum.Times:
                            DisplayText = (LeftOperand.Value * RightOperand.Value).ToString("N0");
                            LeftOperand = (LeftOperand.Value * RightOperand.Value);
                            break;
                        case CalcModeEnum.Div:
                            DisplayText = (LeftOperand.Value / RightOperand.Value).ToString("N0");
                            LeftOperand = (LeftOperand.Value / RightOperand.Value);
                            break;
                        default:
                            Clear();
                            break;
                    }

                    CalcMode = CalcModeEnum.None;
                    RightOperand = null;
                    _isPostEqual = true;
                    return;
                }

                Clear();
            }
            catch (Exception)
            {
                DisplayText = "Err";
            }
        }

        private void Clear()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(DisplayText) && LeftOperand != null)
                {
                    DisplayText = "";
                    return;
                }

                CalcMode = CalcModeEnum.None;
                DisplayText = "";
                LeftOperand = null;
                RightOperand = null;
            }
            catch (Exception)
            {
                DisplayText = "Err";
            }
        }
    }
}
