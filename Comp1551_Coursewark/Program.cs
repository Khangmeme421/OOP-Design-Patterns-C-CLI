using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Reflection;
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
            MenuItems = new List<string> { "Exit Program", "Manage Questions", "Play" };
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
            int optionIndex = int.Parse(option);
            string returnMenu = MenuItems[optionIndex].Replace(" ", "");
            return returnMenu;
        }
    }
    public class ManageQuestionsMenu : Menu
    {
        public ManageQuestionsMenu()
        {
            MenuItems = new List<string> { "Back", "Create New Question", "View All Questions", "Delete Questions" };
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
            int optionIndex = int.Parse(option);
            string returnMenu = MenuItems[optionIndex].Replace(" ", "");
            return returnMenu;
        }
    }

    public class ViewAllQuestionsMenu : Menu
    {
        public ViewAllQuestionsMenu()
        {
            // Initialize the MenuItems list with "Back"
            MenuItems = new List<string> { "Back" };
        }

        public override string DisplayMenu()
        {
            // Fetch questions from DataManagement and add their titles to the MenuItems
            MenuItems = new List<string> { "Back" };
            var questions = DataManagement.Instance.Questions;
            foreach (var question in questions)
            {
                MenuItems.Add(question.Title + " " + question.QuestionType);
            }

            Console.WriteLine("View All Questions Menu:");
            for (int i = 0; i < MenuItems.Count; i++)
            {
                Console.WriteLine($"{i}. {MenuItems[i]}");
            }
            Console.Write("Enter (0) to get back: ");
            string option = Console.ReadLine();

            // Validate the input option
            while (option != "0")
            {
                Console.Write("Invalid option. Enter (0) to get back: ");
                option = Console.ReadLine();
            }
            DataManagement.Instance.MenuHistory.RemoveAt(DataManagement.Instance.MenuHistory.Count - 1);
            return "Back";
        }
    }

    public class DeleteQuestionsMenu : Menu
    {
        public DeleteQuestionsMenu()
        {
            // Initialize the MenuItems list with "Back"
            MenuItems = new List<string> { "Back" };
        }

        public override string DisplayMenu()
        {
            // Fetch questions from DataManagement and add their titles to the MenuItems
            MenuItems = new List<string> { "Back" };
            var questions = DataManagement.Instance.Questions;
            foreach (var question in questions)
            {
                MenuItems.Add(question.Title + " " + question.QuestionType);
            }

            Console.WriteLine("Delete Questions Menu:");
            for (int i = 0; i < MenuItems.Count; i++)
            {
                Console.WriteLine($"{i}. {MenuItems[i]}");
            }
            Console.Write("Enter question number: ");
            string option = Console.ReadLine();

            // Validate the input option
            int index;
            if (option == "0")
                return "Back";
            while (!int.TryParse(option, out index) || index < 0 || index >= MenuItems.Count)
            {
                Console.Write("Invalid option. Please choose again: ");
                option = Console.ReadLine();
            }
            bool success = DataManagement.Instance.DeleteQuestion(index - 1); // Adjust for "Back" option
            DataManagement.Instance.MenuHistory.RemoveAt(DataManagement.Instance.MenuHistory.Count - 1);
            return "Back";
        }
    }
    public class CreateQuestionMenu : Menu
    {
        public CreateQuestionMenu()
        {
            MenuItems = new List<string> { "Back" }; // The only option initially is to go back
        }

        public override string DisplayMenu()
        {
            List<string> questionTypes = new List<string>()
        {
            "Open-Ended",
            "Multiple Choice",
            "True/False"
        };

            Console.WriteLine("Create New Question:");
            Console.WriteLine("Select question type:");
            Console.WriteLine("0. Back");
            for (int i = 0; i < questionTypes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {questionTypes[i]}");
            }

            string choice = Console.ReadLine();
            int choice_int;
            while (!int.TryParse(choice, out choice_int) || choice_int < 0 || choice_int > 3)
            {
                Console.Write("Invalid choice. Please enter a number between 0 and 3: ");
                choice = Console.ReadLine();
            }

            if (choice_int == 0) // If the user chooses to go back
            {
                return "Back"; // Indicate that the user wants to go back
            }

            Console.Write($"({questionTypes[choice_int - 1]}) Enter question title: ");
            string title = Console.ReadLine();

            Question question = null;

            switch (choice_int)
            {
                case 1:
                    Console.Write("Enter correct answer(s) separated by commas: ");
                    string openEndedAnswer = Console.ReadLine();
                    question = new OpenEndedQuestion(title, openEndedAnswer);
                    break;

                case 2:
                    List<string> options = new List<string>();
                    for (int i = 0; i < 4; i++)
                    {
                        Console.Write($"Enter option {i + 1}: ");
                        options.Add(Console.ReadLine());
                    }
                    int index;
                    do
                    {
                        Console.Write("Enter correct answer character (A-D): ");
                        char correctAnswerChar = char.ToUpper(Console.ReadLine()[0]);
                        index = correctAnswerChar - 'A';
                    }
                    while (index < 0 || index > 3);
                    question = new MultipleChoiceQuestion(title, options[index], options);
                    break;

                case 3:
                    Console.Write("Enter correct answer (True/False): ");
                    string trueFalseAnswer = Console.ReadLine();
                    question = new TrueFalseQuestion(title, trueFalseAnswer);
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    return "Back";
            }

            // Add the created question to the DataManagement instance
            DataManagement.Instance.Questions.Add(question);
            DataManagement.Instance.MenuHistory.RemoveAt(DataManagement.Instance.MenuHistory.Count - 1);
            Console.WriteLine("Question created successfully.");
            Console.ReadLine();
            return "Back"; // After creating a question, go back to the previous menu
        }
    }

    public class PlayMenu : Menu
    {
        public PlayMenu()
        {
            MenuItems = new List<string> { "Back" };
        }
        public override string DisplayMenu()
        {
            Player user = new Player("Player", "Player");
            Console.WriteLine("Play Quiz Menu:");
            if (DataManagement.Instance.Questions.Count == 0)
                Console.WriteLine("No questions available.\nPress Enter to go back");
            else
            {
                DataManagement stopWatch = DataManagement.Instance;
                // Start the stopwatch
                stopWatch.StartStopwatch();
                for (int i = 0; i < DataManagement.Instance.Questions.Count; i++)
                {
                    Question currentQuestion = DataManagement.Instance.Questions[i];
                    currentQuestion.DisplayQuestion(i);
                    string answer = Console.ReadLine();
                    user.AnswerQuestion(currentQuestion, answer);
                    Console.WriteLine("Press Enter to continue");
                    Console.ReadLine();
                }
                // Stop the stopwatch after the loop
                stopWatch.StopStopwatch();
                Console.WriteLine($"Your score: {user.Score}");
                // Get and print the elapsed time
                string elapsedTime = stopWatch.GetElapsedTime();
                Console.WriteLine($"Total time taken: {elapsedTime}");
            }
            Console.ReadLine();
            return "Back";
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
            int optionIndex = int.Parse(option);
            string returnMenu = MenuItems[optionIndex].Replace(" ", "");
            return returnMenu;
        }
    }

    // User class interface
    public abstract class User
    {
        private string _userName;
        private string _role;
        private int _score;
        public abstract int Score { get; set;}
        public abstract string UserName { get; set; }
        public abstract string Role { get; set; }
        public User(string userName, string role)
        {
            UserName = userName;
            Role = role;
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
        public override int Score 
        { 
            get { return _score; } 
            set { _score = value; } 
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
        public void AnswerQuestion(Question question, string answer)
        {
            if (question.CheckAnswer(answer))
            {
                _score += question.Points;
                Console.WriteLine("Correct!");
            }
            else
            {
                Console.WriteLine("Incorrect!");
            } 
        }
    }

    // Question class
    public abstract class Question
    {
        public string Title { get; set; }
        public string CorrectAnswer { get; set; }
        public string QuestionType { get; set; }
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
            Console.WriteLine($"{questionNumber}. {Title}  {QuestionType}");
        }
        public override bool CheckAnswer(string answer)
        {
            return answer.Equals(CorrectAnswer, StringComparison.OrdinalIgnoreCase);
        }
        public TrueFalseQuestion(string title, string correctAnswer, int points = 10)
        : base(title, correctAnswer)
        {
            Points = points;
            QuestionType = "(True/False)";
        }
    }
    public class MultipleChoiceQuestion : Question
    {
        private List<string> answerOptions;

        public MultipleChoiceQuestion(string title, string correctAnswer, List<string> options, int points = 15)
            : base(title, correctAnswer)
        {
            Points = points;
            answerOptions = options;
            QuestionType = "(A,B,C,D)";
        }

        public override bool CheckAnswer(string answer)
        {
            return answer.Equals(CorrectAnswer, StringComparison.OrdinalIgnoreCase);
        }
        public override void DisplayQuestion(int questionNumber)
        {
            Console.WriteLine($"{questionNumber}. {Title} {QuestionType}");
            for (int i = 0, j = 'A'; j <= 'D'; i++, j++)
            {
                Console.WriteLine($"{Convert.ToChar(j)}. {answerOptions[i]}");
            }
        }
    }

    public class OpenEndedQuestion : Question
    {
        public OpenEndedQuestion(string title, string correctAnswer) : base(title, correctAnswer) 
        {
            Points = 20;
            QuestionType = "(Open-ended)";
        }

        public override void DisplayQuestion(int questionNumber)
        {
            Console.WriteLine($"{questionNumber}. {Title} {QuestionType}");
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
            List<string> questionTypes;     //Temporary list of question types
            questionTypes = new List<string>()
            {
                "Open-Ended",
                "Multiple Choice",
                "True/False"
            };
            Console.WriteLine($"Select question type: \n0. Back \n1. {questionTypes[0]} \n2. {questionTypes[1]} \n3. {questionTypes[2]}");
            string choice = Console.ReadLine();
            int choice_int; // temporary choice_int storages the choice as integer
            while (!int.TryParse(choice, out choice_int) || choice_int < 0 || choice_int > 3)
            {
                Console.Write("Invalid choice. Please enter a number between 0 and 3: ");
                choice = Console.ReadLine();
            }

            Console.Write($"({questionTypes[choice_int-1]}) Enter question title: ");
            string title = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter correct answer(s) separated by commas: ");
                    string openEndedAnswer = Console.ReadLine();
                    return new OpenEndedQuestion(title, openEndedAnswer);

                case "2":
                    List<string> options = new List<string>();
                    for (int i = 0; i < 4; i++)
                    {
                        Console.Write($"Enter option {i + 1}: ");
                        options.Add(Console.ReadLine());
                    }
                    int index;
                    do
                    {
                        Console.Write("Enter correct answer character (A-D): ");
                        char correctAnswerChar = char.ToUpper(Console.ReadLine()[0]); // Ensure it's uppercase
                        index = correctAnswerChar - 'A'; // Convert character to index (A=0, B=1, C=2, D=3)

                    }
                    while (index < 0 ||  index > 3);
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

    public class MenuFactory
    {
        public static Menu CreateMenu(string menuType)
        {
            switch (menuType)
            {
                case "MainMenu":
                    return new MainMenu();
                case "ManageQuestions":
                    return new ManageQuestionsMenu();
                case "ViewAllQuestions":
                    return new ViewAllQuestionsMenu();
                case "DeleteQuestions":
                    return new DeleteQuestionsMenu();
                case "Play":
                    return new PlayMenu();
                case "CreateNewQuestion":
                    return new CreateQuestionMenu();
                case "Back":
                    DataManagement.Instance.MenuHistory.RemoveAt(DataManagement.Instance.MenuHistory.Count - 1);
                    return DataManagement.Instance.MenuHistory[DataManagement.Instance.MenuHistory.Count - 1];
                default:
                    return null;
            }
        }
    }
    public class DataManagement
    {
        private static DataManagement _instance;
        private static readonly object _lock = new object();

        public List<Question> Questions { get; private set; } = new List<Question>();
        public List<Menu> MenuHistory { get; private set; } = new List<Menu>();
        private Stopwatch _stopwatch = new Stopwatch();
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
        public void AddMenuToHistory(Menu menu)
        {
            MenuHistory.Add(menu);
        }
        public bool DeleteQuestion(int index)
        {
            // Validate the index
            if (index < 0 || index >= Questions.Count)
            {
                Console.WriteLine("Invalid index. Unable to delete the question.");
                return false; // Indicate failure
            }
            // Remove the question at the specified index
            Questions.RemoveAt(index);
            Console.WriteLine($"Question at index {index} has been deleted.");
            return true; // Indicate success
        }
        // Method to start the stopwatch
        public void StartStopwatch()
        {
            _stopwatch.Reset();
            _stopwatch.Start();
        }
        // Method to stop the stopwatch
        public void StopStopwatch()
        {
            _stopwatch.Stop();
        }
        // Method to get formatted elapsed time
        public string GetElapsedTime()
        {
            TimeSpan elapsed = _stopwatch.Elapsed;
            return string.Format("{0:D2}:{1:D2}", (int)elapsed.TotalMinutes, elapsed.Seconds);
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Menu menu = MenuFactory.CreateMenu("MainMenu");
            EndlessMenu(menu);
            Console.ReadLine();
        }
        static void EndlessMenu(Menu menu)
        {
            string option = menu.DisplayMenu();
            if (option == "ExitProgram")
            {
                return;
            }
            Console.Clear();
            DataManagement.Instance.AddMenuToHistory(menu);
            EndlessMenu(MenuFactory.CreateMenu(option));
        }
    }
}
