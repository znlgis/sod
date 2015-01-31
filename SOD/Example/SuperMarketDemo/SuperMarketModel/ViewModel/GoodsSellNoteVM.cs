/*
 * PDF.NET 数据开发框架
 * http://www.pwmis.com/sqlmap
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarketModel.ViewModel
{
    public class GoodsSellNoteVM
    {
        public int NoteID { get; set; }
        public string CustomerName { get; set; }
        public string ManchinesNumber { get; set; }
        public string EmployeeName { get; set; }
        public string SalesType { get; set; }
        public DateTime SellDate { get; set; }
    }
}
