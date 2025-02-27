using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
//This package is used in substitution for System.Text.Json as .Net Framework does not include that package
//All Code made by References their documentation https://www.newtonsoft.com/json/help/html/Introduction.html (accessed 27/02/25 04:00)
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace LEALANG_V0
{
    public partial class Form2 : Form
    {
        //This variable will change after the prototype. This is a placeholder for now
        string ChosenLang = "Python";
        public Form2()
        {
            InitializeComponent();
        }

        //These Classses were created by Visual Studio via Edit -> Paste Special -> Paste Json As Classes
        //These Classes are needed to Deserialise the json file.
        public class Rootobject
        {
            public List<Basicmulti> BasicMulti { get; set; }
            public List<Basicstate> BasicState { get; set; }
        }

        public class Basicmulti
        {
            public int QuestionID { get; set; }
            public string Question { get; set; }
            public string[] Answers { get; set; }
            public string CorrectAnswer { get; set; }
        }

        public class Basicstate
        {
            public int QuestionID { get; set; }
            public string Question { get; set; }
            public string CorrectAnswer { get; set; }
            public string Hint { get; set; }
        }





        private void Form2_Load(object sender, EventArgs e)
        {
            //Deserialise the right languages JSON file
            string FileLoc = ChosenLang + ".json";

            //Deserialises The JSON file.
            Rootobject root = JsonConvert.DeserializeObject<Rootobject>(File.ReadAllText(FileLoc));

            //This choses the question. I only have 2 so only 0 and 1 is needed
            int ChosenNum = new Random().Next(0, 2);

            //This just singles out both a hint and a question
            string H = JsonConvert.SerializeObject(root.BasicState[ChosenNum].Hint);
            string Q = JsonConvert.SerializeObject(root.BasicState[ChosenNum].Question);

            //This assigns them to a label (TEXT ELEMENT)
            label1.Text = Q;
            label2.Text = H;
        }

    }
}
