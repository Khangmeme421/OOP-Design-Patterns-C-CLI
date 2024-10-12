using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comp1551_Coursewark
{
    // Menu class interface
    public abstract class Menu
    {
        public List<string> MenuItems { get; set; }

        public abstract string DisplayMenu();
    }

    public class MainMenu : Menu
    {
        public MainMenu()
        {
            MenuItems = new List<string> { "Exit program", "Create new user", "Login as user" };
        }

        public override string DisplayMenu()
        {
            Console.WriteLine("Menu:");
            for (int i = 0; i < MenuItems.Count; i++)
            {
                Console.WriteLine($"{i}. {MenuItems[i]}");
            }
            Console.Write("Choose an option: ");
            string option = Console.ReadLine();
            while (option != "0" && option != "1" && option != "2")
            {
                Console.Write("Invalid option. Please choose again: ");
                option = Console.ReadLine();
            }
            return option;
        }
    }

    public class ManageUserMenu : Menu
    {
        public ManageUserMenu()
        {
            MenuItems = new List<string> { "Back", "Create new user", "Delete user", "Edit user name" };
        }

        public override string DisplayMenu()
        {
            Console.WriteLine("Menu:");
            for (int i = 0; i < MenuItems.Count; i++)
            {
                Console.WriteLine($"{i}. {MenuItems[i]}");
            }
            Console.Write("Choose an option: ");
            string option = Console.ReadLine();
            while (option != "0" && option != "1" && option != "2" && option != "3")
            {
                Console.Write("Invalid option. Please choose again: ");
                option = Console.ReadLine();
            }
            return option;
        }
    }

    // User class interface
    public abstract class User
    {
        private string _userName;
        private string _role;
        public abstract string UserName { get; set; }
        public abstract string Role { get; set; }
        public User(string userName, string role)
        {
            UserName = userName;
            Role = role;
        }
    }
    public class QuestionGiver : User
    {
        private string _userName;
        private string _role;

        public override string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public override string Role
        {
            get { return _role; }
            set { _role = value; }
        }
        public QuestionGiver(string userName, string role) : base(userName, role)
        {
            this.UserName = userName;
            this.Role = role;
        }
        public void CreateQuestion()
        {
            // TO DO: implement the CreateQuestion method
            // You can add your own implementation here
        }
    }

    // Player class is a subclass of User
    public class Player : User
    {
        private string _userName;
        private string _role;
        private int _score;     //Has score attribute in addition
        public override string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public override string Role
        {
            get { return _role; }
            set { _role = value; }
        }
        public Player(string userName, string role) : base(userName, role)
        {
            this.UserName = userName;
            this.Role = role;
            _score = 0;
        }
        public void AnswerQuestion()
        {
            // TO DO: implement the CreateQuestion method
            // You can add your own implementation here
        }
    }

    // Question class
    public abstract class Question
    {
        public string Title { get; set; }
        public string CorrectAnswer { get; set; }
        public int Points { get; set; }

        public Question(string title, string correctAnswer)
        {
            Title = title;
            CorrectAnswer = correctAnswer;
        }

        public abstract bool CheckAnswer(string answer);
        public abstract void DisplayQuestion(int questionNumber);
    }
    public class TrueFalseQuestion : Question
    {
        public override void DisplayQuestion(int questionNumber)
        {
            Console.WriteLine($"{questionNumber}. {Title}");
        }
        public override bool CheckAnswer(string answer)
        {
            return (answer == CorrectAnswer);
        }
        public TrueFalseQuestion(string title, string correctAnswer, int points = 10)
        : base(title, correctAnswer)
        {
            Points = points;
        }
    }

    // Data of the program such as: Questions, Players, etc.
    public class DataManagement
    {

    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Question TrueFalseQuestion = new TrueFalseQuestion("Question 1", "false");
            TrueFalseQuestion.DisplayQuestion(1);

            Console.WriteLine(TrueFalseQuestion.CheckAnswer("true"));
            Console.Read();
        }
    }
}
