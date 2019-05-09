using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace SqlServer_dll
{
    public class DbConnection
    {
        public SqlConnection conn = null;
        SqlTransaction trans = null;
        public string SConn = "";

        public DbConnection(string connStr)
        {
            try
            {
                SConn = connStr + ";MultipleActiveResultSets = true";
                conn = new SqlConnection(SConn);
            }
            catch (Exception ex)
            {
                throw ex;
            }   
        }

        public void DbOpen()
        {
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 断开连接
        /// </summary>
        public void DbClose()
        { 
            this.conn.Close();
        }

        public void DbDispose()
        {
            this.conn.Dispose();
        }


        /// <summary>
        /// 返回SqlConnection对象
        /// </summary>
        /// <returns></returns>
        public SqlConnection GetConnection()
        {
            return conn;
        }

        public void BeginTrans()
        {
            try
            {
                trans = conn.BeginTransaction();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CommitTrans()
        {
            try
            {
                if(trans != null)trans.Commit();
                trans = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        public SqlTransaction GetTrans()
        {
            return this.trans;
        }

        public void RollTrans(){
            try
            {
                if(trans != null)trans.Rollback();
                trans = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }   
        }
    }


}
