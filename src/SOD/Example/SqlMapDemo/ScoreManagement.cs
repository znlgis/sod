//ʹ�øó���ǰ�������ó��򼯣�PWMIS.Core���������涨������ƿռ�ǰ׺��Ҫʹ�ãУףͣɣӣ�������Ϣ����鿴 http://www.pwmis.com/sqlmap 
// ========================================================================
// Copyright(c) 2008-2010 ��˾����, All Rights Reserved.
// ========================================================================

using System.Collections.Generic;
using System.Data;
using PWMIS.DataMap.Entity;
using PWMIS.DataMap.SqlMap;

namespace SqlMapDemo.SqlMapDAL
{
    /// <summary>
    ///     �ļ�����ScoreManagement.cs
    ///     �ࡡ����ScoreManagement
    ///     �桡����1.0
    ///     ����ʱ�䣺2019/5/17 12:41:09
    ///     ��;��������������
    ///     ������Ϣ�����ļ��� PDF.NET Code Maker �Զ����ɣ��޸�ǰ���ȱ��ݣ�
    /// </summary>
    public class ScoreManagement
        : DBMapper
    {
        /// <summary>
        ///     Ĭ�Ϲ��캯��
        /// </summary>
        public ScoreManagement()
        {
            Mapper.CommandClassName = "ScoreManagement";
            //CurrentDataBase.DataBaseType=DataBase.enumDataBaseType.SqlServer;
            Mapper.EmbedAssemblySource =
                "SqlMapDemo,SqlMapDemo.SqlMap.config"; //SQL-MAP�ļ�Ƕ��ĳ������ƺ���Դ���ƣ�����ж��SQL-MAP�ļ������ڴ�ָ����
        }


        /// <summary>
        ///     ��ѯ����ѧ����Ϣ
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllStudents()
        {
            //��ȡ������Ϣ
            var cmdInfo = Mapper.GetCommandInfo("GetAllStudents");
            //ִ�в�ѯ
            return CurrentDataBase.ExecuteDataSet(CurrentDataBase.ConnectionString, cmdInfo.CommandType,
                cmdInfo.CommandText, null);
            //
        } //End Function

        /// <summary>
        ///     ��ѯ����ϵ��ѧ����Ϣ
        /// </summary>
        /// <param name="DID"></param>
        /// <returns></returns>
        public DataSet GetStudent(int DID)
        {
            //��ȡ������Ϣ
            var cmdInfo = Mapper.GetCommandInfo("GetStudent");
            //������ֵ���Ƽ�ʹ�ø��ַ�ʽ��
            cmdInfo.DataParameters[0].Value = DID;
            //������ֵ��ʹ��������ʽ��
            //cmdInfo.SetParameterValue("@DID", DID);
            //ִ�в�ѯ
            return CurrentDataBase.ExecuteDataSet(CurrentDataBase.ConnectionString, cmdInfo.CommandType,
                cmdInfo.CommandText, cmdInfo.DataParameters);
            //
        } //End Function

        /// <summary>
        ///     ��ѯ����ϵ��ѧ���ɼ�
        /// </summary>
        /// <param name="Category"></param>
        /// <returns></returns>
        public DataSet GetStudentScore(string Category)
        {
            //��ȡ������Ϣ
            var cmdInfo = Mapper.GetCommandInfo("GetStudentScore");
            //������ֵ���Ƽ�ʹ�ø��ַ�ʽ��
            cmdInfo.DataParameters[0].Value = Category;
            //������ֵ��ʹ��������ʽ��
            //cmdInfo.SetParameterValue("@Category", Category);
            //ִ�в�ѯ
            return CurrentDataBase.ExecuteDataSet(CurrentDataBase.ConnectionString, cmdInfo.CommandType,
                cmdInfo.CommandText, cmdInfo.DataParameters);
            //
        } //End Function

        /// <summary>
        ///     ��ѯѧ���ĳɼ���ӳ������DTO�б�
        /// </summary>
        /// <param name="StuId"></param>
        /// <returns></returns>
        public List<StudentScore> GetStudentScore2(int StuId)
        {
            //��ȡ������Ϣ
            var cmdInfo = Mapper.GetCommandInfo("GetStudentScore2");
            //������ֵ���Ƽ�ʹ�ø��ַ�ʽ��
            cmdInfo.DataParameters[0].Value = StuId;
            //������ֵ��ʹ��������ʽ��
            //cmdInfo.SetParameterValue("@StuId", StuId);
            //ִ�в�ѯ
            return MapObjectList<StudentScore>(CurrentDataBase.ExecuteReader(CurrentDataBase.ConnectionString,
                cmdInfo.CommandType, cmdInfo.CommandText, cmdInfo.DataParameters));
            //
        } //End Function

        /// <summary>
        ///     ��ѯѧ���ĳɼ���ӳ������ʵ�����б�
        /// </summary>
        /// <param name="StuId"></param>
        /// <returns></returns>
        public List<ScoreEntity> GetStudentScoreEntitys(int StuId)
        {
            //��ȡ������Ϣ
            var cmdInfo = Mapper.GetCommandInfo("GetStudentScoreEntitys");
            //������ֵ���Ƽ�ʹ�ø��ַ�ʽ��
            cmdInfo.DataParameters[0].Value = StuId;
            //������ֵ��ʹ��������ʽ��
            //cmdInfo.SetParameterValue("@StuId", StuId);
            //ִ�в�ѯ
            return EntityQuery<ScoreEntity>.QueryList(CurrentDataBase.ExecuteReader(CurrentDataBase.ConnectionString,
                cmdInfo.CommandType, cmdInfo.CommandText, cmdInfo.DataParameters));
            //
        } //End Function

        /// <summary>
        ///     ����ѧ��
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="DeptId"></param>
        /// <returns></returns>
        public int AddStudent(string Name, int DeptId)
        {
            //��ȡ������Ϣ
            var cmdInfo = Mapper.GetCommandInfo("InsertStudent");
            //������ֵ���Ƽ�ʹ�ø��ַ�ʽ��
            cmdInfo.DataParameters[0].Value = Name;
            cmdInfo.DataParameters[1].Value = DeptId;
            //������ֵ��ʹ��������ʽ��
            //cmdInfo.SetParameterValue("@Name", Name);
            //cmdInfo.SetParameterValue("@DeptId", DeptId);
            //ִ�в�ѯ
            return CurrentDataBase.ExecuteNonQuery(CurrentDataBase.ConnectionString, cmdInfo.CommandType,
                cmdInfo.CommandText, cmdInfo.DataParameters);
            //
        } //End Function
    } //End Class
} //End NameSpace 