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
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using System.Runtime.Remoting.Channels;

namespace LEALANG_V0
{
    public partial class Quiz : Form
    {
        //This variable will change after the prototype. This is a placeholder for now
        // string ChosenLang = Initialisation.LangChosen; <- I will get this to work later
        string ChosenJSON;
        string ChosenLang;
        string CA; //Correct Answer
        string H; //Hint
        string Q; //Question

        //For MultiQuestions
        string A1;
        string A2;
        string A3;
        string A4;

        bool Check = false; //Quick check for amount of q's answered 

        Random rnd = new Random();

        //array to figure out the users answered questions
        List<int> AnsweredQuestions = new List<int>();

        //need in the class
        int ChosenNum;

        //to allow the program to choose the Question type
        int ChosenQType;

        //Deserialises The JSON file.
        Rootobject root;

        int[] ints = { 0, 1, 2, 3 };

        int Score;

        string nameID;

        object snder;

        int UserScore;

        DatabaseFunctions DBfuncs = new DatabaseFunctions();

        //the basic function for any form
        public Quiz(string LangChosen,string nameid)
        {
            nameID = nameid;
            ChosenLang = LangChosen;
            UserScore = DBfuncs.GetScoreBase(nameid); //Getting the usersdb score to determine the difficulty
            if (UserScore >= 120)
            {
                ChosenJSON = LangChosen + "Basic.json";
            }
            else
            {
                ChosenJSON = LangChosen + "Int.json";
            }

            InitializeComponent();
        }

        //These Classses were created by Visual Studio via Edit -> Paste Special -> Paste Json As Classes
        //These Classes are needed to Deserialise the json file.
        public class Rootobject
        {
            public List<multi> Multi { get; set; }
            public List<state> State { get; set; }
        }

        public class multi
        {
            public int QuestionID { get; set; }
            public string Question { get; set; }
            public string[] Answers { get; set; }
            public string CorrectAnswer { get; set; }
        }

        public class state
        {
            public int QuestionID { get; set; }
            public string Question { get; set; }
            public string CorrectAnswer { get; set; }
            public string Hint { get; set; }
        }


        //as name suggests this functions always gets called after a question
        public void AfterQuestionReview(bool correct = false)
        {
            this.Size = new System.Drawing.Size(393, 328); // sets the size of the form a bit bigger to allow for the info
            label3.Visible = true;
            label4.Visible = true;
            button5.Visible = true;
            if (correct) //if the correct boolean variable is true, having it as if(correct) is the same as if(true)
            {
                label3.Text = "Correct!";
                label4.Text = "Well Done";
            }
            else
            {
                label3.Text = "Incorrect!";
                label4.Text = "The Correct Answer was: " + CA;
            }
        }

        //make a new state question
        private void MakeNewStateQuestion(Rootobject root)
        {
            //This choses the question. I only have 2 so only 0 and 1 is needed
            ChosenNum = rnd.Next(0, 30);

            if (AnsweredQuestions.Count() == 4)
            {
                Check = true;
            }

            //Assigns the new Q
            SerialiseBasicState(ChosenNum, root);

            //This assigns them to a label (TEXT ELEMENT)
            label1.Text = H;
            label2.Text = Q;
        }

        private void MakeNewMultiQuestion(Rootobject root)
        {
            //This choses the question. I have 30 so only 0 and 30 is needed
            ChosenNum = rnd.Next(0, 30);
           
            if (AnsweredQuestions.Count() == 4)
            {
                Check = true;
            }
            // THIS FOR LOOP IS NOT MY CODE
            for (int i = 0; i < 4; i++)
            {
                //Fisher-Yates shuffling algorithm https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle (Accessed 03/03/25 23:51) 
                
                int r = rnd.Next(i, ints.Length);
                (ints[r], ints[i]) = (ints[i], ints[r]);
            }
            //Assigns the new Q
            SerialiseBasicMulti(ChosenNum, root, ints);


            //This assigns them to a label (TEXT ELEMENT)
            label1.Text = H;
            label2.Text = Q;
            button1.Text = A1;
            button2.Text = A2;
            button3.Text = A3;
            button4.Text = A4;
        }

        //This being in a function just looks nicer.
        private void SerialiseBasicMulti(int ChosenNum, Rootobject root, int[] ints)
        {
            //This just singles out both a hint and a question
            H = root.Multi[ChosenNum].Question; //H = Hint
            CA = root.Multi[ChosenNum].CorrectAnswer; //CA = Correct Answer
            A1 = root.Multi[ChosenNum].Answers[ints[0]]; //A1 = Answer 1
            A2 = root.Multi[ChosenNum].Answers[ints[1]]; //A2 = Answer 2
            A3 = root.Multi[ChosenNum].Answers[ints[2]]; //A3 = Answer 3
            A4 = root.Multi[ChosenNum].Answers[ints[3]]; //A4 = Answer 4
        }

        //This being in a function just looks nicer.
        private void SerialiseBasicState(int ChosenNum, Rootobject root)
        {
            //This just singles out both a hint and a question
            Q = root.State[ChosenNum].Hint; //Q = Question
            H = root.State[ChosenNum].Question; //H = Hint
            CA = root.State[ChosenNum].CorrectAnswer; //A = Answer
        }


        private void Form2_Load(object sender, EventArgs e)
        {
            //"Depacks" the json
            root = JsonConvert.DeserializeObject<Rootobject>(File.ReadAllText(ChosenJSON));

            //Choosing the Question type
            ChosenQType = rnd.Next(0, 2);

            switch (ChosenQType) 
            {
                case 0:
                    MakeNewStateQuestion(root);
                    break;
                case 1:
                    MakeNewMultiQuestion(root);
                    textBox1.Enabled = false;
                    textBox1.Visible = false;
                    button1.Visible = true;
                    button2.Visible = true;
                    button3.Visible = true; 
                    button4.Visible = true;
                    button1.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    break;
            }

        }


        private void AnsCheck(object sender)
        {
            if (sender is Button btn)
            {
                snder = sender;
                // bassically if entered answer if correct / incorrect
                switch (btn.Text == CA)
                {
                    case true:
                        AfterQuestionReview(true);
                        //Do some funky scoring stuff here
                        Score++;
                        break;
                    case false:
                        AfterQuestionReview();
                        //Do some funky scoring stuff here
                        break;
                }




            }
            else if (sender is TextBox textbox)
            {
                snder = sender;
                // bassically if entered answer if correct / incorrect
                textbox.Enabled = false;
                switch (textbox.Text == CA)
                {
                    case true:
                        AfterQuestionReview(true);
                        textbox.Text = null;
                        //Do some funky scoring stuff here
                        Score++;

                        break;
                    case false:
                        AfterQuestionReview();
                        textbox.Text = null;
                        //Do some funky scoring stuff here

                        break;
                }




            }

        }



        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                AnsCheck(sender);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            AnsCheck(sender);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AnsCheck(sender);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AnsCheck(sender);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AnsCheck(sender);
        }


        //when the "next question" button is pressed
        private void button5_Click(object sender, EventArgs e)
        {
            if (Check == true)
            {
                this.Hide();
                new Home(ChosenLang, nameID, Score).ShowDialog();
                this.Close();
            }
            this.Size = new System.Drawing.Size(393, 258);
            label3.Visible = false;
            label4.Visible = false;
            button5.Visible = false;
            AnsweredQuestions.Add(ChosenNum); //add to answered questions
            if (snder is Button btn)
            {
                MakeNewMultiQuestion(root); //New Question
            }
            else if(snder is TextBox textbox)
            {
                textbox.Enabled = true; //reenable the textbox
                
                MakeNewStateQuestion(root); //New Question
            }

        }
    }
}
