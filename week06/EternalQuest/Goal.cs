namespace EternalQuest
{
    public abstract class Goal
    {
        protected string _shortName;
        protected string _description;
        protected int _points;

        public Goal(string name, string description, int points)
        {
            _shortName = name;
            _description = description;
            _points = points;
        }

        public string GetName() => _shortName;
        public string GetDescription() => _description;
        public int GetPoints() => _points;

        public abstract void RecordEvent();
        public abstract bool IsComplete();
        public abstract string GetStringRepresentation();

        public virtual string GetDetailString()
        {
            string checkBox = IsComplete() ? "[X]" : "[ ]";
            return $"{checkBox} {_shortName} ({_description})";
        }
    }
}