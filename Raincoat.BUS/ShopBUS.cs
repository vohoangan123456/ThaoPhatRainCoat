using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raincoat.DAL;
using Raincoat.BE;
using System.Data;

namespace Raincoat.BUS
{
    public class ShopBUS
    {
        private ShopDAL shopDAL = new ShopDAL();
        public List<ProductsBE> GetAllProducts()
        {
            DataTable table = shopDAL.GetAllProducts();
            return TableToBE(table);
        }

        public int CreateNewProduct(ProductsBE product)
        {
            int productId = shopDAL.CreateNewProduct(product.Name, product.Price);
            return productId;
        }
        public bool UpdateProduct(ProductsBE product)
        {
            int noOfSuccess = shopDAL.UpdateProduct(product.Id, product.Name, product.Price);
            return noOfSuccess > 0 ? true : false;
        }
        private List<ProductsBE> TableToBE(DataTable table)
        {
            List<ProductsBE> returnBE = new List<ProductsBE>();
            foreach(DataRow row in table.Rows)
            {
                ProductsBE be = new ProductsBE();
                be.Id = row.Table.Columns.Contains("Id")
                    ? DataProvider.GetDBInteger(row, "Id")
                    : -1;
                be.Name = row.Table.Columns.Contains("Name")
                    ? DataProvider.GetDBString(row, "Name")
                    : string.Empty;
                be.Description = row.Table.Columns.Contains("Description")
                    ? DataProvider.GetDBString(row, "Description")
                    : string.Empty;
                be.Price = row.Table.Columns.Contains("Price")
                    ? DataProvider.GetDBInteger(row, "Price")
                    : -1;
                be.CreatedDate = row.Table.Columns.Contains("CreatedDate")
                    ? DataProvider.GetDBDateTime(row, "CreatedDate")
                    : new DateTime();
                be.CreatedBy = row.Table.Columns.Contains("CreatedBy")
                    ? DataProvider.GetDBString(row, "CreatedBy")
                    : string.Empty;
                returnBE.Add(be);
            }
            return returnBE;
        }
    }
}