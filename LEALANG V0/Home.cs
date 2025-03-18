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
        public Home(string LangChosen, int score=0)
        {
            
            ChosenLang = LangChosen;
            InitializeComponent();
            label1.Text = "Hello: USER";

        }
    }
}
