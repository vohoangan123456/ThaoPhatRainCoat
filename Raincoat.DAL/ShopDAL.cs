using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Raincoat.DAL
{
    public class ShopDAL : DataProvider
    {
        public DataTable GetAllProducts()
        {
            SqlParameter[] paramList = new SqlParameter[0];
            DataTable returnTable = this.ExecuteSelectQuery(Constants.GET_ALL_PRODUCTS, paramList);

            return returnTable;
        }
    }
}