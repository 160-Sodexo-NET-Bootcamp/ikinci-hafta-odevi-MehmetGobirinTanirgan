﻿using Data.DataModels;
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var allContainers = await unitOfWork.Containers.GetAllAsync();

                if (allContainers is null)
                {
                    return NoContent();
                }
                return Ok(allContainers);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddContainer([FromBody] ContainerCreateDto reqContainer)
        {
            // Manuel mapping
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

        [HttpPut]
        public async Task<IActionResult> UpdateContainer([FromBody] ContainerUpdateDto reqContainer)
        {
            try
            {
                var existingContainer = await unitOfWork.Containers.GetByIdAsync((long)reqContainer.Id);
                if (existingContainer == null)
                {
                    return BadRequest(new { Message = "Container doesn't exist." });
                }

                existingContainer.ContainerName = reqContainer.ContainerName ?? existingContainer.ContainerName;
                existingContainer.Latitude = reqContainer.Latitude != default ? reqContainer.Latitude : existingContainer.Latitude;
                existingContainer.Longitude = reqContainer.Longitude != default ? reqContainer.Longitude : existingContainer.Longitude;

                unitOfWork.Containers.Update(existingContainer);
                await unitOfWork.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContainer([FromRoute] long id)
        {
            if (id <= 0)
            {
                return BadRequest(new { Message = "Invalid parameter." });
            }

            try
            {
                await unitOfWork.Containers.DeleteAsync(id);
                await unitOfWork.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{vehicleId}")]
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

                if (containersOfVehicle is null)
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

        [HttpGet("{vehicleId}/{n}")]
        public async Task<IActionResult> GetClusteredContainersOfVehicle([FromRoute] long vehicleId, int n)
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

                if (containersOfVehicle is null)
                {
                    return NoContent();
                }

                if (n > containerCt / 2)
                {
                    return BadRequest(new { Message = "Number of clusters cannot be higher then " + containerCt / 2 });
                }

                var responseList = containersOfVehicle.ToClusters<Container>(n);
                return Ok(responseList);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}