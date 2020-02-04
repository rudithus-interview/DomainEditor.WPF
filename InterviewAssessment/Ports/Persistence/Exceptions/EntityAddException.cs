using System;

namespace InterviewAssessment.Ports.Persistence.Exceptions
{
    public class EntityAddException : Exception
    {
        public EntityAddException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
