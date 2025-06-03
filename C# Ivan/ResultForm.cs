using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Master
{
    public class ResultForm : Form
    {
        private TextBox textBox;

        public ResultForm(List<string> data)
        {
            this.Text = "Results from agents";
            this.Width = 600;
            this.Height = 400;

            textBox = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Dock = DockStyle.Fill
            };

            this.Controls.Add(textBox);

            foreach (var line in data)
            {
                textBox.AppendText(line + Environment.NewLine);
            }
        }
    }
}
