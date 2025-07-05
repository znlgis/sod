using Sod.Pwmis.com.Common;
using Sod.Pwmis.com.DTO;
using Sod.Pwmis.com.Interface.IServices;
using Sod.Pwmis.com.Interface;
//@ using Sod.Pwmis.com.Model;
//@ using Microsoft.AspNetCore.Mvc;
using SODCodeGen.Web.Mock;
using System.Collections.Generic;

namespace Sod.Pwmis.com.API.Controllers
{
    /// <summary>
    /// 轮巡策略管理接口
    /// </summary>
    //@ [Route("api/[controller]")]
    //@ [ApiController]
    public class SodDemoController : ControllerBase
    {
        private readonly ISodDemoService _service;

        public SodDemoController(ISodDemoService service)
        {
            _service = service;
        }

        // GET: api/<SodDemoController>
        //@ [HttpGet]
        public IEnumerable<ISodDemo> Get()
        {
            return _service.GetAll();
        }


        // GET api/<SodDemoController>/5
        //@ [HttpGet("{id}")]
        public ISodDemo Get(int id)
        {
            return _service.GetById(id);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageNum">页码，从1开始</param>
        /// <param name="typeId">设备类型</param>
        /// <param name="filter">过滤条件，格式为 key1$value1,key2$value2 的字符串，其中key为实体对象属性名</param>
        /// <returns></returns>
        //@ [HttpGet("PageList/{pageSize}/{pageNum}/{total}")]
        public PageResult<SodDemoDto> GetPageList(int pageSize, int pageNum, int total, string filter = "null")
        {
            return _service.GetPageList(pageSize, pageNum, total, filter);
        }


        // POST api/<SodDemoController>
        //@ [HttpPost]
        //@ [UnitOfWork]
        public int Post(/*[FromBody]*/ SodDemoDto value)
        {
            if (_service.Insert(value))
                return value.ID;
            else
                return 0;
        }

        // PUT api/<SodDemoController>/5
        //@ [HttpPut("{id}")]
        public int Put(int id, /*[FromBody]*/ SodDemoDto value)
        {
            if (_service.Update(value))
                return id;
            else
                return 0;
        }

        // DELETE api/<SodDemoController>/5
        //@ [HttpDelete("{id}")]
        public int Delete(int id)
        {
            if (_service.Delete(id))
                return id;
            else
                return 0;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="value"></param>
        /// <returns>操作受影响的行数</returns>
        //@ [HttpPost("BatchDelete")]
        public int BatchDelete(/*[FromBody]*/ int[] value)
        {
            return _service.BatchDelete(value);
        }


    }
}
