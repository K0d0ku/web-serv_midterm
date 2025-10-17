using KuroApi.Models;
using System.Collections.Generic;

namespace KuroApi.Repositories
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAll();
        Customer GetById(int id);
        void Add(Customer customer);
        void Update(Customer customer);
        void Delete(int id);
        bool ExistsByEmail(string email);
    }
}
