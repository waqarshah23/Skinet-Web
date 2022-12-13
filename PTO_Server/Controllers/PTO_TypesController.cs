using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PTO_Server.Extensions.Logger;
using Core.Models;
using PTO_Server.Repository;

namespace PTO_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PTO_TypesController : ControllerBase
    {
        private readonly IRepository<PTO_Types> _Pto_Repo;
        private readonly ILoggerManager _logger;
        public PTO_TypesController(IRepository<PTO_Types> Pto_Repo, ILoggerManager logger)
        {
            _Pto_Repo = Pto_Repo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PTO_Types>>> GetPTO_TypesList()
        {
            try
            {
                var pto_typesList = await _Pto_Repo.GetListAsync();
                if (pto_typesList == null)
                {
                    _logger.LogWarning("Empty List Pto typed");
                    return NotFound("No PTO Types added");
                }

                _logger.LogInfo("Pto types List found");
                return Ok(pto_typesList);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PTO_Types>> GetPtoById(int id)
        {
            try
            {
                var Pto_Type = await _Pto_Repo.GetById(id);
                if (Pto_Type == null)
                {
                    _logger.LogWarning("pto type not found");
                    return NotFound("pto type not found");
                }
                _logger.LogInfo("Pto Type found");
                return Ok(Pto_Type);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddPtoType(PTO_Types ptoObj)
        {
            try
            {
                if(ptoObj == null)
                {
                    _logger.LogWarning("Invalid Client Request: empty Pto Param");
                    return BadRequest("Invalid Client Request: empty Pto Param");
                }

                var status = await _Pto_Repo.Add(ptoObj);
                return Ok(status);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdatePtoType(PTO_Types ptoObj)
        {
            try
            {
                var status = await _Pto_Repo.Update(ptoObj);
                return Ok(status);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePtoType(int id)
        {
            try
            {
                var status = await _Pto_Repo.Delete(id);
                return Ok(status);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }
    }
}
