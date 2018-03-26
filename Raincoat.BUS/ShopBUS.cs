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
        public List<ProductsBE> GetAllProducts(string hostPath)
        {
            DataTable table = shopDAL.GetAllProducts();
            return TableToBE(table, hostPath);
        }
        public List<ProductsBE> GetProductByCondition(string searchKey, int orderBy, string hostPath)
        {
            DataTable table = shopDAL.GetProductByCondition(searchKey, orderBy);
            return TableToBE(table, hostPath);
        }

        public int CreateNewProduct(ProductsBE product)
        {
            int productId = shopDAL.CreateNewProduct(product.Name, product.Price, string.Empty);
            return productId;
        }
        public bool UpdateProduct(ProductsBE product)
        {
            int noOfSuccess = shopDAL.UpdateProduct(product.Id, product.Name, product.Price, string.Empty);
            return noOfSuccess > 0 ? true : false;
        }
        public bool DeleteProduct(int productId)
        {
            int noOfSuccess = shopDAL.DeleteProduct(productId);
            return noOfSuccess > 0 ? true : false;
        }
        private List<ProductsBE> TableToBE(DataTable table, string hostPath)
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
                be.ImageValue = row.Table.Columns.Contains("ImageValue")
                    ? DataProvider.GetDBString(row, "ImageValue")
                    : string.Empty;
                be.hostPath = hostPath;
                returnBE.Add(be);
            }
            return returnBE;
        }
    }
}