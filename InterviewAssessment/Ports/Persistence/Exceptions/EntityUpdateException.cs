using System;

namespace InterviewAssessment.Ports.Persistence.Exceptions
{
    public class EntityUpdateException : Exception
    {
        public EntityUpdateException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
