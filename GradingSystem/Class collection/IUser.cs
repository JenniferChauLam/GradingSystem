using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradingSystem.Class_collection
{
    public interface IUser
    {
        string RoleName { get; }

        int Create(
            string username,
            string password,
            string email);
        bool Update(
            string userId,
            string? username = null,
            string? password = null,
            string? email = null);
        void Delete(string userId);
    }
}
