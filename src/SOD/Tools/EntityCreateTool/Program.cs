using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EntityCreateTool
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //EntityCreateTool.exe MyNamespace abc.txt d:\\entitys
            if (args.Length < 3)
            {
                Console.WriteLine("SOD 实体类生成工具，命令行：");
                Console.WriteLine("EntityCreateTool.exe <类命名空间> <表定义文件> <实体类文件输出目录> [DTO类文件输出目录]");
                Console.WriteLine("表定义文件格式示例：");
                var formatText = @"
# 第一行以 table 开始，第二个单词为表名称，例如 Table1
table	Table1
# 该行内容将作为表的说明，下面一行作为表头。每个数据使用制表符分隔，从序号1开始定义第一个字段内容，直到空行结束，然后循环处理下一个表定义。
序号	字段	说明	数据类型	默认值	非空	主键	备注
1	ID	ID号	int(32)	自增	是	√	自增
2	Name	名称 nvarchar(32)	NULL	是	/	/
3	Value	值	nvarchar(32)	NULL	是	/	/
";
                Console.WriteLine(formatText);
                Console.Read();
            }
            else
            {
                Console.WriteLine("*----------------------------------------------------------------------");
                Console.WriteLine("* SOD Framework EntityCreateTool (Ver 1.0 Date:2023-5-1).");
                Console.WriteLine("* namespace:{0}", args[0]);
                Console.WriteLine("* Table Define File:{0}", args[1]);
                Console.WriteLine("* Entity File Output path:{0}", args[2]);
                if (args.Length > 3) Console.WriteLine("DTO File Output path:{0}", args[3]);
                Console.WriteLine("*----------------------------------------------------------------------");
            }

            var fileFormat = @"
//-----------------------------------------------------------------
// SOD Framework (https://github.com/znlgis/sod)
// EntityCreateTool (Ver 1.0 Date:2023-5-1) Created SOD Entity File.
// Created Date: #CreatedDate#
// Please do not modify this file.
//-----------------------------------------------------------------
using System;
using PWMIS.DataMap.Entity;

namespace #NameSpace#
{
    /// <summary>
    #ClassSummary# 
    /// </summary>
    public class #ClassName#Entity:EntityBase
    {
        public #ClassName#Entity()
        {
            #TableName#
            #IdentityName# 
            #PrimaryKeys#
        }

        #Propertys#
       
    }
}
";
            var fileDtoFormat = @"
//-----------------------------------------------------------------
// SOD Framework (https://github.com/znlgis/sod)
// EntityCreateTool (Ver 1.0 Date:2023-5-1) Created DTO File.
// Created Date: #CreatedDate#
// Please do not modify this file.
//-----------------------------------------------------------------
using System;

namespace #NameSpace#
{
    /// <summary>
    #ClassSummary# 
    /// </summary>
    public class #ClassName#Dto
    {
        public #ClassName#Dto()
        {
           
        }

        #Propertys#
       
    }
}
";

            var CreatedDate = DateTime.Now.ToString();
            var strNameSpace = args[0];
            var ClassName = "Sample";
            var TableName = "TableName = \"Table1\";";
            var IdentityName = "";
            var PrimaryKeys = "";
            var Propertys = "";
            var PropertySummary = "/// ";
            var ClassSummary = "/// ";

            var propertyFormat = @"
        /// <summary>
        #Summary# 
        /// </summary>#Remarks#
        public #PropertyType# #PropertyName#
        {
            get { return getProperty<#PropertyType#>(""#PropertyFiledName#""); }
            set { setProperty(""#PropertyFiledName#"", value); }
        }
";
            var propertyStrFormat = @"
        /// <summary>
        #Summary#  
        /// </summary>#Remarks#
        public #PropertyType# #PropertyName#
        {
            get { return getProperty<#PropertyType#>(""#PropertyFiledName#""); }
            set { setProperty(""#PropertyFiledName#"", value,#FieldLength#); }
        }
";
            var propertyDtoFormat = @"
        /// <summary>
        #Summary# 
        /// </summary>#Remarks#
        public #PropertyType# #PropertyName#
        {
            get ;
            set ;
        }
";

            var PropertyType = "int";
            var PropertyName = "ID";
            var PropertyFiledName = "ID";
            var FieldLength = "250";
            var Remarks = "";

            var tableDefineFile = args[1];
            var EntityOutDir = args[2];
            var DtoOutDir = args.Length > 3 ? args[3] : "";
            try
            {
                if (!Directory.Exists(EntityOutDir)) Directory.CreateDirectory(EntityOutDir);
                if (DtoOutDir != "" && !Directory.Exists(DtoOutDir)) Directory.CreateDirectory(DtoOutDir);
            }
            catch
            {
                Console.WriteLine("Directory Access Error.\r\n Entity Out Dir:{0},\r\n DtoOutDir:{1}",
                    EntityOutDir, DtoOutDir);
                Console.Read();
                return;
            }

            using (var sr = File.OpenText(tableDefineFile))
            {
                var s = "";
                TableName = "";
                string[] fieldDefine;
                var propertyList = new List<string>();
                var propertyDtoList = new List<string>();

                while ((s = sr.ReadLine()) != null)
                {
                    if (s.StartsWith("#table"))
                    {
                        var arr = s.Split(new[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (arr.Length >= 2)
                            if (TableName != "" && TableName != arr[1])
                            {
                                //读取到新表的定义，处理上一个表的结果
                                if (propertyList.Count > 0)
                                {
                                    var sb = new StringBuilder();
                                    foreach (var property in propertyList)
                                    {
                                        sb.Append(property);
                                        sb.AppendLine();
                                    }

                                    Propertys = sb.ToString();

                                    var entityFileText = fileFormat.Replace("#CreatedDate#", CreatedDate)
                                        .Replace("#NameSpace#", strNameSpace + ".Entitys")
                                        .Replace("#ClassName#", ClassName)
                                        .Replace("#TableName#", TableName)
                                        .Replace("#IdentityName#", IdentityName)
                                        .Replace("#PrimaryKeys#", PrimaryKeys)
                                        .Replace("#Propertys#", Propertys)
                                        .Replace("#ClassSummary#", ClassSummary);

                                    //Console.WriteLine(entityFileText);
                                    WriteFile(ClassName, EntityOutDir, ".cs", entityFileText);
                                    Console.WriteLine();

                                    //处理DTO文件
                                    var sbDto = new StringBuilder();
                                    foreach (var property in propertyDtoList)
                                    {
                                        sbDto.Append(property);
                                        sbDto.AppendLine();
                                    }

                                    Propertys = sbDto.ToString();

                                    var dtoFileText = fileDtoFormat.Replace("#CreatedDate#", CreatedDate)
                                        .Replace("#NameSpace#", strNameSpace + ".DTO")
                                        .Replace("#ClassName#", ClassName)
                                        .Replace("#Propertys#", Propertys)
                                        .Replace("#ClassSummary#", ClassSummary);

                                    if (!string.IsNullOrEmpty(DtoOutDir))
                                        WriteFile(ClassName, DtoOutDir, ".cs", dtoFileText);
                                    else
                                        Console.WriteLine(dtoFileText);
                                    Console.WriteLine();
                                }

                                propertyList.Clear();
                                propertyDtoList.Clear();
                            }

                        TableName = string.Format("TableName = \"{0}\";", arr[1]);
                        ClassName = arr[1].Replace(" ", "");
                        if (char.IsLower(ClassName[0]))
                            //将首字母转换为大写
                            ClassName = string.Concat(char.ToUpper(ClassName[0]), ClassName.TrimStart(ClassName[0]));
                        var nextLine = sr.ReadLine();
                        if (nextLine != null)
                            ClassSummary = "/// " + nextLine;
                        else
                            ClassSummary = "/// ";
                    }

                    //以数字序号开头，可能是字段定义
                    if (s.Length > 0 && char.IsNumber(s[0]))
                    {
                        FieldLength = "";

                        var arr = s.Split('\t');
                        if (arr.Length != 8)
                            arr = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (arr.Length != 8)
                            continue;
                        fieldDefine = arr;

                        PropertyFiledName = fieldDefine[1];
                        PropertyName = PropertyFiledName.Replace(" ", "");
                        if (!char.IsLetter(PropertyName[0]))
                            //处理不合法的变量名
                            PropertyName = "P" + PropertyName;
                        if (char.IsLower(PropertyName[0]))
                            //将首字母转换为大写
                            PropertyName = string.Concat(char.ToUpper(PropertyName[0]),
                                PropertyName.TrimStart(PropertyName[0]));

                        //计算字段长度和类型
                        var arr1 = fieldDefine[3].Split('(', ')');
                        if (arr1.Length > 1)
                        {
                            int length;
                            if (int.TryParse(arr1[1], out length)) FieldLength = length.ToString();
                            PropertyType = ChangeDbType(arr1[0], length);
                        }
                        else
                        {
                            PropertyType = ChangeDbType(arr1[0], 0);
                        }

                        //处理默认值
                        if (fieldDefine[4] == "自增" || fieldDefine[4].ToLower() == "identity")
                            IdentityName = string.Format("IdentityName = \"{0}\";", PropertyFiledName);
                        //处理主键
                        if (fieldDefine[6] == "是" || fieldDefine[6] == "√" || fieldDefine[6].ToUpper() == "Y")
                            PrimaryKeys = string.Format("PrimaryKeys.Add(\"{0}\");", PropertyFiledName);
                        //处理属性注释
                        PropertySummary = "/// " + fieldDefine[2];
                        Remarks = "";
                        if (!string.IsNullOrEmpty(fieldDefine[7].Trim()))
                        {
                            var template1 = @"
        /// <remarks>
        /// {0}
        /// </remarks>
";
                            Remarks = string.Format(template1, fieldDefine[7]);
                        }

                        var template = PropertyType == "string" ? propertyStrFormat : propertyFormat;
                        var propertyItem = template.Replace("#PropertyType#", PropertyType)
                            .Replace("#PropertyName#", PropertyName)
                            .Replace("#PropertyFiledName#", PropertyFiledName)
                            .Replace("#FieldLength#", FieldLength)
                            .Replace("#Summary#", PropertySummary)
                            .Replace("#Remarks#", Remarks);

                        propertyList.Add(propertyItem);

                        var propertyDtoItem = propertyDtoFormat.Replace("#PropertyType#", PropertyType)
                            .Replace("#PropertyName#", PropertyName)
                            .Replace("#Summary#", PropertySummary)
                            .Replace("#Remarks#", Remarks);

                        propertyDtoList.Add(propertyDtoItem);
                    } //end if
                } //end while

                //读取到新表的定义，处理上一个表的结果
                if (propertyList.Count > 0)
                {
                    var sb = new StringBuilder();
                    foreach (var property in propertyList)
                    {
                        sb.Append(property);
                        sb.AppendLine();
                    }

                    Propertys = sb.ToString();

                    var entityFileText = fileFormat.Replace("#CreatedDate#", CreatedDate)
                        .Replace("#NameSpace#", strNameSpace)
                        .Replace("#ClassName#", ClassName)
                        .Replace("#TableName#", TableName)
                        .Replace("#IdentityName#", IdentityName)
                        .Replace("#PrimaryKeys#", PrimaryKeys)
                        .Replace("#Propertys#", Propertys)
                        .Replace("#ClassSummary#", ClassSummary);

                    //Console.WriteLine(entityFileText);
                    WriteFile(ClassName, EntityOutDir, ".cs", entityFileText);
                    Console.WriteLine();

                    //处理DTO文件
                    var sbDto = new StringBuilder();
                    foreach (var property in propertyDtoList)
                    {
                        sbDto.Append(property);
                        sbDto.AppendLine();
                    }

                    Propertys = sbDto.ToString();

                    var dtoFileText = fileDtoFormat.Replace("#CreatedDate#", CreatedDate)
                        .Replace("#NameSpace#", strNameSpace + ".DTO")
                        .Replace("#ClassName#", ClassName)
                        .Replace("#Propertys#", Propertys)
                        .Replace("#ClassSummary#", ClassSummary);

                    if (!string.IsNullOrEmpty(DtoOutDir))
                        WriteFile(ClassName, DtoOutDir, ".cs", dtoFileText);
                    else
                        Console.WriteLine(dtoFileText);
                    Console.WriteLine();
                }

                propertyList.Clear();
                propertyDtoList.Clear();
            } //end using

            Console.Read();
        }

        private static string ChangeDbType(string dbType, int length)
        {
            dbType = dbType.ToLower();
            if (dbType == "varchar" || dbType == "nvarchar" || dbType == "nchar" || dbType == "char")
                return "string";
            if (dbType == "int" && length == 64)
                return "long";
            if (dbType == "int" && length == 16)
                return "short";
            if (dbType == "datetime")
                return "DateTime";
            if (dbType == "smallint")
                return "short";
            if (dbType == "bit")
                return "bool";
            return dbType;
        }

        private static void WriteFile(string className, string path, string fileExt, string fileContent)
        {
            var fileName = className + fileExt;
            var fullPath = Path.Combine(path, fileName);
            try
            {
                File.WriteAllText(fullPath, fileContent, Encoding.UTF8);
                Console.WriteLine("Save File OK. path: {0}", fullPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Save File {0} Exception,Error Message:{1}", fullPath, ex.Message);
            }
        }
    }
}