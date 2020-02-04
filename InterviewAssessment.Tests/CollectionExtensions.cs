using System;
using System.Collections.Generic;

namespace InterviewAssessmentTests
{
    public static class CollectionExtensions
    {
        public static T GetRandomItem<T>(this IList<T> collection)
        {
            var rnd = new Random();
            var i = rnd.Next(collection.Count);
            return collection[i];
        }
    }
}
