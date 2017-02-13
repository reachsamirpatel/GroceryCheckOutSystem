using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryCheckOutSystem.DataAccess
{
    /// <summary>
    /// Wrapper class for all repository to perform xml related operations
    /// </summary>
    public class Repository
    {
        public string FilePath { get; set; }
        public ProductRepository ProductRepository { get; set; }
        public PromotionRepository PromotionRepository { get; set; }
        public UserRepository UserRepository { get; set; }
        public LoginRepository LoginRepository { get; set; }
        public Repository()
        {
            ProductRepository = new ProductRepository();
            PromotionRepository = new PromotionRepository();
            UserRepository = new UserRepository();
            LoginRepository = new LoginRepository();
        }
    }
}
