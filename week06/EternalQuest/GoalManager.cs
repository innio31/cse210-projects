using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EternalQuest
{
    public class GoalManager
    {
        private List<Goal> _goals;
        private int _score;
        private int _level;
        private int _totalPointsEarned;

        public GoalManager()
        {
            _goals = new List<Goal>();
            _score = 0;
            _level = 1;
            _totalPointsEarned = 0;
        }

        public void Start()
        {
            int choice = 0;
            while (choice != 6)
            {
                DisplayPlayerInfo();
                Console.WriteLine("\nMenu Options:");
                Console.WriteLine("1. Create New Goal");
                Console.WriteLine("2. List Goals");
                Console.WriteLine("3. Save Goals");
                Console.WriteLine("4. Load Goals");
                Console.WriteLine("5. Record Event");
                Console.WriteLine("6. Quit");
                Console.Write("Select a choice from the menu: ");

                choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        CreateGoal();
                        break;
                    case 2:
                        ListGoalDetails();
                        break;
                    case 3:
                        SaveGoals();
                        break;
                    case 4:
                        LoadGoals();
                        break;
                    case 5:
                        RecordEvent();
                        break;
                    case 6:
                        Console.WriteLine("Goodbye! Keep pursuing your Eternal Quest!");
                        break;
                }
            }
        }

        public void DisplayPlayerInfo()
        {
            Console.WriteLine($"\n=== Eternal Quest ===");
            Console.WriteLine($"Current Score: {_score} points");
            Console.WriteLine($"Level: {_level} ({(1000 * _level)} points to next level)");
            Console.WriteLine($"Total Goals: {_goals.Count}");
            Console.WriteLine($"Completed Goals: {_goals.Count(g => g.IsComplete())}");
        }

        public void ListGoalNames()
        {
            Console.WriteLine("\nGoal Names:");
            for (int i = 0; i < _goals.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_goals[i].GetName()}");
            }
        }

        public void ListGoalDetails()
        {
            Console.WriteLine("\n=== Goal Details ===");
            if (_goals.Count == 0)
            {
                Console.WriteLine("No goals created yet. Create some goals first!");
                return;
            }

            for (int i = 0; i < _goals.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_goals[i].GetDetailString()}");
            }
        }

        public void CreateGoal()
        {
            Console.WriteLine("\n=== Create New Goal ===");
            Console.WriteLine("Goal Types:");
            Console.WriteLine("1. Simple Goal");
            Console.WriteLine("2. Eternal Goal");
            Console.WriteLine("3. Checklist Goal");
            Console.Write("Which type of goal would you like to create? ");

            int type = int.Parse(Console.ReadLine());

            Console.Write("What is the name of your goal? ");
            string name = Console.ReadLine();

            Console.Write("What is a short description of it? ");
            string description = Console.ReadLine();

            Console.Write("What is the amount of points associated with this goal? ");
            int points = int.Parse(Console.ReadLine());

            switch (type)
            {
                case 1:
                    _goals.Add(new SimpleGoal(name, description, points));
                    break;
                case 2:
                    _goals.Add(new EternalGoal(name, description, points));
                    break;
                case 3:
                    Console.Write("How many times does this goal need to be accomplished for a bonus? ");
                    int target = int.Parse(Console.ReadLine());
                    Console.Write($"What is the bonus for accomplishing it {target} times? ");
                    int bonus = int.Parse(Console.ReadLine());
                    _goals.Add(new ChecklistGoal(name, description, points, target, bonus));
                    break;
            }

            Console.WriteLine($"Goal '{name}' created successfully!");
        }

        public void RecordEvent()
        {
            if (_goals.Count == 0)
            {
                Console.WriteLine("No goals available. Create some goals first!");
                return;
            }

            ListGoalDetails();
            Console.Write("Which goal did you accomplish? ");
            int index = int.Parse(Console.ReadLine()) - 1;

            if (index >= 0 && index < _goals.Count)
            {
                Goal goal = _goals[index];
                int pointsEarned = 0;

                if (!goal.IsComplete())
                {
                    if (goal is SimpleGoal simpleGoal)
                    {
                        pointsEarned = goal.GetPoints();
                        simpleGoal.RecordEvent();
                    }
                    else if (goal is EternalGoal)
                    {
                        pointsEarned = goal.GetPoints();
                        goal.RecordEvent();
                    }
                    else if (goal is ChecklistGoal checklistGoal)
                    {
                        pointsEarned = goal.GetPoints();
                        checklistGoal.RecordEvent();

                        if (checklistGoal.IsComplete())
                        {
                            int bonus = ((ChecklistGoal)goal).GetBonus();
                            pointsEarned += bonus;
                            Console.WriteLine($"🎉 BONUS! You earned an extra {bonus} points for completing the checklist goal! 🎉");
                        }
                    }

                    _score += pointsEarned;
                    _totalPointsEarned += pointsEarned;

                    // Level up system (every 1000 points)
                    int newLevel = (_totalPointsEarned / 1000) + 1;
                    if (newLevel > _level)
                    {
                        _level = newLevel;
                        Console.WriteLine($"✨ LEVEL UP! You are now Level {_level}! ✨");
                    }

                    Console.WriteLine($"\n🎯 You earned {pointsEarned} points!");
                }
                else
                {
                    Console.WriteLine($"\n⚠️ '{goal.GetName()}' is already complete! You cannot earn more points for this goal.");
                }
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        public void SaveGoals()
        {
            Console.Write("Enter filename to save: ");
            string filename = Console.ReadLine();

            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine(_score);
                writer.WriteLine(_totalPointsEarned);
                writer.WriteLine(_level);

                foreach (Goal goal in _goals)
                {
                    writer.WriteLine(goal.GetStringRepresentation());
                }
            }

            Console.WriteLine($"Goals saved to {filename}");
        }

        public void LoadGoals()
        {
            Console.Write("Enter filename to load: ");
            string filename = Console.ReadLine();

            if (!File.Exists(filename))
            {
                Console.WriteLine("File not found.");
                return;
            }

            _goals.Clear();

            using (StreamReader reader = new StreamReader(filename))
            {
                _score = int.Parse(reader.ReadLine());
                _totalPointsEarned = int.Parse(reader.ReadLine());
                _level = int.Parse(reader.ReadLine());

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(':');
                    string type = parts[0];
                    string[] data = parts[1].Split(',');

                    switch (type)
                    {
                        case "SimpleGoal":
                            SimpleGoal simple = new SimpleGoal(data[0], data[1], int.Parse(data[2]));
                            if (bool.Parse(data[3]))
                            {
                                simple.RecordEvent();
                            }
                            _goals.Add(simple);
                            break;
                        case "EternalGoal":
                            _goals.Add(new EternalGoal(data[0], data[1], int.Parse(data[2])));
                            break;
                        case "ChecklistGoal":
                            ChecklistGoal checklist = new ChecklistGoal(data[0], data[1], int.Parse(data[2]), int.Parse(data[4]), int.Parse(data[3]));
                            for (int i = 0; i < int.Parse(data[5]); i++)
                            {
                                checklist.RecordEvent();
                            }
                            _goals.Add(checklist);
                            break;
                    }
                }
            }

            Console.WriteLine($"Goals loaded from {filename}");
        }
    }
}