/*
 SQL 的查询级别示例：
SELECT	FROM
	ORDER BY
-----------------
SELECT FROM
	JOIN ON
	WHERE
		ORDER BY
----------------
SELECT FROM
	JOIN ON
	WHERE
		GROUP BY
			HAVEING
				ORDER BY
 * 
 * 根据SQL查询级别，制定IOQL接口。
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace PWMIS.DataMap.Entity
{
    public interface IOQL
    {
        OQL1 Select(params object[] fields);
    }

    public interface IOQL1 : IOQL2
    {
        //OQL End { get; }
        //OQL3 GroupBy(object field);
        //OQL4 Having(object field);
        //OQL4 OrderBy(object field);
        OQL2 Where(params object[] fields);
    }

    public interface IOQL2 : IOQL3
    {
        //OQL End { get; }
        OQL3 GroupBy(object field);
        //OQL4 Having(object field);
        //OQL4 OrderBy(object field);
    }

    public interface IOQL3 : IOQL4
    {
        OQL4 Having(object field,object Value,string sqlFunctionFormat);
        //OQL End { get; }
        //OQL4 OrderBy(object field);
    }

    public interface IOQL4
    {
        OQL END { get; }
        OQLOrderType OrderBy(object field);
    }
}
