using Data.DataModels;
using Data.Uow.Abstract;
using Microsoft.AspNetCore.Mvc;
using SwcsAPI.Dtos;
using System;
using System.Threading.Tasks;

namespace SwcsAPI.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public VehicleController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVehicles()
        {
            try
            {
                var allVehicles = await unitOfWork.Vehicles.GetAllAsync();
                if (allVehicles is null)
                {
                    return NoContent();
                }

                return Ok(allVehicles);
            }
            catch (Exception ex)
            {

                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddVehicle([FromBody] VehicleCreateDto reqVehicle)
        { 
            // Manuel mapping
            var newVehicle = new Vehicle
            {
                VehicleName = reqVehicle.VehicleName,
                VehiclePlate = reqVehicle.VehiclePlate
            };

            try
            {
                await unitOfWork.Vehicles.AddAsync(newVehicle);
                await unitOfWork.SaveAsync();
                return Ok(reqVehicle);
            }
            catch (Exception ex)
            {

                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateVehicle([FromBody] VehicleUpdateDto reqVehicle)
        {
            try
            {
                var existingVehicle = await unitOfWork.Vehicles.GetByIdAsync((long)reqVehicle.Id);

                if (existingVehicle is null)
                {
                    return BadRequest("Vehicle does not exist.");
                }
               
                existingVehicle.VehicleName = reqVehicle.VehicleName ?? existingVehicle.VehicleName;
                existingVehicle.VehiclePlate = reqVehicle.VehiclePlate ?? existingVehicle.VehiclePlate;
                unitOfWork.Vehicles.Update(existingVehicle);
                await unitOfWork.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")] // Hard delete
        public async Task<IActionResult> DeleteVehicle([FromRoute] long id)
        {
            if (id <= 0)
            {
                return BadRequest(new { Message = "Invalid parameter." });
            }

            try
            {
                await unitOfWork.Vehicles.DeleteAsync(id);
                await unitOfWork.Containers.DeleteRangeByExpressionAsync(x => x.VehicleId == id);
                await unitOfWork.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(new { Message = ex.Message });
            }
        }


    }
}
