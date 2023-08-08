﻿/*
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
 * 修改者：         时间：
 * 修改说明：
 * ========================================================================
 */

using System.Collections;

namespace PWMIS.Windows.Validate
{
    /// <summary>
    ///     RegexStatic 的摘要说明。
    /// </summary>
    public class RegexStatic
    {
        public static Hashtable GetGenerateRegex()
        {
            var ht = new Hashtable();
            ht.Add("电话号码", @"(\d{3}-\d{8}|\d{4}-\d{8}|\d{4}-\d{7})\b");
            ht.Add("电话号码或手机", @"(\d{3}-\d{8}|\d{4}-\d{8}|\d{4}-\d{7}|\d{11})\b");
            ht.Add("手机", @"(\d{11})\b");
            ht.Add("邮政编码", @"\d{6}");
            ht.Add("身份证 15位|18位", @"(\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$");
            ht.Add("日期",
                @"(?:(?:1[6-9]|[2-9]\d)?\d{2}[\/\-\.](?:0?[1,3-9]|1[0-2])[\/\-\.](?:29|30))(?: (?:0?\d|1\d|2[0-3])\:(?:0?\d|[1-5]\d)\:(?:0?\d|[1-5]\d)(?: \d{1,3})?)?$|^(?:(?:1[6-9]|[2-9]\d)?\d{2}[\/\-\.](?:0?[1,3,5,7,8]|1[02])[\/\-\.]31)(?: (?:0?\d|1\d|2[0-3])\:(?:0?\d|[1-5]\d)\:(?:0?\d|[1-5]\d)(?: \d{1,3})?)?$|^(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])[\/\-\.]0?2[\/\-\.]29)(?: (?:0?\d|1\d|2[0-3])\:(?:0?\d|[1-5]\d)\:(?:0?\d|[1-5]\d)(?: \d{1,3})?)?$|^(?:(?:16|[2468][048]|[3579][26])00[\/\-\.]0?2[\/\-\.]29)(?: (?:0?\d|1\d|2[0-3])\:(?:0?\d|[1-5]\d)\:(?:0?\d|[1-5]\d)(?: \d{1,3})?)?$|^(?:(?:1[6-9]|[2-9]\d)?\d{2}[\/\-\.](?:0?[1-9]|1[0-2])[\/\-\.](?:0?[1-9]|1\d|2[0-8]))(?: (?:0?\d|1\d|2[0-3])\:(?:0?\d|[1-5]\d)\:(?:0?\d|[1-5]\d)(?: \d{1,3})?)?");
            ht.Add("Email", @"[\w-]+@[\w-]+(\.(\w)+)*(\.(\w){2,3})");
            ht.Add("URL", @"[a-zA-z]+:\/\/[^\s]*");
            ht.Add("IPV4", @"(\d+)\.(\d+)\.(\d+)\.(\d+)");
            ht.Add("数字", @"-?(\d)+\.?(\d)*");
            ht.Add("整数", @"-?\d+");
            ht.Add("浮点数", @"(-?\d+)(\.\d+)?");
            ht.Add("字母", @"[A-Za-z]+");
            ht.Add("大写字母", @"[A-Z]+");
            ht.Add("小写字母", @"[a-z]+");
            ht.Add("中文字符", @"[\u4e00-\u9fa5](\s*[\u4e00-\u9fa5])*$");

            return ht;
        }
    }
}