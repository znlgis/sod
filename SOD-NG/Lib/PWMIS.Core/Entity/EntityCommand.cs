/*
 * ========================================================================
 * Copyright(c) 2006-2010 PWMIS, All Rights Reserved.
 * Welcom use the PDF.NET (PWMIS Data Process Framework).
 * See more information,Please goto http://www.pwmis.com/sqlmap 
 * ========================================================================
 * 该类的作用实体类命令，将实体类转换成合适的SQL更新语句和参数
 * 
 * 作者：邓太华     时间：2008-10-12
 * 版本：V4.5
 * 
 * 修改者：         时间：2012-01-13                
 * 修改说明：增加根据实体类，生成建表脚本的功能，例如下面的例子：
            EntityCommand ecmd=new EntityCommand (new LT_Users(),new SqlServer());
            Console.WriteLine(ecmd.CreateTableCommand);

 * 
 * ========================================================================
*/

using System;
using System.Collections.Generic;
using System.Data;
using PWMIS.DataProvider.Data;

namespace PWMIS.DataMap.Entity
{
    /// <summary>
    ///     实体类命令，将实体类转换成合适的SQL更新语句和参数
    /// </summary>
    public class EntityCommand
    {
        private readonly CommonDB currDb;
        private readonly EntityBase currEntity;
        private readonly string currTableName = string.Empty;
        private List<IDataParameter> _deleteParas;
        private List<IDataParameter> _insertParas;
        private string[] _targetFields;
        private List<IDataParameter> _updateParas;

        /// <summary>
        ///     插入数据的时候是否插入自增列，默认否
        /// </summary>
        public bool IdentityEnable = false;

        public EntityCommand(EntityBase entity, CommonDB db)
        {
            currEntity = entity;
            currDb = db;
            currTableName = entity.TableName;
        }

        /// <summary>
        ///     要操作的目标表的所有字段名
        /// </summary>
        public string[] TargetFields
        {
            get
            {
                if (_targetFields == null || _targetFields.Length == 0)
                    _targetFields = currEntity.PropertyNames;
                return _targetFields;
            }
            set { _targetFields = value; }
        }

        #region 命令属性

        private string _insertCommand;

        public string InsertCommand
        {
            get
            {
                if (_insertCommand == null)
                {
                    _insertParas = new List<IDataParameter>();


                    _insertCommand = "INSERT INTO [" + currTableName + "] ";
                    var fields = "";
                    var values = "";


                    var currFields = new List<string>();
                    if (IdentityEnable)
                    {
                        currFields.AddRange(TargetFields);
                    }
                    else
                    {
                        var identityName = currEntity.IdentityName;
                        foreach (var field in TargetFields)
                        {
                            if (identityName != field)
                                currFields.Add(field);
                        }
                    }

                    foreach (var field in currFields)
                    {
                        fields += ",[" + field + "]";
                        var paraName = "@" + field.Replace(" ", "");
                        values += "," + paraName;
                        var para = currDb.GetParameter(paraName, currEntity.PropertyList(field));
                        para.SourceColumn = field;
                        _insertParas.Add(para);
                    }
                    _insertCommand = _insertCommand + "(" + fields.TrimStart(',') + ") VALUES (" + values.TrimStart(',') +
                                     ")";
                }
                return _insertCommand;
            }
            private set { _insertCommand = value; }
        }

        private string _updateCommand;

        public string UpdateCommand
        {
            get
            {
                if (_updateCommand == null)
                {
                    if (currEntity.PrimaryKeys.Count == 0)
                        throw new Exception("EntityCommand Error:实体类没有指定主键，无法生成Update语句。");

                    _updateParas = new List<IDataParameter>();

                    _updateCommand = "UPDATE [" + currTableName + "] SET ";
                    var values = "";
                    var condition = "";

                    foreach (var field in TargetFields)
                    {
                        var paraName = "@" + field.Replace(" ", "");
                        if (currEntity.PrimaryKeys.Contains(field))
                        {
                            //当前字段为主键，不能被更新
                            condition += " AND [" + field + "] = " + paraName;
                        }
                        else
                        {
                            values += ",[" + field + "] = " + paraName;
                        }
                        var para = currDb.GetParameter(paraName, currEntity.PropertyList(field));
                        para.SourceColumn = field;
                        _updateParas.Add(para);
                    }


                    _updateCommand = _updateCommand + values.TrimStart(',') + " WHERE " +
                                     condition.Substring(" AND ".Length);
                }
                return _updateCommand;
            }
            private set { _updateCommand = value; }
        }

        private string _deleteCommand;

        public string DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    if (currEntity.PrimaryKeys.Count == 0)
                        throw new Exception("EntityCommand Error:实体类没有指定主键，无法生成Delete语句。");

                    _deleteParas = new List<IDataParameter>();

                    _deleteCommand = "DELETE FROM [" + currTableName + "] WHERE ";
                    var condition = "";

                    foreach (var key in currEntity.PrimaryKeys)
                    {
                        var paraName = "@P" + key.Replace(" ", "");
                        condition += " AND [" + key + "]=" + paraName;
                        var para = currDb.GetParameter(paraName, currEntity.PropertyList(key));
                        para.SourceColumn = key;
                        _deleteParas.Add(para);
                    }
                    _deleteCommand = _deleteCommand + " " + condition.Substring(" AND ".Length);
                }
                return _deleteCommand;
            }
            private set { _deleteCommand = value; }
        }

        private string _createTableCommand;

        /// <summary>
        ///     获取创建表的命令脚本
        /// </summary>
        public string CreateTableCommand
        {
            get
            {
                if (_createTableCommand == null)
                {
                    var script = @"
CREATE TABLE @TABLENAME(
@FIELDS
)
                    ";
                    var entityFields = EntityFieldsCache.Item(currEntity.GetType());
                    var fieldsText = "";
                    foreach (var field in currEntity.PropertyNames)
                    {
                        var columnScript = entityFields.CreateTableColumnScript(currDb as AdoHelper, currEntity, field);
                        fieldsText = fieldsText + "," + columnScript + "\r\n";
                    }
                    var tableName = currDb.GetPreparedSQL("[" + currTableName + "]");
                    _createTableCommand = script.Replace("@TABLENAME", tableName)
                        .Replace("@FIELDS", fieldsText.Substring(1));
                }
                return _createTableCommand;
            }
        }

        #endregion

        #region 参数

        public IDataParameter[] InsertParameters
        {
            get
            {
                if (_insertParas != null)
                    return _insertParas.ToArray();
                return null;
            }
        }

        public IDataParameter[] UpdateParameters
        {
            get
            {
                if (_updateParas != null)
                    return _updateParas.ToArray();
                return null;
            }
        }

        public IDataParameter[] DeleteParameters
        {
            get
            {
                if (_deleteParas != null)
                    return _deleteParas.ToArray();
                return null;
            }
        }

        #endregion
    }
}