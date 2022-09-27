using CardStorageService.DataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CardStorageService.DataBase.Repository.Impl
{
    public class ClientRepository : IRepository<Client, int>
    {
        private readonly CardStorageServiceDbContext _context;
        public ClientRepository(CardStorageServiceDbContext context)
        {
            _context = context;
        }

        public int Create(Client data)
        {
            _context.Set<Client>().Add(data);
            int res = _context.SaveChanges();

            return res;
        }

        public int Delete(int id)
        {
            var element = _context.Clients.Single(x => x.ClientId == id);
            _context.Remove(element);
            int res = _context.SaveChanges();

            return res;
        }

        public IList<Client> GetAll()
        {
            return _context.Clients.ToList();
        }

        public Client GetById(int id)
        {
            return _context.Clients.Single(x => x.ClientId == id);
        }

        public int Update(Client data)
        {
            var element = _context.Clients.Single(x => x.ClientId == data.ClientId);
            element.ClientId = data.ClientId;
            element.Surname = data.Surname;
            element.FirstName = data.FirstName;
            element.Patronymic = data.Patronymic;
            int res = _context.SaveChanges();

            return res;
        }
    }
}
