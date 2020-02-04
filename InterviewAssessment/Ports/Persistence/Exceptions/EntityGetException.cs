using System;

namespace InterviewAssessment.Ports.Persistence.Exceptions
{
    public class EntityGetException : Exception
    {
        public EntityGetException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
