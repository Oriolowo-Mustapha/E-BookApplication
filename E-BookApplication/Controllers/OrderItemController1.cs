using AutoMapper;
using E_BookApplication.Contract.Service;
using E_BookApplication.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_BookApplication.Controllers
{
    [Authorize]
    public class OrderItemsController : Controller
    {
        private readonly IOrderItemService _orderItemService;
        private readonly IMapper _mapper;

        public OrderItemsController(IOrderItemService orderItemService, IMapper mapper)
        {
            _orderItemService = orderItemService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> ByOrder(Guid orderId)
        {
            var orderItems = await _orderItemService.GetOrderItemsByOrderIdAsync(orderId);
            var orderItemDtos = _mapper.Map<IEnumerable<OrderItemDTO>>(orderItems);
            return View(orderItemDtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Guid orderId, decimal unitPrice, OrderItemCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var orderItem = await _orderItemService.CreateOrderItemAsync(orderId, dto.BookId, dto.Quantity, unitPrice);
            var orderItemDto = _mapper.Map<OrderItemDTO>(orderItem);

            return Json(orderItemDto);
        }


    
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(Guid orderItemId, int newQuantity)
        {
            if (newQuantity < 1)
                return BadRequest("Quantity must be at least 1");

            try
            {
                var updatedItem = await _orderItemService.UpdateQuantityAsync(orderItemId, newQuantity);
                var dto = _mapper.Map<OrderItemDTO>(updatedItem);
                return Json(dto);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        
        [HttpPost]
        public async Task<IActionResult> Delete(Guid orderItemId)
        {
            var success = await _orderItemService.RemoveOrderItemAsync(orderItemId);
            if (!success) return NotFound();

            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> CalculateTotal(Guid orderId)
        {
            var total = await _orderItemService.CalculateOrderTotalAsync(orderId);
            return Json(new { Total = total });
        }
    }

}
