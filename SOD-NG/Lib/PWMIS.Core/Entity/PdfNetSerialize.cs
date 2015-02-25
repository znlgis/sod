using System;
using System.Collections.Generic;
using System.Text;
using PWMIS.DataMap.Entity;
using System.IO;

namespace PWMIS.DataMap.Entity
{
     
    /// <summary>
    /// PDF.NET 实体类序列化器
    /// </summary>
    public class PdfNetSerialize
    {
       public  const int ENTITY_ITEM_FLAG = 1010280;//纪念今天的日子 2010.10.28
       public  const int ENTITY_ARRAY_FLAG = 1010281;

        /// <summary>
        /// 二进制序列化
        /// </summary>
        /// <param name="entity">实体类实例</param>
        /// <returns>字节数组</returns>
        public static byte[] BinarySerialize(EntityBase entity)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            Type factEntityType = entity.GetType();
            EntityFields ef = EntityFieldsCache.Item(factEntityType);
            byte b;
            //写入实体类标记
            bw.Write(ENTITY_ITEM_FLAG);
            int length = entity.PropertyValues.Length-1;
            for (int i = 0; i <= length; i++)
            {
                object obj = entity.PropertyValues[i];
                //if (obj == System.DBNull.Value) obj = null;//DBNull.Value在Convert 的时候会失败
                //为每个属性添加null标记
                if (obj == System.DBNull.Value || obj == null)
                    b = 0;//ＮＵＬＬ标记
                else
                {
                    if (length == i) b = byte.MaxValue; else b = 1;//如果标志位为 byte.MaxValue ，则表示已经写入所有的属性字段，可用于以后添加实体属性但又要成功读取原有的记录
                }
                

                bw.Write(b); //写入标记
                if (b >0 )
                {
                    Type propertyType = ef.GetPropertyType(entity.PropertyNames[i]);
                    if (propertyType == null)
                        throw new Exception("PDF.NET实体类序列化错误：未知的实体属性类型，请检查实体类的属性和字段定义是否匹配。");

                    switch (propertyType.Name )
                    {
                        case "Int32":
                            bw.Write(Convert.ToInt32(obj)); break;
                        case "String":
                            bw.Write(Convert.ToString(obj));
                            break;
                        case "DateTime":
                            bw.Write(((DateTime)obj).ToBinary());
                            break;
                        case "Int16": bw.Write(Convert.ToInt16(obj)); break;
                        case "Int64": bw.Write(Convert.ToInt64(obj)); break;
                        case "Single": bw.Write(Convert.ToSingle(obj)); break;
                        case "Double": bw.Write(Convert.ToDouble(obj)); break;
                        case "Decimal": bw.Write(Convert.ToDecimal(obj)); break;
                        case "Boolean": bw.Write(Convert.ToBoolean(obj)); break;
                        case "Byte": bw.Write(Convert.ToByte(obj)); break;
                        case "Char": bw.Write(Convert.ToChar(obj)); break;
                        case "Byte[]":
                            Byte[] buffer = (Byte[])obj;
                            bw.Write(buffer.Length);//写入字节序列的长度
                            if (buffer.Length > 0)
                                bw.Write(buffer);
                            break;
                    }
                }
                

            }

            bw.Close();
            ms.Close();
            return ms.ToArray();
        }

        protected internal  static void  BinaryDeserializeCommon(BinaryReader br, EntityFields ef,EntityBase factEntity)
        {
            int flag = br.ReadInt32();
            if (flag != PdfNetSerialize.ENTITY_ITEM_FLAG)
                throw new Exception("反序列化错误：不是有效的实体类数据格式！");

            byte FieldFlag;
            for (int i = 0; i < factEntity.PropertyValues.Length; i++)
            {
                object obj = null;
                FieldFlag = br.ReadByte();
                if (FieldFlag == 0)
                {
                    obj = DBNull.Value;
                }
                else
                {
                    Type propertyType = ef.GetPropertyType(factEntity.PropertyNames[i]);
                    if (propertyType == null)
                        throw new Exception("PDF.NET实体类序列化错误：未知的实体属性类型，请检查实体类的属性和字段定义是否匹配。");

                    switch (propertyType.Name)
                    {
                        case "Int32": obj = br.ReadInt32(); break;
                        case "String":
                            obj = br.ReadString();//继续读一个字符串
                            break;
                        case "DateTime": obj = DateTime.FromBinary(br.ReadInt64()); break;
                        case "Int16": obj = br.ReadInt16(); break;
                        case "Int64": obj = br.ReadInt64(); break;
                        case "Single": obj = br.ReadSingle(); break;
                        case "Double": obj = br.ReadDouble(); break;
                        case "Decimal": obj = br.ReadDecimal(); break;
                        case "Boolean": obj = br.ReadBoolean(); break;
                        case "Byte": obj = br.ReadByte(); break;
                        case "Char": obj = br.ReadChar(); break;
                        case "Byte[]":
                            int length = br.ReadInt32();
                            if (length > 0)
                                obj = br.ReadBytes(length);
                            break;
                    }
                }
                
                factEntity.PropertyValues[i] = obj;
                if (FieldFlag == byte.MaxValue)
                {
                    break;//属性已经读取完成，不再处理当前实体的其它字段
                }
            }
        }

        /// <summary>
        /// 反序列化实体类
        /// </summary>
        /// <param name="buffer">要反序列化的数据源</param>
        /// <param name="factEntityType">实体类的实际类型</param>
        /// <returns>实体类实例</returns>
        public static object BinaryDeserialize(byte[]  buffer, Type factEntityType)
        {
            MemoryStream ms2 = new MemoryStream(buffer);
            BinaryReader br = new BinaryReader(ms2);
            EntityFields ef = EntityFieldsCache.Item(factEntityType);
            EntityBase entity = (EntityBase)System.Activator.CreateInstance(factEntityType);

            BinaryDeserializeCommon(br, ef, entity);
           
            br.Close();
            ms2.Close();
            return entity;
        }


    
    }


    /// <summary>
    /// PDF.NET 实体类序列化器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PdfNetSerialize<T> where T: EntityBase,new()
    {
       

        public enum SerializeFormates
        { 
            Binary,
            XML,
            Json
        }
        /// <summary>
        /// 二进制序列化一个实体类
        /// </summary>
        /// <param name="entity">实体对象实例</param>
        /// <returns>字节数组</returns>
        public static byte[] BinarySerialize(T entity)
        {
            return PdfNetSerialize.BinarySerialize(entity);
        }

        /// <summary>
        /// 二进制序列化一个实体类数组
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public static byte[] BinarySerialize(T[] entitys)
        {
            List<byte[]> list = new List<byte[]>(); 
            foreach (T entity in entitys)
            {
                list.Add(BinarySerialize(entity));
            }

            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write(PdfNetSerialize.ENTITY_ARRAY_FLAG);//写入标记

            foreach (byte[] item in list)
            {
                bw.Write(item);
            }
            bw.Close();
            ms.Close();
            return ms.ToArray();
        }

        /// <summary>
        /// 反序列化一个二进制实体类
        /// </summary>
        /// <param name="buffer">二进制实体类字节流</param>
        /// <returns>实体类</returns>
        public static T BinaryDeserialize(byte[] buffer )
        {
            MemoryStream ms2 = new MemoryStream(buffer);
            BinaryReader br = new BinaryReader(ms2);
            EntityFields ef = EntityFieldsCache.Item(typeof(T));

            T entity=BinaryDeserializeObjectInner(br, ef);
            br.Close ();
            ms2.Close ();
            return entity;
        }

        private static T BinaryDeserializeObjectInner( BinaryReader br, EntityFields ef)
        {
            T entity = new T();
            PdfNetSerialize.BinaryDeserializeCommon(br, ef, entity);
            return entity;
        }

        /// <summary>
        /// 反序列化一个二进制实体类数组
        /// </summary>
        /// <param name="buffer">二进制字节缓存</param>
        /// <returns>实体类数组</returns>
        public static T[] BinaryDeserializeArray(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer);
            BinaryReader br = new BinaryReader(ms);
            EntityFields ef = EntityFieldsCache.Item(typeof(T));
            int flag = br.ReadInt32();
            if (flag != PdfNetSerialize.ENTITY_ARRAY_FLAG)
                throw new Exception("反序列化错误：不是有效的实体类数组格式！");

            long length=(long)buffer.Length ;
            List<T> list = new List<T>();
            while (ms.Position < length)
            {
                T entity = BinaryDeserializeObjectInner( br, ef);
                list.Add(entity);
            }
            return list.ToArray();
        }
    
    }
}
