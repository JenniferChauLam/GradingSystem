using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradingSystem.Class_collection
{
    public class Teacher : BaseUser
    {
        public Teacher(string connectionString) : base(connectionString, "teacher") { }

        /* Tạo hàm kế thừa BaseUser...
        public override bool Update(string userId, string? username = null, string? password = null, string? email = null)
        {
            return base.Update(userId, username, password, email);
        }
        */
    }
}
