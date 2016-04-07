/*
 * ========================================================================
 * Copyright(c) 2006-2010 PWMIS, All Rights Reserved.
 * Welcom use the PDF.NET (PWMIS Data Process Framework).
 * See more information,Please goto http://www.pwmis.com/sqlmap 
 * ========================================================================
 * 该类的作用
 * 
 * 作者：邓太华     时间：2008-10-12
 * 版本：V3.0
 * 
 * 修改者：         时间：2010.6.20                
 * 修改说明：SQL-MAP 真正支持存储过程
 * ========================================================================
*/
using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using PWMIS.DataProvider.Data;
using PWMIS.Common;
using PWMIS.DataMap.Entity;

namespace PWMIS.DataMap.SqlMap
{
    /// <summary>
    /// SQL-程序代码映射类
    /// </summary>
    /// <remarks></remarks>
    public class SqlMapper
    {

        #region "局部变量定义"


        private List<string> arrStrParas = new List<string>();
        private List<string> arrStrReplaceText = new List<string>();
        //替换参数数组
        private int _ParasLenth;
        private CommonDB _DataBase = null;
        private string _ParaChar = null;

        //private string _SqlText;
        private string _SqlMapScript;
        //SQL MAP 脚本
        private ParamMapType[] _ParamsMap;
        private string _CommandClassName;
        private string _SqlMapFile;
        //private CommandType _CommandType;
        //private IDataParameter[] _DataParameters;
        private string _ResultMap = string.Empty;
        private enumResultClass _ResultClass;

        #endregion

        #region "公开的属性"
        /// <summary>
        /// SQL变量前导字符
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ParaChar
        {
            get
            {
                if (_ParaChar == null)
                {
                    //if (this.DataBase is PWMIS.DataProvider.Data.Oracle)
                    //{
                    //    _ParaChar = ":";
                    //}
                    //else if (this.DataBase is PWMIS.DataProvider.Data.SqlServer)
                    //{
                    //    _ParaChar = "@";
                    //}
                    //else
                    //{

                    //    _ParaChar = "@";
                    //}
                    _ParaChar = this.DataBase.GetParameterChar;
                }
                return _ParaChar;
            }
            protected internal set { _ParaChar = value; }
        }

        /// <summary>
        /// 结果类型
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public enumResultClass ResultClass
        {
            get { return _ResultClass; }
            set { _ResultClass = value; }
        }

        string _embedAssemblySource = "";
        /// <summary>
        /// 获取或者设置要嵌入编译的程序集名称，格式为 “程序集名称,默认命名空间.文件名.扩展名”
        /// </summary>
        public string EmbedAssemblySource {
            get { return _embedAssemblySource; }
            set {
                if(!string .IsNullOrEmpty (value ))
                    this.SqlMapFile = "@R://"+value ;
                _embedAssemblySource = value;
            } 
        }

        /// <summary>
        /// 结果映射的实体类名称
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ResultMap
        {
            get
            {
                if (this.ResultClass == enumResultClass.EntityObject)
                {
                    return _ResultMap;
                }
                else
                {
                    return "";
                }
            }
            set { _ResultMap = value; }
        }

        /// <summary>
        /// 带参数描述信息的参数脚本数组
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public List<string> ParamsScript
        {
            get { return arrStrParas; }
        }
        ///// <summary>
        ///// 获取参数数组
        ///// </summary>
        ///// <value></value>
        ///// <returns></returns>
        ///// <remarks></remarks>
        //public IDataParameter[] DataParameters
        //{
        //    get { return _DataParameters; }
        //}

        /// <summary>
        /// 参数映射，用于表示参数对应的“属性类”
        /// </summary>
        /// <param name="index">元素索引</param>
        /// <value></value>
        /// <returns>参数结构</returns>
        /// <remarks></remarks>
        public ParamMapType ParamsMap(int index)
        {
            return _ParamsMap[index];
        }

        /// <summary>
        /// 当前需要替换字符串的参数
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public List<string> ParamsReplaceable
        {
            get { return arrStrReplaceText; }
        }

        /// <summary>
        /// 获取或者设置数据库访问对象
        /// </summary>
        /// <value>据库访问对象</value>
        /// <returns>据库访问对象</returns>
        /// <remarks></remarks>
        public CommonDB DataBase
        {
            get { return _DataBase; }
            set { _DataBase = value; }
        }

        /// <summary>
        /// 获取或设置配置文件中的命令组（类）名称
        /// </summary>
        /// <value>命令组（类）名称</value>
        /// <returns>命令组（类）名称</returns>
        /// <remarks></remarks>
        public string CommandClassName
        {
            get { return _CommandClassName; }
            set { _CommandClassName = value; }
        }

        /// <summary>
        /// 获取参数数目
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int ParasLenth
        {
            get { return _ParasLenth; }
        }

        ///// <summary>
        ///// 获取可以执行的SQL命令文本
        ///// </summary>
        ///// <value></value>
        ///// <returns></returns>
        ///// <remarks></remarks>
        //public string CommandText
        //{
        //    get { return _SqlText; }
        //}

        ///// <summary>
        ///// 获取命令类型
        ///// </summary>
        ///// <value></value>
        ///// <returns></returns>
        ///// <remarks></remarks>
        //public CommandType CommandType
        //{
        //    get { return _CommandType; }
        //}

        private DBMSType _dataBaseType=DBMSType.UNKNOWN ;
        /// <summary>
        /// 获取或者设置数据库类型
        /// </summary>
        public DBMSType DataBaseType
        {
            get {
                if (_dataBaseType == DBMSType.UNKNOWN)
                {
                    //原始代码：
                    //_dataBaseType=CommonDB.GetDBMSType(this.DataBase);
                    //解决返回的数据库类型是UNKNOWN 问题
                    //edit at 2011.5.13
                    _dataBaseType = this.DataBase.CurrentDBMSType;
                    
                }
                return _dataBaseType;
            }
            set {
                _dataBaseType = value;
            }
        }

        /// <summary>
        /// 获取或设置SQL Map 配置文件地址
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string SqlMapFile
        {
            get
            {
                if (string.IsNullOrEmpty(_SqlMapFile))
                {
                    _SqlMapFile = ConfigurationManager.AppSettings["SqlMapFile"];
                    if (string.IsNullOrEmpty(_SqlMapFile))
                    {
                        throw new ArgumentOutOfRangeException("SqlMapFile", "该属性没有在应用程序中设置值，请在应用程序配置文件中配置SqlMapFile键和值。 ");
                    }
                }
                return _SqlMapFile;
            }
            set { _SqlMapFile = value; }
        }

        /// <summary>
        /// SQL-MAP 配置脚本文件内容
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string SqlMapScript
        {
            get { return _SqlMapScript; }
        }
        #endregion

        #region "私有的方法"


        //解析SQL脚本信息，分析其中的参数数目
        //Example:
        //Dim SqlScript As String = "UPDATE EngineState SET " & _
        // "DoneLink=#DoneLink:String,String,10#,FoundLink=#FoundLink:String#,SiteCount=#SiteCount:String#, " & _
        // "Formation=#Formation#,ReaderPoint=#ReaderPoint#,UseTime=#UseTime#, " & _
        // "Message=#Message:String#,ObjectNum=#ObjectNum:String#,LookTime=GetDate()  " & _
        //"WHERE PlanID=#PlanID:Int32#"




        /// <summary>
        /// 解析SQL脚本信息，分析其中的参数数目，必须先在GetParameters 之前调用
        /// </summary>
        /// <param name="SqlMapScript">SQL脚本信息</param>
        /// <remarks>
        /// Dim SqlScript As String = "
        /// UPDATE EngineState SET 
        /// DoneLink=#DoneLink:String,String,10#,FoundLink=#FoundLink:String#,SiteCount=#SiteCount:String#,
        /// Formation=#Formation#,ReaderPoint=#ReaderPoint#,UseTime=#UseTime#,
        /// WHERE PlanID=#PlanID:Int32# "
        /// </remarks>
        /// <returns>解析后的原始SQL语句</returns>
        public string  GetScriptInfo(string SqlMapScript)
        {
            string[] ArrStr = SqlMapScript.Split(new char[] { '#' });
            //ArrStr 中奇数将表示参数。
            string  _SqlText = string.Empty;//解析后的原始SQL语句
            bool IsParas = true;
            arrStrParas.Clear();
            arrStrReplaceText.Clear();
            _ParasLenth = 0;
            List<string> strParas = new List<string>();
            for (int I = 0; I <= ArrStr.Length - 1; I++)
            {
                IsParas = !IsParas;
                if (IsParas)
                {
                    string[] strArrTemp = ArrStr[I].Split(':');
                    string strTemp = this.ParaChar + strArrTemp[0].Trim();

                    _SqlText += strTemp;
                    //处理替换参数
                    //VB:if (Strings.Left(strTemp, 2) == this.ParaChar + "%" && Strings.Right(strTemp, 1) == "%")
                    if (strTemp.Substring(0, 2) == this.ParaChar + "%" && strTemp.EndsWith("%"))
                    {
                        //获取替换参数信息，用于生成代码的辅助信息
                        //代码暂时没有实现
                        //VB:arrStrReplaceText.Add(Strings.Mid(strTemp, 3).Replace("%", ""));
                        arrStrReplaceText.Add(strTemp.Substring(2).Replace("%", ""));

                        //Debug.Write(strTemp);
                    }
                    else
                    {
                        //处理同名参数
                        //VB:if (Strings.InStr(strParas, strTemp, CompareMethod.Text) == 0)
                        string str = strTemp.ToLower();
                        if (!strParas.Contains(str))
                        {
                            arrStrParas.Add(ArrStr[I]);
                            strParas.Add(str);
                            _ParasLenth += 1;
                        }
                    }
                }
                else
                {
                    _SqlText += ArrStr[I];
                }
            }
            //_ParasLenth = arrStrParas.Count
            _ParamsMap = new ParamMapType[_ParasLenth];
            return _SqlText;

        }

        /// <summary>
        /// 根据SQL脚本，获取参数（带类型）列表
        /// </summary>
        /// <param name="SqlMapScript">SQL脚本</param>
        /// <returns>参数（带类型）列表</returns>
        /// <remarks></remarks>
        public IDataParameter[] GetParameters(string SqlMapScript)
        {
           
            IDataParameter[] Paras = new IDataParameter[this.ParasLenth];
            // Dim para As IDataParameter

            for (int I = 0; I <= arrStrParas.Count - 1; I++)
            {
                string[] strArrTemp = arrStrParas[I].Split(':');
                string strTemp = strArrTemp[0].Trim();
                string strSystemType = "Object";

                if (strArrTemp.Length > 1)
                {
                    //例如：[System.Type[,System.DbType[,Size[,ParameterDirection]]]]
                    string[] strArrParaTemp = strArrTemp[1].Split(',');

                    string strSystemDbType = string.Empty;
                    int intSize = 0;
                    string strParameterDirection = string.Empty;
                    System.Data.DbType dbType;
                    //根据不同的参数值获取参数形式
                    switch (strArrParaTemp.Length)
                    {
                        case 1:
                            //声明了属性类型
                            strSystemType = strArrParaTemp[0];
                            Paras[I] = _DataBase.GetParameter();
                            Paras[I].ParameterName = this.ParaChar + strTemp;
                            Paras[I].DbType = Type2DbType(strArrParaTemp[0].Trim());

                            break;
                        case 2:
                            //并且声明了数据字段类型

                            strSystemType = strArrParaTemp[0];
                            strSystemDbType = strArrParaTemp[1];
                            Paras[I] = _DataBase.GetParameter();
                            Paras[I].ParameterName = this.ParaChar + strTemp;
                            Paras[I].DbType = (DbType)System.Enum.Parse(typeof(DbType), strSystemDbType.Trim());

                            break;
                        case 3:
                            //并且声明了数据长度
                            strSystemType = strArrParaTemp[0];
                            strSystemDbType = strArrParaTemp[1];

                            dbType = (DbType)System.Enum.Parse(typeof(DbType), strSystemDbType.Trim());
                            if (strArrParaTemp[2] == string.Empty)
                            {
                                intSize = GetDefaultSize(dbType);
                            }
                            else
                            {
                                intSize = Int32.Parse(strArrParaTemp[2].Trim());
                            }

                            Paras[I] = _DataBase.GetParameter(this.ParaChar + strTemp, dbType, intSize, ParameterDirection.Input);

                            break;
                        case 4:
                            //并且声明了参数输入输出类型
                            strSystemType = strArrParaTemp[0];
                            strSystemDbType = strArrParaTemp[1].Trim(); //处理Decimal的精度问题 Decimal(14.2)，表示精度14位，小数 2 位。
                            strParameterDirection = strArrParaTemp[3].Trim();
                            ParameterDirection Direction = default(ParameterDirection);
                            
                            dbType = (DbType)System.Enum.Parse(typeof(DbType), strSystemDbType.Trim());
                            if (strArrParaTemp[2] == string.Empty)
                            {
                                intSize = GetDefaultSize(dbType);
                            }
                            else
                            {
                                intSize = Int32.Parse(strArrParaTemp[2].Trim());
                            }
                            
                            Direction = (ParameterDirection)Enum.Parse(typeof(ParameterDirection), strParameterDirection);
                            Paras[I] = _DataBase.GetParameter(this.ParaChar + strTemp, dbType, intSize, Direction);

                            break;

                        default :
                            //并且声明了参数输入输出类型
                            strSystemType = strArrParaTemp[0];
                            strSystemDbType = strArrParaTemp[1].Trim(); //处理Decimal的精度问题 Decimal(14.2)，表示精度14位，小数 2 位。
                            strParameterDirection = strArrParaTemp[3].Trim();
                            ParameterDirection Direction1 = default(ParameterDirection);

                            dbType = (DbType)System.Enum.Parse(typeof(DbType), strSystemDbType.Trim());
                            if (strArrParaTemp[2] == string.Empty)
                            {
                                intSize = GetDefaultSize(dbType);
                            }
                            else
                            {
                                intSize = Int32.Parse(strArrParaTemp[2].Trim());
                            }

                            Direction1 = (ParameterDirection)Enum.Parse(typeof(ParameterDirection), strParameterDirection);
                            byte  Precision = 18;//精度
                            byte  Scale = 4;//小数位

                            if (strArrParaTemp.Length == 5)
                            {
                                if (strArrParaTemp[4] != string.Empty)
                                {
                                    Precision = byte.Parse(strArrParaTemp[4]); 
                                }
                            }
                            if (strArrParaTemp.Length == 6)
                            {
                                if (strArrParaTemp[5] != string.Empty)
                                {
                                    Scale = byte.Parse(strArrParaTemp[5]);
                                }
                            }
                            Paras[I] = _DataBase.GetParameter(this.ParaChar + strTemp, dbType, intSize, Direction1, Precision, Scale);
                            break;
                    }
                }
                else
                {
                    Paras[I] = _DataBase.GetParameter();
                    Paras[I].ParameterName = this.ParaChar + strTemp;
                }
                //参数的属性名和属性类型
                _ParamsMap[I].ParamName = strTemp;
                _ParamsMap[I].TypeCode = (TypeCode)Enum.Parse(typeof(TypeCode), strSystemType.Trim());
            }
            return Paras;
        }

        /// <summary>
        /// 系统类型到数据库类型转换
        /// </summary>
        /// <param name="strSystemType"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private DbType Type2DbType(string strSystemType)
        {
            strSystemType = strSystemType.ToLower();//VB: Strings.LCase(strSystemType);
            //VB: if (Strings.Left(strSystemType, 7) == "system.")
            if (strSystemType.Length >7 && strSystemType.Substring(0, 7) == "system.")
            {
                strSystemType = strSystemType.Substring(7);//VB: Strings.Mid(strSystemType, 8);
            }
            switch (strSystemType)
            {
                case "boolean":
                    //System.Boolean
                    return DbType.Boolean;
                case "byte":
                    //System.Byte
                    return DbType.Byte;
                case "char":
                    //System.Char
                    return DbType.AnsiStringFixedLength;
                case "datetime":
                    //System.DateTime
                    return DbType.DateTime;
                case "decimal":
                    //System.Decimal
                    return DbType.Decimal;
                case "double":
                    //System.Double
                    return DbType.Double;
                case "guid":
                    //System.Guid
                    return DbType.Guid;
                case "int16":
                    //System.Int16
                    return DbType.Int16;
                case "int32":
                    //System.Int32
                    return DbType.Int32;
                case "int64":
                    //System.Int64
                    return DbType.Int64;
                case "single":
                    //System.Single
                    return DbType.Single;
                case "string":
                    //System.String
                    //注：原来 string 转换成 DbType.AnsiString ，在PostgreSQL会遇到问题，
                    //参见 http://www.cnblogs.com/bluedoctor/archive/2011/05/18/2050276.html
                    return DbType.String ;
                case "byte[]":
                    //System.Byte[]
                    return DbType.Binary;
                case "type":
                    //System.Type
                    return DbType.String;
                default:
                    return DbType.Object;
            }
        }

        /// <summary>
        /// 获取默认的数据类型长度
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private int GetDefaultSize(DbType dbType)
        {
            switch (dbType)
            {
                case DbType.String:
                    return 255;
                case DbType.AnsiString:
                    return 50;
                case DbType.AnsiStringFixedLength:
                    return 50;
                case DbType.Binary:
                    return 50;
                case DbType.Boolean:
                    return 1;
                case DbType.Byte:
                    return 1;
                case DbType.Currency:
                    return 8;
                case DbType.Date:
                    return 4;
                case DbType.DateTime:
                    return 8;
                case DbType.Decimal:
                    return 9;
                case DbType.Double:
                    return 16;
                case DbType.Guid:
                    return 16;
                case DbType.Int16:
                    return 2;
                case DbType.Int32:
                    return 4;
                case DbType.Int64:
                    return 8;
                case DbType.Object:
                    return 4;
                case DbType.SByte:
                    return 2;
                case DbType.Single:
                    return 8;
                case DbType.StringFixedLength:
                    return 50;
                case DbType.Time:
                    return 8;
                default:
                    return 4;
            }
        }

        //'从SQL MAP 命令配置文件获取命名参数信息
        //Public Function GetCommandParameters() As IDataParameter()
        //    If _SqlMapScript = String.Empty Then
        //        Throw New Exception("未初始化命名信息，请先执行 InitCommandInfo 方法！")
        //    End If
        //    Return GetParameters(_SqlMapScript)
        //End Function

        #endregion

        #region "公开的方法"

        /// <summary>
        /// 获得配置命令信息
        /// </summary>
        /// <param name="XmlCommandName">配置文件中的命令名称</param>
        /// <returns>命令信息</returns>
        public CommandInfo GetCommandInfo(string XmlCommandName)
        {
            XmlCommand xmlCommand = new XmlCommand(SqlMapFile, this.DataBaseType );
            if (!string.IsNullOrEmpty(CommandClassName))
            {
                xmlCommand.CommandClassName = CommandClassName;
            }

            _SqlMapScript = xmlCommand.GetCommand(XmlCommandName);
            if (string.IsNullOrEmpty(_SqlMapScript))
            {
                string errMsg = "没有找到配置信息中的命令：" + XmlCommandName;
                if (!string.IsNullOrEmpty(xmlCommand.ErrDescription))
                    errMsg += "。内部错误原因：" + xmlCommand.ErrDescription;
                throw new Exception(errMsg );
            }

            CommandInfo cmdInfo = new CommandInfo(this.DataBaseType );
            cmdInfo.ParaChar = this.ParaChar;
            cmdInfo.CommandType = xmlCommand.CommandType;
            cmdInfo.CommandText = GetScriptInfo(_SqlMapScript);//必须先分析脚本信息
            cmdInfo.DataParameters = GetParameters(_SqlMapScript);
            //处理存储过程名中的参数
            if (cmdInfo.CommandType == CommandType.StoredProcedure)
            {
                cmdInfo.CommandText = FindWords(cmdInfo.CommandText, 0, 255);
            }
            
            return cmdInfo;

            //_CommandType = xmlCommand.CommandType;
            //_DataParameters = GetParameters(_SqlMapScript);
        }

        /// <summary>
        /// 从输入字符串中寻找一个单词，忽略前面的空白字符，直到遇到单词之后第一个空白字符或者分割符或者标点符号为止。
        /// </summary>
        /// <param name="inputString">输入的字符串</param>
        /// <param name="startIndex">在输入字符串中要寻找单词的起始位置</param>
        /// <param name="maxLength">单词的最大长度，忽略超出部分</param>
        /// <returns>找到的新单词</returns>
        public string FindWords(string inputString, int startIndex, int maxLength)
        {
            maxLength = maxLength > inputString.Length ? inputString.Length : maxLength;

            bool start = false;
            char[] words = new char[maxLength];//存储过程名字，最大长度255；
            int index = 0;
            
            foreach (char c in inputString.ToCharArray(startIndex ,maxLength  ))
            {
                if (Char.IsWhiteSpace(c))
                {
                    if (!start)
                        continue;//过滤前面的空白字符
                    else
                        break;//已经获取过字母字符，又遇到了空白字符，说明单词已经结束，跳出。
                }
                else
                {
                    if (Char.IsSeparator(c) || Char.IsPunctuation(c))
                    {
                        if(c == '.' || c=='_' || c=='-' || c=='[' || c==']' )
                            words[index++] = c;//放入字母，找单词
                        else
                            break;//分割符或者标点符号，跳出。

                    }
                    else
                    {
                        words[index++] = c;//放入字母，找单词
                    
                    }
                    if (!start)
                        start = true;
                }
            }
            return  new string(words, 0, index);
        }

        /// <summary>
        /// 根据SQL-MAP脚本获取命令信息，注意不包含命令类型
        /// </summary>
        /// <param name="SqlMapScript">SQL-MAP脚本</param>
        /// <returns>命令信息</returns>
        public CommandInfo GetCommandInfoBySqlMapScript(string SqlMapScript)
        {
            CommandInfo cmdInfo = new CommandInfo(this.DataBaseType );
            cmdInfo.ParaChar = this.ParaChar;
            //cmdInfo.CommandType = xmlCommand.CommandType;
            cmdInfo.CommandText = GetScriptInfo(SqlMapScript);//必须先分析脚本信息
            cmdInfo.DataParameters = GetParameters(_SqlMapScript);
            return cmdInfo;
        }

        ///// <summary>
        ///// 给指定的参数赋值
        ///// </summary>
        ///// <param name="paramName"></param>
        ///// <param name="paramValue"></param>
        ///// <remarks></remarks>
        //public void SetParameterValue(string paramName, object paramValue)
        //{
        //    for (int I = 0; I <= _DataParameters.Length - 1; I++)
        //    {
        //        if (_DataParameters[I].ParameterName == paramName)
        //        {
        //            _DataParameters[I].Value = paramValue;
        //            return;
        //        }
        //    }
        //    throw new Exception("没有找到指定的参数名：" + paramName);
        //}

        ///// <summary>
        ///// 指定参数替换类型的参数赋值方法,paramName 不带Me.ParaChar 或者 "#"限定符
        ///// </summary>
        ///// <param name="paramName">参数名</param>
        ///// <param name="paramValue">参数值</param>
        ///// <param name="paramType">参数类型</param>
        ///// <remarks></remarks>
        //public void SetParameterValue(string paramName, string paramValue, enumParamType paramType)
        //{
        //    if (paramType == enumParamType.DataParameter)
        //    {
        //        SetParameterValue(paramName, paramValue);
        //    }
        //    else
        //    {
        //        if (_SqlText == null | _SqlText == string.Empty)
        //        {
        //            throw new Exception("无法设置替换参数，可能命令没有初始化．");
        //        }
        //        _SqlText = _SqlText.Replace(this.ParaChar + "%" + paramName + "%", paramValue);
        //    }
        //}

        /// <summary>
        /// 将DataReader中的结果数据映射到实体对象
        /// </summary>
        /// <param name="reader">数据阅读器</param>
        /// <param name="result">实体对象实例</param>
        /// <returns>填充后的实体对象</returns>
        /// <remarks></remarks>
        public EntityBase ResultMapEntity(IDataReader reader, EntityBase result)
        {
            using (reader)
            {
                //if (result!=null && reader.Read())
                //{
                   
                //    for (int i = 0; i < reader.FieldCount; i++)
                //    {
                //        if (!reader.IsDBNull(i))
                //            result.PropertyList [reader.GetName(i)]= reader.GetValue(i);
                //    }
                   
                //}

                if (reader.Read())
                {
                    int fcount = reader.FieldCount;
                    string[] names = new string[fcount];

                    for (int i = 0; i < fcount; i++)
                        names[i] = reader.GetName(i);

                    object[] values = new object[fcount];
                    reader.GetValues(values);


                    result.PropertyNames = names;
                    result.PropertyValues = values;
                }
            }
            return result;
        }

        /// <summary>
        /// 将DataReader中的结果数据映射到实体对象集合，建议使用 EntityQuery泛型类来获取实体类集合
        /// </summary>
        /// <param name="reader">数据阅读器</param>
        /// <param name="result">实体对象实例</param>
        /// <returns>实体对象集合</returns>
        public List<EntityBase> ResultMapEntityList(IDataReader reader, EntityBase result)
        {
            using (reader)
            {
                if (result != null)
                {
                    List<EntityBase> list = new List<EntityBase>();

                    if (reader.Read())
                    {
                        int fcount = reader.FieldCount;
                        string[] names = new string[fcount];
                        object[] values = null;

                        for (int i = 0; i < fcount; i++)
                            names[i] = reader.GetName(i);

                        do
                        {
                            values = new object[fcount];
                            reader.GetValues(values);

                            EntityBase item = (EntityBase)result.Clone(false );
                            item.PropertyNames = names;
                            item.PropertyValues = values;

                            list.Add(item);
                        } while (reader.Read());

                    }
                    return list;
                }
            }
            return null;
        }
        
        #endregion

    }

}
