//ʹ�øó���ǰ�������ó��򼯣�PWMIS.Core���������涨������ƿռ�ǰ׺��Ҫʹ�ãУףͣɣӣ�������Ϣ����鿴 http://www.pwmis.com/sqlmap 
// ========================================================================
// Copyright(c) 2008-2010 ��˾����, All Rights Reserved.
// ========================================================================

using System.Data;
using PWMIS.DataMap.SqlMap;

namespace SqlMapDemo.SqlMapDAL
{
    /// <summary>
    ///     �ļ�����TestSqlMapClass.cs
    ///     �ࡡ����TestSqlMapClass
    ///     �桡����1.0
    ///     ����ʱ�䣺2019/5/17 12:41:09
    ///     ��;������SQL-MAPʾ�����Գ���
    ///     ������Ϣ�����ļ��� PDF.NET Code Maker �Զ����ɣ��޸�ǰ���ȱ��ݣ�
    /// </summary>
    public class TestSqlMapClass
        : DBMapper
    {
        /// <summary>
        ///     Ĭ�Ϲ��캯��
        /// </summary>
        public TestSqlMapClass()
        {
            Mapper.CommandClassName = "TestGroup";
            //CurrentDataBase.DataBaseType=DataBase.enumDataBaseType.SqlServer;
            Mapper.EmbedAssemblySource =
                "SqlMapDemo,SqlMapDemo.SqlMap.config"; //SQL-MAP�ļ�Ƕ��ĳ������ƺ���Դ���ƣ�����ж��SQL-MAP�ļ������ڴ�ָ����
        }


        /// <summary>
        ///     �ҳ�ÿһ��ϵ����߷֣����Ұ�ϵ��ţ�ѧ�������������
        /// </summary>
        /// <returns></returns>
        public DataSet QueryStudentSores()
        {
            //��ȡ������Ϣ
            var cmdInfo = Mapper.GetCommandInfo("QueryStudentSores");
            //ִ�в�ѯ
            return CurrentDataBase.ExecuteDataSet(CurrentDataBase.ConnectionString, cmdInfo.CommandType,
                cmdInfo.CommandText, null);
            //
        } //End Function
    } //End Class
} //End NameSpace 