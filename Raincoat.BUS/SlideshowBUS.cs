using Raincoat.BE;
using Raincoat.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Raincoat.BUS
{
    public class SlideshowBUS
    {
        private SlideshowDAL slideDAL = new SlideshowDAL();
        public List<SlideshowBE> GetAllSlides(string hostPath)
        {
            DataTable table = slideDAL.GetAllSlides();
            return TableToBE(table, hostPath);
        }

        public int CreateNewSlide(SlideshowBE slide)
        {
            int productId = slideDAL.CreateNewSlide(slide.Name, string.Empty, slide.Order);
            return productId;
        }
        public bool UpdateSlide(SlideshowBE slide)
        {
            int noOfSuccess = slideDAL.UpdateSlide(slide.Id, slide.Name, string.Empty, slide.Order);
            return noOfSuccess > 0 ? true : false;
        }
        public bool DeleteSlide(int slideId)
        {
            int noOfSuccess = slideDAL.DeleteSlide(slideId);
            return noOfSuccess > 0 ? true : false;
        }
        private List<SlideshowBE> TableToBE(DataTable table, string hostPath)
        {
            List<SlideshowBE> returnBE = new List<SlideshowBE>();
            foreach (DataRow row in table.Rows)
            {
                SlideshowBE be = new SlideshowBE();
                be.Id = row.Table.Columns.Contains("Id")
                    ? DataProvider.GetDBInteger(row, "Id")
                    : -1;
                be.Name = row.Table.Columns.Contains("Name")
                    ? DataProvider.GetDBString(row, "Name")
                    : string.Empty;
                be.ImageValue = row.Table.Columns.Contains("ImageValue")
                    ? DataProvider.GetDBString(row, "ImageValue")
                    : string.Empty;
                be.Order = row.Table.Columns.Contains("Order")
                    ? DataProvider.GetDBInteger(row, "Order")
                    : -1;
                be.hostPath = hostPath;
                returnBE.Add(be);
            }
            return returnBE;
        }
    }
}