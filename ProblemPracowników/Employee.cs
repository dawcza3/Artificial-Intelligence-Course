using System.Collections.Generic;

namespace ProblemPracowników
{
    public class Employee
    {
        public int Id { get; set; }
        public Dictionary<int,double> TaskDictionary { get; set; }

        public Employee(int id,Dictionary<int,double> taskDictionary )
        {
            Id = id;
            TaskDictionary = taskDictionary;
        }  
    }
}