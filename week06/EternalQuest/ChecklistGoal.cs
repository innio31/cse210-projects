namespace EternalQuest
{
    public class ChecklistGoal : Goal
    {
        private int _amountCompleted;
        private int _target;
        private int _bonus;

        public ChecklistGoal(string name, string description, int points, int target, int bonus)
            : base(name, description, points)
        {
            _target = target;
            _bonus = bonus;
            _amountCompleted = 0;
        }

        public override void RecordEvent()
        {
            if (_amountCompleted < _target)
            {
                _amountCompleted++;
            }
        }

        public override bool IsComplete() => _amountCompleted >= _target;

        public int GetBonus() => _bonus;
        public int GetAmountCompleted() => _amountCompleted;
        public int GetTarget() => _target;

        public override string GetDetailString()
        {
            string checkBox = IsComplete() ? "[X]" : "[ ]";
            return $"{checkBox} {_shortName} ({_description}) -- Currently completed: {_amountCompleted}/{_target}";
        }

        public override string GetStringRepresentation()
        {
            return $"ChecklistGoal:{_shortName},{_description},{_points},{_bonus},{_target},{_amountCompleted}";
        }
    }
}