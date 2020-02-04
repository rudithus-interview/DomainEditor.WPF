namespace InterviewAssessment.Models
{
    public class EntityAttribute
    {
        public string Name { get; }
        public string Value { get; }

        public EntityAttribute(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
