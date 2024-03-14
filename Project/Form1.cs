using System.Windows.Forms;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using CheckBox = System.Windows.Forms.CheckBox;
using System.Diagnostics.PerformanceData;

namespace Project
{
    public partial class Form1 : Form
    {
        private CheckBox[] checkBoxes;
        private int[,] dataArray;
        private Chart chart;
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e) { }

        private void Form1_Load(object sender, EventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {
            string temp;
            OpenFileDialog fileDialog = new OpenFileDialog();

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                var sr = new StreamReader(fileDialog.FileName);

                temp = sr.ReadToEnd();
                dataArray = strToArray(temp);
                NumOfColumns(strToArray(temp));
                //SetText(temp);
                DrawChart();
            }
        }

       /* private void SetText(string text)
        {
            textBox1.Text = text;
        }*/

        private int[,] strToArray(string text)
        {
            string[] rowsData = text.Trim('[', ']').Split("],[");
            int rows = rowsData.Length;
            int columns = rowsData[0].Split(',').Length;

            int[,] array = new int[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                string rowData = rowsData[i].Replace("[", "").Replace("]", "");
                string[] values = rowData.Split(',');

                for (int j = 0; j < columns; j++)
                {
                    if (!string.IsNullOrWhiteSpace(values[j]))
                    {
                        array[i, j] = int.Parse(values[j].Trim());
                    }
                    else
                    {
                        array[i, j] = 0;
                    }
                }
            }
            return array;
        }

        private int NumOfColumns(int[,] arr)
        {
            int columns = arr.GetLength(1);
            string col = columns.ToString();
            textBox2.AppendText($"Количество столбцов в данном файле: {col}");
            CreateCheckBox(columns);

            return columns;
        }

        private void CreateCheckBox(int length)
        {
            const int startingX = 20;
            const int startingY = 280;
            const int spacing = 25;

            checkBoxes = new CheckBox[length];

            for (int i = 0; i < length; i++)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Text = "CheckBox " + (i + 1);
                checkBox.AutoSize = true;
                checkBox.Location = new System.Drawing.Point(startingX, startingY + (spacing * i));

                checkBox.CheckedChanged += CheckBox_CheckedChanged;
                checkBoxes[i] = checkBox;
                this.Controls.Add(checkBox);
            }
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            DrawChart();
        }

        private void DrawChart()
        {
            if (Controls.Contains(chart))
            {
                Controls.Remove(chart);
                chart.Dispose();
            }

            chart = new Chart();
            chart.Size = new Size(500, 500);
            chart.Location = new Point(200, 250);
            chart.ChartAreas.Add(new ChartArea());

            Series series = chart.Series.Add("Data");
            series.ChartType = SeriesChartType.Column;

            for (int i = 0; i < dataArray.GetLength(0); i++)
            {
                double xValue = i;
                double yValue = 0;

                for (int j = 0; j < checkBoxes.Length; j++)
                {
                    if (checkBoxes[j].Checked)
                    {
                        yValue += dataArray[i, j];
                    }
                }

                series.Points.AddXY(xValue, yValue);
            }

            Controls.Add(chart);
        }

        private void GetSolvingFormula()
        {

        }
    }
}


/*
[[1, 3, 2 ,4 ,5, 7],
 [1, 3, 2 ,4 ,5, 8],
 [1, 3, 2 ,4 ,5, 9],
 [1, 3, 2 ,4 ,5, 2],
 [1, 3, 2 ,4 ,5, 3],
 [1, 3, 2 ,4 ,5, 8],
 [1, 3, 2 ,4 ,5, 11],
 [1, 3, 2 ,4 ,5, 5],
 [1, 3, 2 ,4 ,5, 6],
 [1, 3, 2 ,4 ,5, 5],
 [1, 3, 2 ,4 ,5, 8]]
*/