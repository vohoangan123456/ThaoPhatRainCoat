namespace Raincoat.DAL
{
    public class Constants
    {
        public const string CONNECTION_NAME = "ThaoPhat_RainCoat";

        // Log query
        public const string EXECUTE_SELECT_QUERY = "ExecuteSelectQuery";
        public const string EXECUTE_INSERT_QUERY = "ExecuteInsertQuery";
        public const string EXECUTE_UPDATE_QUERY = "ExecuteUpdateQuery";
        public const string EXECUTE_DELETE_QUERY = "ExecuteDeleteQuery";

        // Product procedures
        public const string GET_ALL_PRODUCTS = "GetAllProducts";
        public const string GET_PRODUCT_BY_CONDITION = "GetProductByCondition";
        public const string CREATE_NEW_PRODUCT = "CreateNewProduct";
        public const string UPDATE_PRODUCT = "UpdateProduct";
        public const string DELETE_PRODUCT = "DeleteProduct";
        //slide
        public const string GET_ALL_SLIDES = "GetAllSlides";
        public const string CREATE_NEW_SLIDE = "CreateNewSlide";
        public const string UPDATE_SLIDE = "UpdateSlide";
        public const string DELETE_SLIDE = "DeleteSlide";
    }
}