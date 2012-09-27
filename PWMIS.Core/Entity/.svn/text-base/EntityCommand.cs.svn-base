using System;
using System.Collections.Generic;
using System.Text;
using PWMIS.DataProvider.Data;
using System.Data ;

namespace PWMIS.DataMap.Entity
{
    /// <summary>
    /// 实体类命令，将实体类转换成合适的SQL更新语句和参数
    /// </summary>
    public class EntityCommand
    {
        private EntityBase currEntity;
        private CommonDB currDb;
        List<IDataParameter> _insertParas;
        List<IDataParameter> _updateParas;
        List<IDataParameter> _deleteParas;

        /// <summary>
        /// 插入数据的时候是否插入自增列，默认否
        /// </summary>
        public bool IdentityEnable = false;

        public EntityCommand(EntityBase entity, CommonDB db)
        {
            this.currEntity = entity;
            this.currDb = db;
        }

        private string[] _targetFields;
 
        /// <summary>
        /// 要操作的目标表的所有字段名
        /// </summary>
        public string[] TargetFields {
            get {
                if (_targetFields == null || _targetFields.Length == 0)
                    _targetFields = this.currEntity.PropertyNames;
                return _targetFields;
            }
            set {
                _targetFields = value;
            }
        }

        #region 命令属性

        private  string _insertCommand;
        public string InsertCommand
        {
            get {
                if (_insertCommand == null)
                {
                    _insertParas = new List<IDataParameter>();



                    _insertCommand = "INSERT INTO [" + this.currEntity.TableName +"] ";
                    string fields = "";
                    string values = "";
                   

                    List<string> currFields = new List<string>();
                    if (this.IdentityEnable)
                    {
                        currFields.AddRange(this.TargetFields);
                    }
                    else
                    { 
                        foreach (string field in this.TargetFields)
                        {
                            if (this.currEntity.IdentityName != field)
                                currFields.Add(field);
                        }
                    }

                    foreach (string field in currFields)
                    {
                        fields += ",[" + field+"]";
                        string paraName = "@" + field.Replace (" ","");
                        values += "," + paraName;
                        IDataParameter para = this.currDb.GetParameter(paraName, this.currEntity.PropertyList(field));
                        para.SourceColumn = field;
                        _insertParas.Add (para );
                    }
                    _insertCommand = _insertCommand + "(" + fields.TrimStart(',') + ") VALUES (" + values.TrimStart(',') + ")";

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
                    if (this.currEntity.PrimaryKeys.Count == 0)
                        throw new Exception("EntityCommand Error:实体类没有指定主键，无法生成Update语句。");

                    _updateParas = new List<IDataParameter>();

                    _updateCommand = "UPDATE [" + this.currEntity.TableName + "] SET ";
                    string values = "";
                    string condition = "";
                  
                    foreach (string field in this.TargetFields )
                    {
                        string paraName = "@" + field.Replace (" ","");
                        if (this.currEntity.PrimaryKeys.Contains(field))
                        {
                            //当前字段为主键，不能被更新
                            condition += " AND [" + field + "] = " + paraName;
                        }
                        else
                        {
                            values += ",[" + field + "] = " + paraName;
                        }
                        IDataParameter para = this.currDb.GetParameter(paraName, this.currEntity.PropertyList(field));
                        para.SourceColumn = field;
                        _updateParas.Add(para);
                      
                    }


                    _updateCommand = _updateCommand + values.TrimStart(',') + " WHERE " + condition.Substring(" AND ".Length);
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
                    if (this.currEntity.PrimaryKeys.Count == 0)
                        throw new Exception("EntityCommand Error:实体类没有指定主键，无法生成Delete语句。");

                    _deleteParas = new List<IDataParameter>();

                    _deleteCommand="DELETE FROM [" + this.currEntity.TableName + "] WHERE ";
                    string condition = "";
                    
                    foreach (string key in this.currEntity.PrimaryKeys)
                    {
                        string paraName = "@P" + key.Replace (" ","");
                        condition += " AND [" + key + "]=" + paraName;
                        IDataParameter para = this.currDb.GetParameter(paraName, this.currEntity.PropertyList(key));
                        para.SourceColumn = key;
                        this._deleteParas.Add(para);
                        
                    }
                    _deleteCommand = _deleteCommand + " " + condition.Substring(" AND ".Length);
                }
                return _deleteCommand;
            }
            private set { _deleteCommand = value; }
        }

        #endregion


        #region 参数
        public IDataParameter[] InsertParameters
        {
            get {
                if (_insertParas != null)
                    return _insertParas.ToArray();
                else
                    return null;
            }
        }

        public IDataParameter[] UpdateParameters
        {
            get
            {
                if (_updateParas != null)
                    return _updateParas.ToArray();
                else
                    return null;
            }
        }

        public IDataParameter[] DeleteParameters
        {
            get
            {
                if (_deleteParas != null)
                    return _deleteParas.ToArray();
                else
                    return null;
            }
        }

        #endregion
    }
}
