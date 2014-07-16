using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Common.Helpers
{
    public static class Utils
    {
        /// <summary>
        /// Проверка текста на наличие цифр (silent)
        /// </summary>
        /// <param name="text">текст</param>
        /// <returns>true/false</returns>
        public static bool CheckTextOnDigit(string text)
        {
            var result = false;

            try
            {
                text.Trim()
                    .AsEnumerable()
                    .Select(c => Convert.ToInt32(c.ToString(CultureInfo.InvariantCulture)))
                    .ToArray();
                result = true;
            }
            catch
            {
            }

            return result;
        }

        /// <summary>
        /// Проверка текстf на наличие цифр (exception)
        /// </summary>
        /// <param name="text">текст</param>
        public static void CheckUpTextOnDigit(string text)
        {
            try
            {
                text.Trim()
                    .AsEnumerable()
                    .Select(c => Convert.ToInt32(c.ToString(CultureInfo.InvariantCulture)))
                    .ToArray();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Неверное значение.", text, ex);
            }
        }

        /// <summary>
        /// Позиционирование на указанном значении в указанной колонке грида
        /// </summary>
        /// <param name="grid">грид</param>
        /// <param name="colName"> колонка</param>
        /// <param name="value">значение</param>
        public static void LocateAt(this DataGridView grid, string colName, string value)
        {
            if (grid.CurrentRow != null)
            {
                try
                {
                    var rows = from DataGridViewRow row in grid.Rows
                               where
                                   row.Cells[colName].Value.ToString().ToUpper() == value.ToUpper()
                               select row;
                    var frow = rows.FirstOrDefault();
                    if (frow != null)
                    {
                        grid.CurrentCell = frow.Cells[0];
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Режим read-only для TextBox
        /// </summary>
        /// <param name="textBox"></param>
        public static void MakeReadOnly(this TextBox textBox)
        {
            textBox.ReadOnly = true;
            textBox.BackColor = SystemColors.Info;
        }

        /// <summary>
        /// Разблокирует Ctrl+C; Ctrl+V
        /// </summary>
        public static class TextBoxWithCopy
        {
            public static void _KeyDown(object sender, KeyEventArgs e)
            {
                var textBox = sender as TextBox;
                if (textBox == null)
                {
                    return;
                }

                if (e.Control & e.KeyCode == Keys.V)
                {
                    try
                    {
                        var s = Clipboard.GetText();
                        s.AsEnumerable()
                            .Select(c => Convert.ToInt32(c.ToString(CultureInfo.InvariantCulture)))
                            .ToArray();
                        textBox.Text = s;
                        textBox.SelectionStart = textBox.Text.Length;
                    }
                    catch
                    {
                    }
                }
                else if ((e.Control & e.KeyCode == Keys.C) && !string.IsNullOrWhiteSpace(textBox.Text))
                {
                    try
                    {
                        Clipboard.SetText(string.IsNullOrWhiteSpace(textBox.SelectedText)
                            ? textBox.Text
                            : textBox.SelectedText);
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}