﻿using Microsoft.AspNetCore.Mvc;
using Pekkish.PointOfSale.Api.Services;
using Pekkish.PointOfSale.Wati;
using Pekkish.PointOfSale.Wati.Models.Dtos;
using System.Runtime.CompilerServices;

namespace Pekkish.PointOfSale.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatiController : Controller
    {        
        private readonly IPointOfSaleService _pointOfSaleService;  
        private readonly IWatiService _watiService;

        public WatiController(IPointOfSaleService pointOfSaleService, IWatiService watiService)
        { 
            _pointOfSaleService= pointOfSaleService;            
            _watiService= watiService;
        }
        
        [HttpPost("SessionMessageReceive")]
        public async Task<IActionResult> SessionMessageReceive([FromBody] SessionMessageReceiveDto dto)
        {
            try
            {
                await _watiService.MessageReceive(dto);
                              
                return Ok();    
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("VendorList")]
        private async Task<IActionResult> VendorList()
        {
            try
            {
                var result = await _pointOfSaleService.VendorList();

                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
