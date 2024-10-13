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
    public class MultipleChoiceQuestion : Question
    {
        private List<string> answerOptions;

        public MultipleChoiceQuestion(string title, string correctAnswer, List<string> options)
            : base(title, correctAnswer)
        {
            answerOptions = options;
        }

        public override bool CheckAnswer(string answer)
        {
            return answer == CorrectAnswer;
        }
        public override void DisplayQuestion(int questionNumber)
        {
            // Create a copy of the answer options and shuffle it
            List<string> shuffledOptions = new List<string>(answerOptions);
            Random random = new Random();
            int n = shuffledOptions.Count;

            // Fisher-Yates shuffle algorithm
            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                // Swap shuffledOptions[i] with the element at random index
                string temp = shuffledOptions[i];
                shuffledOptions[i] = shuffledOptions[j];
                shuffledOptions[j] = temp;
            }

            // Display the title and shuffled options
            Console.WriteLine($"Question {questionNumber}: {Title}");
            for (int i = 0; i < shuffledOptions.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {shuffledOptions[i]}");
            }
        }
    }

    public class OpenEndedQuestion : Question
    {

        public OpenEndedQuestion(string title, string correctAnswer) : base(title, correctAnswer)
        {
            
        }

        public override bool CheckAnswer(string answer)
        {
            return string.Equals(answer, CorrectAnswer, StringComparison.OrdinalIgnoreCase);
        }
        public override void DisplayQuestion(int questionNumber)
        {
            Console.WriteLine($"{questionNumber}. {Title}");
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
            Question OpenQuestion = new OpenEndedQuestion("What is the capital of France?", "Paris");
            OpenQuestion.DisplayQuestion(1);
            Console.Write("Answer: ");
            string answer = Console.ReadLine();
            Console.WriteLine(OpenQuestion.CheckAnswer(answer) ? "Correct" : "Incorrect");
            Console.Read();
        }
    }
}
