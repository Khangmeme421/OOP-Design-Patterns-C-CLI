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
            MenuItems = new List<string> { "Exit program", "Manage Questions", "Play" };
        }
        public override string DisplayMenu()
        {
            Console.WriteLine("Main Menu:");
            for (int i = 0; i < MenuItems.Count; i++)
            {
                Console.WriteLine($"{i}. {MenuItems[i]}");
            }
            Console.Write("Choose an option (0-2): ");
            string option = Console.ReadLine();
            List<string> invalidOptions = new List<string> { "0", "1", "2" };
            while (!invalidOptions.Contains(option))
            {
                Console.Write("Invalid option. Please choose again: ");
                option = Console.ReadLine();
            }
            return option;
        }
    }
    public class ManageQuestionsMenu : Menu
    {
        public ManageQuestionsMenu()
        {
            MenuItems = new List<string> { "Back", "Create new question", "View all questions", "Delete question" };
        }
        public override string DisplayMenu()
        {
            Console.WriteLine("Manage Questions Menu:");
            for (int i = 0; i < MenuItems.Count; i++)
            {
                Console.WriteLine($"{i}. {MenuItems[i]}");
            }
            Console.Write("Choose an option (0-3): ");
            string option = Console.ReadLine();
            List<string> invalidOptions = new List<string> { "0", "1", "2", "3" };
            while (!invalidOptions.Contains(option))
            {
                Console.Write("Invalid option. Please choose again: ");
                option = Console.ReadLine();
            }
            return option;
        }
    }
    public class CreateQuestionMenu : Menu
    {
        public CreateQuestionMenu()
        {
            MenuItems = new List<string> { "Back", "Open ended question", "Multiple choice question", "True or false question" };
        }
        public override string DisplayMenu()
        {
            Console.WriteLine("Manage Questions Menu:");
            for (int i = 0; i < MenuItems.Count; i++)
            {
                Console.WriteLine($"{i}. {MenuItems[i]}");
            }
            Console.Write("Choose a question type (0-3): ");
            string option = Console.ReadLine();
            List<string> invalidOptions = new List<string> { "0", "1", "2", "3" };
            while (!invalidOptions.Contains(option))
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
            List<string> invalidOptions = new List<string> { "0", "1", "2", "3" };
            while (!invalidOptions.Contains(option))
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
            Console.WriteLine($"{questionNumber}. {Title} (True/False)");
        }
        public override bool CheckAnswer(string answer)
        {
            return answer.Equals(CorrectAnswer, StringComparison.OrdinalIgnoreCase);
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
            return answer.Equals(CorrectAnswer, StringComparison.OrdinalIgnoreCase);
        }
        public override void DisplayQuestion(int questionNumber)
        {
            Console.WriteLine($"{questionNumber}. {Title} (A,B,C,D)");
            for (int i = 0; i < answerOptions.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {answerOptions[i]}");
            }
        }
    }

    public class OpenEndedQuestion : Question
    {
        public OpenEndedQuestion(string title, string correctAnswer) : base(title, correctAnswer) { }

        public override void DisplayQuestion(int questionNumber)
        {
            Console.WriteLine($"{questionNumber}. {Title} (Open-ended)");
        }
        public override bool CheckAnswer(string answer)
        {
            return CorrectAnswer.Split(',').Any(a => a.Trim().Equals(answer.Trim(), StringComparison.OrdinalIgnoreCase));
        }
    }
    public class QuestionFactory
    {
        public static Question CreateQuestion()
        {
            Console.WriteLine("Select question type: 1. Open-ended 2. Multiple choice 3. True/False");
            string choice = Console.ReadLine();

            Console.Write("Enter question title: ");
            string title = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter correct answer(s) separated by commas: ");
                    string openEndedAnswer = Console.ReadLine();
                    return new OpenEndedQuestion(title, openEndedAnswer);

                case "2":
                    Console.Write("Enter correct answer index (1-4): ");
                    int index = int.Parse(Console.ReadLine()) - 1;
                    List<string> options = new List<string>();
                    for (int i = 0; i < 4; i++)
                    {
                        Console.Write($"Enter option {i + 1}: ");
                        options.Add(Console.ReadLine());
                    }
                    return new MultipleChoiceQuestion(title, options[index], options);

                case "3":
                    Console.Write("Enter correct answer (True/False): ");
                    string trueFalseAnswer = Console.ReadLine();
                    return new TrueFalseQuestion(title, trueFalseAnswer);

                default:
                    Console.WriteLine("Invalid choice.");
                    return null;
            }
        }
    }
    /*
    // Data of the program such as: Questions, Players, etc.
    public class DataManagement
    {
        public static List<User> Users { get; set; } = new List<User>();
        public static List<Question> Questions { get; set; } = new List<Question>();
        public static List<Menu> Menus { get; set; } = new List<Menu>();
    }
    */
    public class DataManagement
    {
        private static DataManagement _instance;
        private static readonly object _lock = new object();

        public List<Question> Questions { get; private set; } = new List<Question>();

        private DataManagement() { }

        public static DataManagement Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new DataManagement();
                    }
                    return _instance;
                }
            }
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("1. Create a new question");
                Console.WriteLine("2. Exit");
                string option = Console.ReadLine();

                if (option == "1")
                {
                    Question question = QuestionFactory.CreateQuestion();
                    if (question != null)
                    {
                        DataManagement.Instance.Questions.Add(question);
                        Console.WriteLine("Question added successfully!");
                    }
                }
                else if (option == "2")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please try again.");
                }
            }
            Console.WriteLine("All questions below: ");
            for (int i = 0; i < DataManagement.Instance.Questions.Count; i++)
            {
                DataManagement.Instance.Questions[i].DisplayQuestion(i + 1);
            }
        }
    }
}
