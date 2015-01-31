/*
 * PDF.NET 数据开发框架
 * http://www.pwmis.com/sqlmap
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperMarketModel;
using SuperMarketDAL.Entitys;
using PWMIS.DataMap.Entity;
using PWMIS.Core.Extensions;

namespace SuperMarketBLL
{
    public class CashierManageBIZ
    {
        /// <summary>
        /// 已经指派的收银员
        /// </summary>
        private static List<Cashier> AssignedCashier = new List<Cashier>();

        /// <summary>
        /// 正在工作中的收银员
        /// </summary>
        public static List<Cashier> WorkingCashier
        {
            get { return AssignedCashier; }
        }
        /// <summary>
        /// 指派收银员到某个收银台（使用某个收银机）
        /// </summary>
        /// <param name="cashRegisterNo">收银机编号</param>
        /// <param name="cashier">收银员</param>
        /// <returns></returns>
        public bool AssignCashier(string cashRegisterNo,Cashier cashier)
        {
            //如果该收银机编号已经被使用，则不能再分配
            foreach (Cashier c in AssignedCashier)
            {
                if (c.UsingCashierRegister.CashRegisterNo  == cashRegisterNo)
                    return false;
            }
            CashierRegisterMachines manchines = new CashierRegisterMachines() { CashRegisterNo = cashRegisterNo };
            cashier.UsingCashierRegister = manchines;
            AssignedCashier.Add(cashier);
            return true;
        }

        /// <summary>
        /// 测试当前收银员是否已经分派到收银台
        /// </summary>
        /// <param name="cashier"></param>
        /// <returns></returns>
        public bool TestAssignedCashier(Cashier cashier)
        {
            foreach (Cashier c in AssignedCashier)
            {
                if (c.WorkNumber == cashier.WorkNumber)
                {
                    cashier.UsingCashierRegister = c.UsingCashierRegister;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取所有的收银员
        /// </summary>
        /// <returns></returns>
        public List<Cashier> GetAllCashiers()
        {
            Employee emp = new Employee();
            emp.JobName = "收银员";
            OQL q = OQL.From(emp)
                .Select(emp.WorkNumber,emp.EmployeeName)
                .Where(emp.JobName)
                .OrderBy(emp.EmployeeName, "asc")
                .END;
            //List<Employee> list= EntityQuery<Employee>.QueryList(q);
            List<Employee> list = q.ToList<Employee>();//使用OQL扩展
            return list.ConvertAll<Cashier>(p =>
            {
                return new Cashier() { CashierName = p.EmployeeName, WorkNumber = p.WorkNumber };
            }
            ); 
        }

        /// <summary>
        /// 获取所有的收银机
        /// </summary>
        /// <returns></returns>
        public List<CashierRegisterMachines> GetAllCashierRegisterMachines()
        {
            Facility fac = new Facility() { FacilityName = "收银机" };
            OQL q = OQL.From(fac).Select(fac.SerialNumber).Where(fac.FacilityName).END;
            List<Facility> list = EntityQuery<Facility>.QueryList(q);
            return list.ConvertAll<CashierRegisterMachines>(p =>
                {
                    return new CashierRegisterMachines() { CashRegisterNo = p.SerialNumber };
                }
            );
        }
    }
}
