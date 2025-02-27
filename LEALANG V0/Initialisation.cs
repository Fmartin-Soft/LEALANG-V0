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
    public partial class Initialisation : Form
    {
        //Global string LangChosen <-- i need something like this so user can decide language to use

        public Initialisation()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            new Quiz().ShowDialog();
            this.Close();
        }
    }
}
