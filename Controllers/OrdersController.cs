using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DutchTreat.Data.Entities;
using DutchTreat.Interfaces;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OrdersController :ControllerBase
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;

        public OrdersController(
            IDutchRepository repository, 
            ILogger<OrdersController> logger,
            IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<OrderViewModel>> Get()
        {
            try
            {
                var result = _repository.GetAllOrders();

                return Ok(_mapper.Map<IEnumerable<OrderViewModel>>(result));
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get all orders: {e}");
                return BadRequest("Failed to get all orders.");
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public ActionResult<OrderViewModel> GetOrderById(int id)
        {
            try
            {
                var order = _repository.GetOrderById(id);

                if (order != null)
                {
                    return Ok(_mapper.Map<OrderViewModel>(order));
                }

                return NotFound($"Could not find order with id: {id}");
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get order: {e}");
                return BadRequest("Failed to get order.");
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult Post([FromBody] OrderViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newOrder = _mapper.Map<Order>(model);

                    if (newOrder.OrderDate == DateTime.MinValue)
                    {
                        newOrder.OrderDate = DateTime.Now;
                    }

                    _repository.AddEntity(newOrder);

                    if (_repository.SaveAll())
                    {
                        return Created($"/api/orders/{newOrder.Id}", _mapper.Map<OrderViewModel>(newOrder));
                    }
                }

                

                
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to save the order: {e}");
            }

            return BadRequest("Failed to save the order.");
        }
    }
}
