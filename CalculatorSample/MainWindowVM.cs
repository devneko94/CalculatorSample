using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

                string tmp_text = DisplayText;

                if (in_str == ".")
                {
                    DisplayText = tmp_text + in_str;
                    return;
                }

                tmp_text = DisplayText + in_str;

                if (decimal.TryParse(tmp_text, out decimal dec_num))
                {
                    DisplayText = GetNumForDisplay(dec_num);
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

        private void RunCalc()
        {
            try
            {
                if (!decimal.TryParse(DisplayText, out _) && LeftOperand != null && RightOperand == null)
                {
                    DisplayText = GetNumForDisplay(LeftOperand);
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

                    DisplayText = GetNumForDisplay(result);
                    CalcMode = CalcModeEnum.None;
                    LeftOperand = result;
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

        private string GetNumForDisplay(decimal? in_num)
        {
            if (in_num == null)
            {
                return "";
            }

            string result = "";
            decimal int_part = Math.Floor(in_num.Value);
            decimal dec_part = in_num.Value - int_part;

            if (in_num.Value.ToString().Contains("."))
            {
                result = int_part.ToString("N0") + dec_part.ToString("0.".PadRight(29, '#')).Substring(1);
            }
            else
            {
                result = int_part.ToString("N0");
            }

            return result;
        }
        #endregion
    }
}
