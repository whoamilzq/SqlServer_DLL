using System;
using System.Data.SqlClient;
using System.Collections;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;

namespace SqlServer_dll
{
    public class DbOperate
    {
        public DbConnection dbcon = null;
        object ojbLock = new object();

        public DbOperate(DbConnection dbcon)
        {
            this.dbcon = dbcon;
        }

        /// <summary>
        /// 返回字段最大长度
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public int GetColumnsLength(string tableName, string columnName)
        {
            int i = 0;

            string sql = string.Format("select character_maximum_length from information_schema.columns where table_name= '{0}' and column_name= '{1}'",tableName,columnName);
            i = GetInt(sql);

            return i;
        }

        /// <summary>
        /// 执行sql语句，没有结果集返回
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Boolean ExcuteSql(String sql)
        {
            Boolean result = false;
            SqlCommand MyCommand = new SqlCommand(sql, dbcon.GetConnection());

            using (MyCommand)
            {
                lock (this.ojbLock)
                {
                    try
                    {
                      
                        MyCommand.ExecuteNonQuery();
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                       
                    }
                }
            }
            return result;
        }


        public void ExcuteSql(string sql, byte[] Binary)
        {
            SqlCommand MyCommand = new SqlCommand(sql, dbcon.GetConnection());
            MyCommand.CommandTimeout = 3600;
            try
            {
                MyCommand.Parameters.Add("@Binary", SqlDbType.Image, Binary.Length);
                MyCommand.Parameters["@Binary"].Value = Binary;
                MyCommand.ExecuteNonQuery();
            }
            catch (SqlException exception)
            {
                throw exception;
            }
            finally
            {
                
            }
        }

        public byte[] GetByte(string sql)
        {
            byte[] buffer = new byte[1];
            SqlCommand MyCommand = new SqlCommand(sql, dbcon.GetConnection());
            try
            {
                buffer = (byte[])MyCommand.ExecuteScalar();
            }
            catch (SqlException exception)
            {
                MessageBox.Show(exception.Message);
            }
            return buffer;
        }

        /// <summary>
        /// 获取结果集，指定结果集中数据表名
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string sql)
        {
            SqlCommand MyCommand = new SqlCommand(sql, dbcon.GetConnection());
            DataSet result = new DataSet();
            using (MyCommand)
            {
                lock (this.ojbLock)
                {
                    try
                    {
                        SqlDataAdapter da = new SqlDataAdapter(MyCommand);
                        da.Fill(result);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                       
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取单行记录的单个字段的值，返回object
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public object GetSingleFldValue(String sql, Record param)
        {
            object result = null;
            if (param != null)
            {
                result = GetSingleFld(sql, param);
            }
            else
            {
                SqlCommand MyCommand = new SqlCommand(sql, dbcon.GetConnection());
                using (MyCommand)
                {
                    lock (this.ojbLock)
                    {
                        try
                        {
                          
                            result = MyCommand.ExecuteScalar();
                        }
                        catch (Exception ex)
                        {
                           
                            throw ex;
                        }
                        finally
                        {
                           
                        }
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// 获取单行记录的单个字段的值，返回object
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public object GetSingleFld(String sql,Record param)
        {
            object result = null;
            SqlCommand MyCommand = new SqlCommand(sql, dbcon.GetConnection());
            using (MyCommand)
            {
                lock (this.ojbLock)
                {
                    try
                    {
                        ArrayList ls = param.getList();
                        for (int i = 0; i < ls.Count; i++)
                        {
                            NameValuePair np = (NameValuePair)ls[i];
                            MyCommand.Parameters.Add(new SqlParameter(np.getName(), np.getO()));
                        }
                        result = MyCommand.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                       
                        throw ex;
                    }
                    finally
                    {
                       
                    }
                }
            }
            return result;
        }

        
        /// <summary>
        /// 执行sql语句，获取单行单字段字符串值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public String GetString(String sql)
        {
            Record param=null;
            String result = "";
            try
            {
                object o = GetSingleFldValue(sql,param);
                if (o != null)
                {
                    result = o.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 执行sql语句，获取单行单字段整形值
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int GetInt(String sql)
        {
            int result = 0;
            try
            {
                object o = GetSingleFldValue(sql, null);
                if ((o != null) && ((o is Int32) || (o is Int16) || (o is Int64) || (o is uint) || (o is UInt16) || (o is UInt32) || (o is UInt64) || (o is decimal)))
                {
                    result = Convert.ToInt32(o);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 执行sql语句，获取单行单字段Double值
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public double GetDouble(String sql)
        {
            double result = 0;
            try
            {
                object o = GetSingleFldValue(sql, null);
                if ((o != null) && ((o is Double) || (o is double) || (o is decimal)))
                {
                    result = Convert.ToDouble(o);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 执行sql语句，获取单行单字段日期时间值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public DateTime GetDateTime(String sql)
        {
            Record param = null;
            DateTime result = DateTime.Now;
            try
            {
                object o = GetSingleFldValue(sql, param);
                if ((o != null) && (o is DateTime))
                { 
                    result = Convert.ToDateTime(o);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 执行sql语句，获取单行单字段日期时间字符串值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="format"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public String GetDateStr(String sql, String format)
        {
            Record param = null;
            String result = "";
            try
            {
                object o = GetSingleFldValue(sql, param);
                if ((o != null) && (o is DateTime))
                {
                    result = ((DateTime)o).ToString(format);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 执行sql语句，获取DateTable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql)
        {
            DataTable dataTable = GetDataSet(sql).Tables[0];

            return dataTable;
        }

        /// <summary>
        /// 使用SqlBulkCopy方式插入数据
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public long SqlBulkCopyInsert(string tableName, DataTable dt)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            DataTable dataTable = dt;

            SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(this.dbcon.SConn);
            sqlBulkCopy.DestinationTableName = tableName;
            sqlBulkCopy.BatchSize = dataTable.Rows.Count;
            SqlConnection sqlConnection = new SqlConnection(this.dbcon.SConn);
            try
            {
                sqlConnection.Open();
                if (dataTable != null && dataTable.Rows.Count != 0)
                {   
                    sqlBulkCopy.WriteToServer(dataTable);
                }
                sqlBulkCopy.Close();
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
    }
}