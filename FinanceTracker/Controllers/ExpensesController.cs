//using AutoMapper;
//using Microsoft.AspNetCore.Mvc;
//using Supabase;
//using FinanceTracker.Domain.Models;
//using static Supabase.Postgrest.Constants.Operator;
//using static Supabase.Postgrest.Constants;
//using Supabase.Postgrest.Models;
//using Supabase.Postgrest.Exceptions;
//using System.Net;

//namespace FinanceTracker.Controllers
//{
//    [ApiController]
//    [Route("[controller]")]
//    public class ExpensesController : ControllerBase
//    {
//        private readonly Client _supabaseClient;
//        private readonly IMapper _mapper;

//        public ExpensesController(Client supabaseClient, IMapper mapper)
//        {
//            _supabaseClient = supabaseClient;
//            _mapper = mapper;
//        }

//        // GET /expenses?startDate=2025-01-01&endDate=2025-03-01&paymentMethodId=2&page=1&pageSize=10
//        [HttpGet]
//        public async Task<IActionResult> GetAll(
//            [FromQuery] DateTime? startDate,
//            [FromQuery] DateTime? endDate,
//            [FromQuery] int? paymentMethodId,
//            [FromQuery] int page = 1,
//            [FromQuery] int pageSize = 10
//        )
//        {
//            // Use string operators (e.g. "eq", "gte", "lte") 
//            var query = _supabaseClient.From<Expense>()
//                .Filter("isDeleted", Operator.Equals, false);

//            if (startDate.HasValue)
//                query = query.Filter("date", Operator.GreaterThanOrEqual, startDate.Value);

//            if (endDate.HasValue)
//                query = query.Filter("date", Operator.LessThanOrEqual, endDate.Value);

//            if (paymentMethodId.HasValue)
//                query = query.Filter("payment_method_id", Operator.Equals, paymentMethodId.Value);

//            // We'll do two queries to get total count & paged data
//            // Query 1: get all matching items for total count
//            var allResponse = await query.Get();
//            var totalCount = allResponse.Models.Count;

//            // Query 2: get the paginated slice
//            // (Supabase .Range() is zero-based for both from/to)
//            var fromIndex = (page - 1) * pageSize;
//            var toIndex = fromIndex + pageSize - 1;

//            var pageResponse = await query
//                .Order("date", Ordering.Descending)
//                .Range(fromIndex, toIndex)
//                .Get();

//            var data = pageResponse.Models
//                .Select(e => _mapper.Map<ExpenseDto>(e))
//                .ToList();

//            return Ok(new
//            {
//                Page = page,
//                PageSize = pageSize,
//                TotalCount = totalCount,
//                Data = data
//            });
//        }

//        // GET /expenses/{id}
//        [HttpGet("{id:long}")]
//        public async Task<IActionResult> GetById(long id)
//        {
//            var response = await _supabaseClient
//                .From<Expense>()
//                .Filter("id", Operator.Equals, id)
//                .Single();

//            return response == null
//                ? NotFound()
//                : Ok(_mapper.Map<ExpenseDto>(response));
//        }

//        // POST /expenses
//        [HttpPost]
//        public async Task<IActionResult> Create([FromBody] CreateExpenseDto dto)
//        {
//            var model = _mapper.Map<Expense>(dto);
//            model.Date = dto.Date ?? DateTime.UtcNow;
//            model.CreatedAt = DateTime.UtcNow;

//            var insertResp = await _supabaseClient.From<Expense>().Insert(model);
//            var created = insertResp.Models.FirstOrDefault();
//            if (created == null)
//                return BadRequest("Failed to create expense.");

//            var createdDto = _mapper.Map<ExpenseDto>(created);
//            return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, createdDto);
//        }

//        // PUT /expenses/{id}
//        [HttpPut("{id:long}")]
//        public async Task<IActionResult> Update(long id, [FromBody] UpdateExpenseDto dto)
//        {
//            var existing = await _supabaseClient
//                .From<Expense>()
//                .Filter("id", Operator.Equals, id)
//                .Single();

//            if (existing == null)
//                return NotFound();

//            _mapper.Map(dto, existing);
//            existing.UpdatedAt = DateTime.UtcNow;

//            var updateResp = await _supabaseClient.From<Expense>().Update(existing);
//            var updated = updateResp.Models.FirstOrDefault();
//            if (updated == null)
//                return BadRequest("Failed to update expense.");

//            return Ok(_mapper.Map<ExpenseDto>(updated));
//        }

//        // DELETE /expenses/{id}
//        [HttpDelete("{id:long}")]
//        public async Task<IActionResult> Delete(long id)
//        {
//            var existing = await _supabaseClient
//                .From<Expense>()
//                .Filter("id", Operator.Equals, id)
//                .Single();

//            if (existing == null)
//                return NotFound();

//            // Hard delete:
//            await _supabaseClient.From<Expense>().Delete(existing);

//            // For soft delete, do:
//            // existing.IsDeleted = true; 
//            // existing.UpdatedAt = DateTime.UtcNow;
//            // await _supabaseClient.From<Expense>().Update(existing);

//            return NoContent();
//        }

//        // GET /expenses/recent?count=5
//        [HttpGet("recent")]
//        public async Task<IActionResult> GetRecent([FromQuery] int count = 5)
//        {
//            var response = await _supabaseClient
//                .From<Expense>()
//                .Filter("isDeleted", Operator.Equals, false)
//                .Order("date", Ordering.Descending)
//                .Range(0, count - 1)
//                .Get();

//            var list = response.Models
//                .Select(e => _mapper.Map<ExpenseDto>(e))
//                .ToList();

//            return Ok(list);
//        }

//        // GET /expenses/summary
//        [HttpGet("summary")]
//        public async Task<IActionResult> GetSummary()
//        {
//            // Example stub. If amounts live in `expense_item`, sum them separately.
//            var allExpenses = await _supabaseClient
//                .From<Expense>()
//                .Filter("isDeleted", Operator.Equals, false)
//                .Get();

//            decimal totalExpenses = 0; // Real logic might sum from expense_item.

//            return Ok(new
//            {
//                TotalIncome = 0,
//                TotalExpenses = totalExpenses,
//                Balance = -totalExpenses
//            });
//        }
//    }
//}
