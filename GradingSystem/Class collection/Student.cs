using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradingSystem.Class_collection
{
    public class Student : BaseUser
    {
        public Student(string connectionString) : base(connectionString, "student") { }
    }
}
