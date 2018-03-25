using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Raincoat.DAL
{
    public class SlideshowDAL: DataProvider
    {
        public DataTable GetAllSlides()
        {
            SqlParameter[] paramList = new SqlParameter[0];
            DataTable returnTable = this.ExecuteSelectQuery(Constants.GET_ALL_SLIDES, paramList);

            return returnTable;
        }

        public int CreateNewSlide(string name, string imageValue, int order)
        {
            SqlParameter[] paramList = new SqlParameter[2];
            paramList[0] = DataProvider.AddParameters("@Name", SqlDbType.NVarChar, name);
            paramList[1] = DataProvider.AddParameters("@ImageValue", SqlDbType.NVarChar, imageValue);
            int slideId = this.ExecuteInsertQueryReturnId(Constants.CREATE_NEW_SLIDE, paramList);
            return slideId;
        }

        public int UpdateSlide(int id, string name, string imageValue, int order)
        {
            SqlParameter[] paramList = new SqlParameter[4];
            paramList[0] = DataProvider.AddParameters("@Id", SqlDbType.Int, id);
            paramList[1] = DataProvider.AddParameters("@Name", SqlDbType.NVarChar, name);
            paramList[2] = DataProvider.AddParameters("@ImageValue", SqlDbType.NVarChar, imageValue);
            paramList[3] = DataProvider.AddParameters("@Order", SqlDbType.Int, order);
            int noOfSuccess = this.ExecuteUpdateQuery(Constants.UPDATE_SLIDE, paramList);
            return noOfSuccess;
        }

        public int DeleteSlide(int slideId)
        {
            SqlParameter[] paramList = new SqlParameter[1];
            paramList[0] = DataProvider.AddParameters("@SlideId", SqlDbType.Int, slideId);
            int noOfSuccess = this.ExecuteUpdateQuery(Constants.DELETE_SLIDE, paramList);
            return noOfSuccess;
        }
    }
}