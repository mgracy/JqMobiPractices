using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Web.Configuration;
using INXService;



namespace BookMeeting2
{
    public class DALService
    {
        public static DALConnectionString getDALConnectionString()
        {
            DALConnectionString dcs = new DALConnectionString();
            dcs.ConnectString = WebConfigurationManager.ConnectionStrings["System.Conn"].ConnectionString;
            dcs.Provider = WebConfigurationManager.ConnectionStrings["System.Conn"].ProviderName;

            return dcs;
        }

        public static DAL getDAL()
        {
            DAL dal = new DAL();
            dal.DALConnectionStringValue = getDALConnectionString();

            return dal;
        }

        public static DataSet ExecuteDataSet(string strSql)
        {
            try
            {
                DAL dal = getDAL();
                DataSet ds = dal.ExecuteDataSet_Conn(strSql);
                dal.Dispose();

                return ds;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public static DataSet ExecuteBatchQuery(string[] strSqls)
        {
            try
            {
                DAL dal = getDAL();
                DataSet ds = dal.ExecuteBatchQuery_Conn(strSqls.ToArray());
                dal.Dispose();

                return ds;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public static int ExecuteNonQuery(string strSql)
        {
            try
            {
                DAL dal = getDAL();
                int result = dal.ExecuteNonQuery_Conn(strSql);
                dal.Dispose();

                return result;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public static int ExecuteBatchNonQuery(string[] strSqls)
        {
            try
            {
                DAL dal = getDAL();
                int result = dal.ExecuteBatchNonQuery_Conn(strSqls.ToArray());
                dal.Dispose();

                return result;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public static int ExecuteNonQuerySp(string strSpName, object[] paras)
        {
            try
            {
                DAL dal = getDAL();
                object[] outs = null;
                int result = dal.ExecuteNonQuerySp_Conn(strSpName, paras, out outs);
                dal.Dispose();

                return result;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}