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

        public int CreateNewProduct(string name, int price, string imageValue)
        {
            SqlParameter[] paramList = new SqlParameter[3];
            paramList[0] = DataProvider.AddParameters("@Name", SqlDbType.NVarChar, name);
            paramList[1] = DataProvider.AddParameters("@Price", SqlDbType.BigInt, price);
            paramList[2] = DataProvider.AddParameters("@ImageValue", SqlDbType.NVarChar, imageValue);
            int productId = this.ExecuteInsertQueryReturnId(Constants.CREATE_NEW_PRODUCT, paramList);
            return productId;
        }

        public int UpdateProduct(int id, string name, int price, string imageValue)
        {
            SqlParameter[] paramList = new SqlParameter[4];
            paramList[0] = DataProvider.AddParameters("@Id", SqlDbType.Int, id);
            paramList[1] = DataProvider.AddParameters("@Name", SqlDbType.NVarChar, name);
            paramList[2] = DataProvider.AddParameters("@Price", SqlDbType.BigInt, price);
            paramList[3] = DataProvider.AddParameters("@ImageValue", SqlDbType.NVarChar, imageValue);
            int noOfSuccess = this.ExecuteUpdateQuery(Constants.UPDATE_PRODUCT, paramList);
            return noOfSuccess;
        }
    }
}