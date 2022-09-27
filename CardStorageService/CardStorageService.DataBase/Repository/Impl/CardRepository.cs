using CardStorageService.DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardStorageService.DataBase.Repository.Impl
{
    public class CardRepository : IRepository<Card, Guid>
    {
        private readonly CardStorageServiceDbContext _context;
        public CardRepository(CardStorageServiceDbContext context)
        {
            _context = context;
        }

        public int Create(Card data)
        {
            _context.Set<Card>().Add(data);
            int res = _context.SaveChanges();

            return res;
        }

        public int Delete(Guid id)
        {
            var element = _context.Cards.Single(x => x.CardId == id);
            _context.Remove(element);
            int res = _context.SaveChanges();

            return res;
        }

        public IList<Card> GetAll()
        {
            return _context.Cards.ToList();
        }

        public Card GetById(Guid id)
        {
            return _context.Cards.Single(x => x.CardId == id);
        }

        public int Update(Card data)
        {
            var element = _context.Cards.Single(x => x.CardId == data.CardId);
            element.CardId = data.CardId;
            element.ClientId = data.ClientId;
            element.CardNo = data.CardNo;
            element.Name = data.Name;
            element.CVV2 = data.CVV2;
            element.ExpDate = data.ExpDate;
            int res = _context.SaveChanges();

            return res;
        }
    }
}
