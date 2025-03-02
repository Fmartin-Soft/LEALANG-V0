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
//All Code made by References their documentation https://www.newtonsoft.com/json/help/html/Introduction.htm (accessed 27/02/25 04:00)
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace LEALANG_V0
{
    public partial class Quiz : Form
    {
        //This variable will change after the prototype. This is a placeholder for now
        // string ChosenLang = Initialisation.LangChosen; <- I will get this to work later
        string ChosenLang = "Python" + ".json";

        string A; //Answer
        string H; //Hint
        string Q; //Question

        bool Check = false; //Quick check for amount of q's answered 

        //array to figure out the users answered questions
        List<int> AnsweredQuestions = new List<int>();

        //need in the class
        int ChosenNum;

        //Deserialises The JSON file.
        Rootobject root;
        public Quiz()
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

        private void MakeNewQuestion(Rootobject root)
        {
            //This choses the question. I only have 2 so only 0 and 1 is needed
            ChosenNum = new Random().Next(0, 2);

            while (AnsweredQuestions.Contains(ChosenNum))
            {

                ChosenNum = new Random().Next(0, 2);

                if (AnsweredQuestions.SequenceEqual(Enumerable.Range(0, 2)))
                {
                    Check = true;
                    return;
                }
            }

            SerialiseBasicState(ChosenNum, root);

            //This assigns them to a label (TEXT ELEMENT)
            label1.Text = Q;
            label2.Text = H;
        }


        //This being in a function just looks nicer.
        private void SerialiseBasicState(int ChosenNum, Rootobject root)
        {
            //This just singles out both a hint and a question
            H = root.BasicState[ChosenNum].Hint; //H = Hint
            Q = root.BasicState[ChosenNum].Question; //Q = Question
            A = root.BasicState[ChosenNum].CorrectAnswer; //A = Answer
        }


        private void Form2_Load(object sender, EventArgs e)
        {
            root = JsonConvert.DeserializeObject<Rootobject>(File.ReadAllText(ChosenLang));

            MakeNewQuestion(root);

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (textBox1.Text == A)
                {
                    label3.Text = "Correct";

                    //Do some funky scoring stuff here


                }
                else
                {
                    label3.Text = "Better luck next time!";

                    //Do some funky scoring stuff here

                }
                //add to answered questions
                AnsweredQuestions.Add(ChosenNum);

                //New Question
                MakeNewQuestion(root);

                if (Check == true){
                    new Home().ShowDialog();
                    this.Close();
                }
            }
        }
    }
}
