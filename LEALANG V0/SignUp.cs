using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.IdentityModel.Tokens;

namespace LEALANG_V0
{
    public partial class SignUp : Form
    {
        int curloop = 5;
        public SignUp()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            foreach ( TextBox textbox in this.Controls.OfType<TextBox>()) //checking each textbox in the form
            {
                curloop--;
                if (String.IsNullOrEmpty(textbox.Text)) //if the textbox its currently on is empty or null
                {
                    Control lab = this.Controls["label" + curloop.ToString()];
                    lab.Visible = true;
                }
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
