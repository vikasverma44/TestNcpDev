using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace SQLDataMaskingConfigurator.Helpers
{
    public static class Utility
    {
        public static DialogResult ShowMessageBox(string Msg, MessageBoxButtons messageBoxButtons)
        {
            return MessageBox.Show(Msg, Constants.MessageBoxTitle, messageBoxButtons, MessageBoxIcon.Information);
        }
        public static DialogResult ShowErrorMessageBox(string Msg)
        {
            return MessageBox.Show(Msg, Constants.MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static DialogResult ShowConfirmationBox(string Msg, MessageBoxButtons messageBoxButtons)
        {
            return MessageBox.Show(Msg, Constants.MessageBoxTitle, messageBoxButtons, MessageBoxIcon.Warning);
        }
        public static bool AllowOnlyNumericEntry(char KeyChar)
        {
            if (!char.IsDigit(KeyChar) && (KeyChar != (char)Keys.Back))
            {
                return true;
            }
            else { return false; }
        }

        public static string GetMaskedFieldValue(string fieldValue, bool IsRandamValues = true)
        {
            char randomChar;
            int randomInt;
            Random randomValue = new Random();
            StringBuilder objStringBuilder = new StringBuilder();
            string strVal;
            object obj = new object();
            lock (obj)
            {
                objStringBuilder.Clear();
                strVal = string.Empty;
                if (IsRandamValues)
                {
                    foreach (char ch in fieldValue)
                    {
                        if (Char.IsLetter(ch))
                        {
                            randomChar = (char)randomValue.Next('A', 'Z');
                            objStringBuilder.Append(randomChar);
                        }
                        else if (Char.IsNumber(ch))
                        {
                            randomInt = randomValue.Next(0, 9);
                            objStringBuilder.Append(randomInt);
                        }
                        else
                        {
                            objStringBuilder.Append(ch);
                        }
                    }
                    strVal = objStringBuilder.ToString();
                }
                else
                {
                    if (int.TryParse(fieldValue, out int intVal))
                    {
                        if (intVal % 9 == 0)
                        {
                            intVal += 1;
                            return Convert.ToString(intVal);
                        }
                    }

                    foreach (char ch in fieldValue)
                    {
                        if (Char.IsLetter(ch))
                        {
                            randomChar = (char)(((int)(ch == 'z' ? '`' : (ch == 'Z' ? '@' : ch))) + 1);
                            objStringBuilder.Append(randomChar);
                        }
                        else if (Char.IsNumber(ch))
                        {
                            randomInt = (Convert.ToInt32(ch.ToString()) + 1).ToString().Length > 1 ? 0 : Convert.ToInt32(ch.ToString()) + 1;
                            objStringBuilder.Append(randomInt);
                        }
                        else
                        {
                            objStringBuilder.Append(ch);
                        }
                    }

                    strVal = objStringBuilder.ToString();
                    if (strVal.StartsWith("0"))
                    { strVal = strVal.Replace("0", "1"); }
                    //if (int.TryParse(strVal, out int intStrVal))
                    //{
                    //    if (intStrVal == 0)
                    //    { strVal = strVal.Replace("0", "1"); }
                    //}
                }
            }

            return strVal;
        }
        public static IEnumerable<string> GetDescriptions(Type type)
        {
            var descs = new List<string>();
            var names = Enum.GetNames(type);
            foreach (var name in names)
            {
                var field = type.GetField(name);
                var fds = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                foreach (DescriptionAttribute fd in fds)
                {
                    descs.Add(fd.Description);
                }
            }
            return descs;
        }

    }
}
