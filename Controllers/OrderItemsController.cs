using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DutchTreat.Interfaces;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Controllers
{
    [Route("api/orders/{orderid}/items")]
    [ApiController]
    [Produces("application/json")]
    public class OrderItemsController : ControllerBase
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger<OrderItemsController> _logger;
        private readonly IMapper _mapper;

        public OrderItemsController(
            IDutchRepository repository,
            ILogger<OrderItemsController> logger,
            IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<OrderItemViewModel>> Get(int orderId)
        {
            try
            {
                var order = _repository.GetOrderById(orderId);

                if (order == null)
                {
                    return NotFound($"No order found with orderId: {orderId}");
                }

                if (order.Items.Count > 0)
                {
                    return Ok(_mapper.Map<IEnumerable<OrderItemViewModel>>(order.Items));
                }

                return NotFound("No items found for order.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get items for order: {e}");
                return BadRequest("Failed to get items for order.");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public ActionResult<OrderItemViewModel> Get(int orderId, int id)
        {
            try
            {
                var order = _repository.GetOrderById(orderId);

                if (order == null)
                {
                    return NotFound($"No order found with orderId: {orderId}");
                }

                var orderItem = order
                    .Items
                    .FirstOrDefault(i => i.Id == id);

                if (orderItem != null)
                {
                    return Ok(_mapper.Map<OrderItemViewModel>(orderItem));
                }

                return NotFound($"No item found with the id: {id}");
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get item: {e}");
                return BadRequest("Failed to get item.");
            }
        }

    }
}
