using System.Collections.Generic;

namespace GroceryCheckOut.Entity.Interfaces
{
    public interface IRepositoryBP
    {
        void UpSert<T>(T t) where T : class;
        List<T> GetAll<T>() where T : class;
    }
}
