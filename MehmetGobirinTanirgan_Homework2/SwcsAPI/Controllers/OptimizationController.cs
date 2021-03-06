using Data.Uow.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SwcsAPI.Extensions;
using System;
using System.Threading.Tasks;

namespace SwcsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OptimizationController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public OptimizationController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("{vehicleId}/{n}")]// K Means algoritmasına göre n adet kümeye ayırma
        public async Task<IActionResult> GetOptimizedClusters([FromRoute] long vehicleId, [FromRoute] int n)
        {
            if (vehicleId <= 0 || n <= 0)
            {
                return BadRequest(new { Message = "Invalid parameter." });
            }

            try
            {
                var containersOfVehicle = await unitOfWork.Containers.
                   GetListByExpression(x => x.VehicleId == vehicleId).ToListAsync();
                var containerCt = containersOfVehicle.Count;          

                if (containerCt == 0)
                {
                    return NoContent();
                }

                if (n > containerCt / 2) // Bu sınırı ben koydum. Kayıt sayısının yarısını geçmesini istemedim.
                {
                    return BadRequest(new { Message = "Number of clusters cannot be higher then " + containerCt / 2 });
                }

                if (n == 1)
                {
                    return Ok(containersOfVehicle);
                }

                var responseList = containersOfVehicle.ToKMeansClusters(n);
                return Ok(responseList);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
