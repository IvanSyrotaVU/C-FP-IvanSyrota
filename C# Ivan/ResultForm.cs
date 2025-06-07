using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Master
{
    public class ResultForm : Form
    {
        private Label statusLabel;
        private TextBox textBox;

        public ResultForm(List<string> data)
        {
            this.Text = "Results from agents";
            this.Width = 600;
            this.Height = 400;

            statusLabel = new Label
            {
                Text = "Receiving data from agents...",
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0)
            };

            textBox = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Dock = DockStyle.Fill
            };

            this.Controls.Add(textBox);
            this.Controls.Add(statusLabel);

            // Отобразить результат
            foreach (var line in data)
            {
                textBox.AppendText(line + Environment.NewLine);
            }

            // Обновим статус
            statusLabel.Text = $"Done! {data.Count} items received.";
        }
    }
}

