using Data.DataModels;
using Data.Uow.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SwcsAPI.Dtos;
using SwcsAPI.Extensions;
using System;
using System.Threading.Tasks;

namespace SwcsAPI.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class ContainerController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public ContainerController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]// Tüm container'ları getir
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var allContainers = await unitOfWork.Containers.GetAll().ToListAsync();

                if (allContainers.Count == 0)
                {
                    return NoContent();
                }
                return Ok(allContainers); // Ödevde tüm actionlarda sonuçları incelemek amaçlı model döndürdüm ancak Client'ın
                                          // ihtiyacına yönelik bir dto oluşturup gönderebiliriz. 
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost]// Container ekleme
        public async Task<IActionResult> AddContainer([FromBody] ContainerCreateDto reqContainer)
        {
            // Manuel mapping ile iki farklı türü birbirine çevirme
            var newContainer = new Container
            {
                ContainerName = reqContainer.ContainerName,
                Latitude = reqContainer.Latitude,
                Longitude = reqContainer.Longitude,
                VehicleId = reqContainer.VehicleId
            };

            try
            {
                await unitOfWork.Containers.AddAsync(newContainer);
                await unitOfWork.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut]// Container güncelleme
        public async Task<IActionResult> UpdateContainer([FromBody] ContainerUpdateDto reqContainer)
        {
            try
            {
                var existingContainer = await unitOfWork.Containers.GetByIdAsync(reqContainer.Id);
                if (existingContainer is null)
                {
                    return BadRequest(new { Message = "Container doesn't exist." });
                }

                //Manuel mapping
                existingContainer.ContainerName = reqContainer.ContainerName;
                existingContainer.Latitude = reqContainer.Latitude;
                existingContainer.Longitude = reqContainer.Longitude;

                unitOfWork.Containers.Update(existingContainer);
                await unitOfWork.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]// Container silme
        public async Task<IActionResult> DeleteContainer([FromRoute] long id)
        {
            if (id <= 0)
            {
                return BadRequest(new { Message = "Invalid parameter." });
            }

            try
            {
                unitOfWork.Containers.Delete(id);
                await unitOfWork.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{vehicleId}")]// Gelen vehicle id'ye ait container'ları getirme
        public async Task<IActionResult> GetContainersOfVehicle([FromRoute] long vehicleId)
        {
            if (vehicleId <= 0)
            {
                return BadRequest(new { Message = "Invalid parameter." });
            }

            try
            {
                var containersOfVehicle = await unitOfWork.Containers.
                    GetListByExpression(x => x.VehicleId == vehicleId).ToListAsync();

                if (containersOfVehicle.Count == 0)
                {
                    return NoContent();
                }
                return Ok(containersOfVehicle);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{vehicleId}/{n}")]// Gelen vehicle id'ye ait container'ları n adet eşit büyüklükte kümeye ayırma
        public async Task<IActionResult> GetClusteredContainersOfVehicle([FromRoute] long vehicleId, [FromRoute] int n)
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

                if (n > containerCt / 2)
                {
                    return BadRequest(new { Message = "Number of clusters cannot be higher then " + containerCt / 2 });
                }

                var responseList = containersOfVehicle.ToClusters(n);
                return Ok(responseList);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
