using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardStorageService.DataBase.Repository
{
    public interface IRepository<Type, Id>
    {
        IList<Type> GetAll();

        Type GetById(Id id);

        int Create(Type data);

        int Update(Type data);

        int Delete(Id id);
    }
}
