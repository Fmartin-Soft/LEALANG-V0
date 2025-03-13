using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LEALANG_V0
{
    public partial class Home : Form
    {
        string ChosenLang;
        string ChosenDif;
        public Home(string LangChosen, int score)
        {
            
            ChosenLang = LangChosen;
            InitializeComponent();
            label1.Text = "Score: " + score.ToString();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            ChosenLang = "Python";
            Form basint = new BasicInter();
            basint.ShowDialog();
            if (basint.DialogResult == DialogResult.OK)
            {
                ChosenDif = "Basic";
            }
            else
            {
                ChosenDif = "Int";
            }
            this.Hide();
            new Quiz(ChosenLang + ChosenDif).ShowDialog();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ChosenLang = "C#";
            Form basint = new BasicInter();
            basint.ShowDialog();
            if (basint.DialogResult == DialogResult.OK)
            {
                ChosenDif = "Basic";
            }
            else
            {
                ChosenDif = "Int";
            }
            this.Hide();
            new Quiz(ChosenLang + ChosenDif).ShowDialog();
            this.Close();
        }
    }
}
