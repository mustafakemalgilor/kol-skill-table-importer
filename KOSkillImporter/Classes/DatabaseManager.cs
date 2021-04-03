/**
 * ______________________________________________________
 * This file is part of ko-skill-table-importer project.
 * 
 * @author       Mustafa Kemal Gılor <mustafagilor@gmail.com> (2016)
 * .
 * SPDX-License-Identifier:	MIT
 * ______________________________________________________
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace KOSkillImporter.Classes
{
    public struct SqlQuery
    {
        public string CommandText;
        public List<SqlParameter> Parameters;
    }



    internal class DatabaseManager
    {
        private SqlConnection _mainConnection;
        public delegate void ExceptionDelegate(Exception ex);

        public ExceptionDelegate OnException;
        /// <summary>
        /// Returns true if connection established.
        /// </summary>
        /// <returns></returns>
        internal bool IsConnected()
        {
            switch (_mainConnection.State)
            {
                case ConnectionState.Open:
                case ConnectionState.Fetching:
                case ConnectionState.Executing:
                    return true;
                default:
                    return false;
            }
        }

        public bool TestConnection(SqlConnectionStringBuilder sqlConnString)
        {
            _mainConnection = new SqlConnection
                                  {
                                      ConnectionString = sqlConnString.ConnectionString
                                  };
            try
            {
                _mainConnection.Open();
                switch (_mainConnection.State)
                {
                    case ConnectionState.Open:
                    case ConnectionState.Fetching:
                    case ConnectionState.Executing:
                        _mainConnection.Close();
                        _mainConnection.Dispose();
                        return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        public void Connect(SqlConnectionStringBuilder sqlConnString, SqlInfoMessageEventHandler _mainConnection_InfoMessage)
        {
            _mainConnection = new SqlConnection
                                  {
                                      ConnectionString = sqlConnString.ConnectionString
                                  };
            try
            {
                _mainConnection.Open();
            }
            catch
            {
                // myParent.Log.DebugException ("Connect error", ex);
                // MessageInterface.ThrowException (ex);

            }

            _mainConnection.InfoMessage +=_mainConnection_InfoMessage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connData"></param>
        public DatabaseManager()
        {


        }



         internal bool ExecuteQuery (SqlQuery query)
             {
             try
                 {
                 using ( var command = new SqlCommand () )
                     {
                     command.Connection = _mainConnection;
                     command.CommandText = query.CommandText;

                     if ( query.Parameters != null )
                         foreach ( var sp in query.Parameters )
                             {
                             command.Parameters.Add (sp);
                             }
                     return command.ExecuteNonQuery () > 0;
                     }
                 }
             catch (Exception ex)
                 {
                     if (OnException != null)
                     {
                         OnException.Invoke(ex);
                       
                     }
                     return false;
                 }

             }
           

        
    }
}
                                       
    
