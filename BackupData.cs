using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SqlServer_dll
{
    public class BackupData
    {
        private SqlConnection conn;

        public BackupData(DbConnection dbc)
        {
            this.conn = dbc.conn;
        }

        /// <summary>
        /// 备份数据库
        /// </summary>
        /// <param name="databaseName">要备份的数据源名称</param>
        /// <param name="backuptoDatabase">备份到的数据库文件名称及路径</param>
        /// <returns></returns>
        public bool BackUpDataBase(string databaseName, string backuptoDatabase)
        {
            string procname;
            string sql;

            //删除逻辑备份设备，但不会删掉备份的数据库文件
            procname = "sp_dropdevice";

            SqlCommand sqlcmd1 = new SqlCommand(procname, conn);
            sqlcmd1.CommandType = CommandType.StoredProcedure;
            SqlParameter sqlpar = new SqlParameter();
            sqlpar = sqlcmd1.Parameters.Add("@logicalname", SqlDbType.VarChar, 20);
            sqlpar.Direction = ParameterDirection.Input;
            sqlpar.Value = databaseName;
            try        //如果逻辑设备不存在，略去错误
            {
                sqlcmd1.ExecuteNonQuery();
            }
            catch
            {

            }

            //创建逻辑备份设备
            procname = "sp_addumpdevice";
            SqlCommand sqlcmd2 = new SqlCommand(procname, conn);
            sqlcmd2.CommandType = CommandType.StoredProcedure;
            sqlpar = sqlcmd2.Parameters.Add("@devtype", SqlDbType.VarChar, 20);
            sqlpar.Direction = ParameterDirection.Input;
            sqlpar.Value = "disk";

            sqlpar = sqlcmd2.Parameters.Add("@logicalname", SqlDbType.VarChar, 20);//逻辑设备名
            sqlpar.Direction = ParameterDirection.Input;
            sqlpar.Value = databaseName;
            sqlpar = sqlcmd2.Parameters.Add("@physicalname", SqlDbType.NVarChar, 260);//物理设备名
            sqlpar.Direction = ParameterDirection.Input;
            sqlpar.Value = backuptoDatabase;

            try
            {
                int i = sqlcmd2.ExecuteNonQuery();
            }
            catch (Exception err)
            {
                string str = err.Message;
            }

            //备份数据库到指定的数据库文件(完全备份)
            sql = "BACKUP DATABASE " + databaseName + " TO " + databaseName + " WITH INIT";
            SqlCommand sqlcmd3 = new SqlCommand(sql, conn);
            sqlcmd3.CommandType = CommandType.Text;
            try
            {
                sqlcmd3.ExecuteNonQuery();
            }
            catch (Exception err)
            {
                string str = err.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 还原指定的数据库文件
        /// </summary>
        /// <param name="databaseName">要还原的数据库</param>
        /// <param name="databaseFile">数据库备份文件及路径</param>
        /// <returns></returns>
        public bool RestoreDataBase(string databaseName, string databaseFile)
        {
            //sql数据库名  
            string dbName = databaseName;
            //创建连接对象  
            SqlConnection conn = this.conn;
            //还原指定的数据库文件  
            string sql = string.Format("use master ;declare @s varchar(8000);select @s=isnull(@s,'')+' kill '+rtrim(spID) from master..sysprocesses where dbid=db_id('{0}');select @s;exec(@s) ;RESTORE DATABASE {1} FROM DISK = N'{2}' with replace", dbName, dbName, databaseFile);
            SqlCommand sqlcmd = new SqlCommand(sql, conn);
            sqlcmd.CommandType = CommandType.Text;
            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception err)
            {
                string str = err.Message;
                return false;
            }

            return true;
        }
    }
}
