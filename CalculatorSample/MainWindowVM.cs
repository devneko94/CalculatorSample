using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CalculatorSample
{
    class MainWindowVM : BindableBase
    {
        #region 列挙体
        public enum CalcModeEnum
        {
            None,
            Add,
            Sub,
            Times,
            Div,
        }
        #endregion

        #region 定数
        private readonly string ERR_TEXT = "Err";

        private readonly string DOT_TEXT = ".";
        #endregion

        #region プライベートフィールド
        private bool _isPostEqual = false;
        #endregion

        #region プロパティ
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
        #endregion

        #region コマンド
        public DelegateCommand<string> InputNumCommand { get; private set; }

        public DelegateCommand<string> SetCalcModeCommand { get; private set; }

        public DelegateCommand RunCalcCommand { get; private set; }

        public DelegateCommand ClearCommand { get; private set; }
        #endregion

        #region コンストラクタ
        public MainWindowVM()
        {
            InputNumCommand = new DelegateCommand<string>(InputNum);
            SetCalcModeCommand = new DelegateCommand<string>(SetCalcMode);
            RunCalcCommand = new DelegateCommand(RunCalc);
            ClearCommand = new DelegateCommand(Clear);
        }
        #endregion

        #region プライベートメソッド
        private void InputNum(string in_str)
        {
            try
            {
                if (_isPostEqual)
                {
                    DisplayText = "";
                }

                _isPostEqual = false;

                if (in_str == DOT_TEXT && !DisplayText.Contains(DOT_TEXT))
                {
                    DisplayText = DisplayText + in_str;
                    return;
                }
                else if (in_str == DOT_TEXT && DisplayText.Contains(DOT_TEXT))
                {
                    return;
                }

                DisplayText = DisplayText + in_str;

                if (decimal.TryParse(DisplayText, out decimal dec_num))
                {
                    DisplayText = dec_num.ToString();
                }
                else
                {
                    DisplayText = ERR_TEXT;
                }
            }
            catch (Exception)
            {
                DisplayText = ERR_TEXT;
            }
        }

        private void SetCalcMode(string mode_string)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(DisplayText))
                {
                    if (LeftOperand == null || (RightOperand != null && CalcMode == CalcModeEnum.None))
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
                else if (LeftOperand == null && RightOperand == null)
                {
                    string result_str = disp_dec.ToString();

                    if (Regex.IsMatch(result_str, @"\.0+$"))
                    {
                        LeftOperand = decimal.Parse(Regex.Replace(result_str, @"\.0+$", ""));
                    }
                    else if ((result_str?.Contains(".") ?? false) && Regex.IsMatch(result_str, @"0+$"))
                    {
                        LeftOperand = decimal.Parse(Regex.Replace(result_str, @"0+$", ""));
                    }
                    else
                    {
                        LeftOperand = disp_dec;
                    }

                    DisplayText = "";
                }
                else if (LeftOperand != null && RightOperand == null)
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
                DisplayText = ERR_TEXT;
            }
        }

        private void RunCalc()
        {
            try
            {
                if (!decimal.TryParse(DisplayText, out _) && LeftOperand != null && RightOperand == null)
                {
                    DisplayText = LeftOperand?.ToString();
                    return;
                }

                if (decimal.TryParse(DisplayText, out decimal disp_dec) && LeftOperand != null)
                {
                    RightOperand = disp_dec;
                    decimal? result = null;

                    switch (CalcMode)
                    {
                        case CalcModeEnum.Add:
                            result = (LeftOperand.Value + RightOperand.Value);
                            break;
                        case CalcModeEnum.Sub:
                            result = (LeftOperand.Value - RightOperand.Value);
                            break;
                        case CalcModeEnum.Times:
                            result = (LeftOperand.Value * RightOperand.Value);
                            break;
                        case CalcModeEnum.Div:
                            result = (LeftOperand.Value / RightOperand.Value);
                            break;
                        default:
                            Clear();
                            return;
                    }

                    string result_str = result?.ToString();

                    if (Regex.IsMatch(result_str, @"\.0+$"))
                    {
                        DisplayText = Regex.Replace(result_str, @"\.0+$", "");
                    }
                    else if ((result_str?.Contains(".") ?? false) && Regex.IsMatch(result_str, @"0+$"))
                    {
                        DisplayText = Regex.Replace(result_str, @"0+$", "");
                    }
                    else
                    {
                        DisplayText = result_str;
                    }

                    CalcMode = CalcModeEnum.None;
                    LeftOperand = decimal.Parse(DisplayText);
                    RightOperand = null;

                    _isPostEqual = true;
                }
                else
                {
                    Clear();
                }
            }
            catch (Exception)
            {
                DisplayText = ERR_TEXT;
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
                DisplayText = ERR_TEXT;
            }
        }
        #endregion
    }
}
