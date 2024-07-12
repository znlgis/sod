using Microsoft.AspNetCore.Mvc;
using SimpleDemo.Interface.IServices;
using SimpleDemo.Interface;
using SimpleDemo.Model.Dtos;
using SimpleDemo.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimpleWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentController : ControllerBase
    {
        private readonly IEquipmentService _service;

        public EquipmentController(IEquipmentService equipmentService)
        {
            _service = equipmentService;
        }

        // GET: api/<EquipmentController>
        [HttpGet]
        public IEnumerable<IEquipment> Get()
        {
            return _service.GetAll();
        }

        // GET api/<EquipmentController>/5
        [HttpGet("{id}")]
        public IEquipment Get(int id)
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
        [HttpGet("PageList/{pageSize}/{pageNum}/{total}")]
        public PageResult<EquipmentDto> GetPageList(int pageSize, int pageNum, int total, string filter = "null")
        {
            return _service.GetPageList(pageSize, pageNum, total, filter);
        }


        // POST api/<EquipmentController>
        [HttpPost]
        public int Post([FromBody] EquipmentDto value)
        {
            if (_service.Insert(value))
                return value.ID;
            else
                return 0;
        }

        // PUT api/<EquipmentController>/5
        [HttpPut("{id}")]
        public int Put(int id, [FromBody] EquipmentDto value)
        {
            if (_service.Update(value))
                return id;
            else
                return 0;
        }

        // DELETE api/<EquipmentController>/5
        [HttpDelete("{id}")]
        public int Delete(int id)
        {
            if (_service.Delete(id))
            return id;
            else
                //    return 0;
                throw new Exception("server error:ID不存在");
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="value"></param>
        /// <returns>操作受影响的行数</returns>
        [HttpPost("BatchDelete")]
        public int BatchDelete([FromBody] int[] value)
        {
            return _service.BatchDelete(value);
        }
    }
}
