//using AutoMapper;
//using FinanceTracker.Domain.Models;
//using FinanceTracker.Infrastructure.DbContexts;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Supabase;
//using static Supabase.Postgrest.Constants;

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
//            var query = _supabaseClient.From<Expense>()
//                .Filter("isDeleted", Operator.Equals, false);

//            if (startDate.HasValue)
//                query = query.Filter("date", Operator.Gte, startDate.Value);

//            if (endDate.HasValue)
//                query = query.Filter("date", Operator.Lte, endDate.Value);

//            if (paymentMethodId.HasValue)
//                query = query.Filter("payment_method_id", Operator.Equals, paymentMethodId.Value);

//            // Supabase .Range() is zero-based for both "start" and "end" indexes
//            var fromIndex = (page - 1) * pageSize;
//            var toIndex = fromIndex + pageSize - 1;

//            // Apply ordering & pagination
//            var response = await query
//                .Order("date", Ordering.Descending)
//                .Range(fromIndex, toIndex)
//                .Get();

//            var totalCount = response.ContentRange?.Total ?? 0;

//            var data = response.Models
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
//            var expense = await _supabaseClient
//                .From<Expense>()
//                .Filter("id", Operator.Equals, id)
//                .Single();

//            if (expense == null)
//                return NotFound();

//            return Ok(_mapper.Map<ExpenseDto>(expense));
//        }

//        // POST /expenses
//        [HttpPost]
//        public async Task<IActionResult> Create([FromBody] CreateExpenseDto dto)
//        {
//            var model = _mapper.Map<Expense>(dto);

//            // If the incoming DTO doesn't specify date, default to now
//            model.Date = dto.Date ?? DateTime.UtcNow;
//            model.CreatedAt = DateTime.UtcNow;

//            var response = await _supabaseClient.From<Expense>().Insert(model);
//            var created = response.Models.FirstOrDefault();

//            if (created == null)
//                return BadRequest("Failed to create expense.");

//            var createdDto = _mapper.Map<ExpenseDto>(created);

//            return CreatedAtAction(
//                nameof(GetById),
//                new { id = createdDto.Id },
//                createdDto
//            );
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

//            // Map updates
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
//            // If you want a "soft delete," do this:
//            // existing.IsDeleted = true; existing.UpdatedAt = DateTime.UtcNow; ... then Update(existing)

//            var existing = await _supabaseClient
//                .From<Expense>()
//                .Filter("id", Operator.Equals, id)
//                .Single();

//            if (existing == null)
//                return NotFound();

//            // Hard delete from DB:
//            await _supabaseClient.From<Expense>().Delete(existing);

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
//            // This is a stub. If your actual amounts live in `expense_item`, 
//            // you'd do a separate query or a Postgres function call.
//            var allExpenses = await _supabaseClient
//                .From<Expense>()
//                .Filter("isDeleted", Operator.Equals, false)
//                .Get();

//            // If there's no direct "Amount" field here, 
//            // you might set totalExpenses = 0 or retrieve from `expense_item`.
//            decimal totalExpenses = 0;

//            // Return a sample object
//            return Ok(new
//            {
//                TotalIncome = 0,
//                TotalExpenses = totalExpenses,
//                Balance = -totalExpenses
//            });
//        }
//    }

//    public class ExpenseDto
//    {
//        public long Id { get; set; }
//        public Guid UserId { get; set; }
//        public DateTime Date { get; set; }
//        public string Description { get; set; }
//        public int? PaymentMethodId { get; set; }
//        public Guid? CreatedBy { get; set; }
//        public DateTime CreatedAt { get; set; }
//        public Guid? UpdatedBy { get; set; }
//        public DateTime? UpdatedAt { get; set; }
//        public bool IsDeleted { get; set; }

//        // Optional: If you want to include items in the read DTO
//        public List<ExpenseItemDto> ExpenseItems { get; set; }
//            = new List<ExpenseItemDto>();
//    }

//    public class CreateExpenseDto
//    {
//        public Guid UserId { get; set; }
//        public DateTime? Date { get; set; } // If null, default to DateTime.UtcNow in your service
//        public string Description { get; set; }
//        public int? PaymentMethodId { get; set; }
//        public Guid? CreatedBy { get; set; }
//        public bool IsDeleted { get; set; }

//        // Optional: If you want to create items together with the Expense
//        public List<CreateExpenseItemDto> ExpenseItems { get; set; }
//            = new List<CreateExpenseItemDto>();
//    }

//    public class UpdateExpenseDto
//    {
//        public DateTime? Date { get; set; }
//        public string Description { get; set; }
//        public int? PaymentMethodId { get; set; }
//        public Guid? UpdatedBy { get; set; }
//        public bool? IsDeleted { get; set; }

//        // If you allow updating ExpenseItems in the same call, include them
//        public List<UpdateExpenseItemDto> ExpenseItems { get; set; }
//            = new List<UpdateExpenseItemDto>();
//    }

//    // Full read DTO for returning to client
//    public class ExpenseItemDto
//    {
//        public long Id { get; set; }
//        public long ExpenseId { get; set; }
//        public long CategoryId { get; set; }
//        public decimal Amount { get; set; }
//        public string Description { get; set; }
//        public Guid? CreatedBy { get; set; }
//        public DateTime CreatedAt { get; set; }
//        public Guid? UpdatedBy { get; set; }
//        public DateTime? UpdatedAt { get; set; }
//        public bool IsDeleted { get; set; }

//        // If you want to expose the computed total:
//        public decimal Total => Amount;
//    }

//    public class CreateExpenseItemDto
//    {
//        public long CategoryId { get; set; }
//        public decimal Amount { get; set; }
//        public string Description { get; set; }
//        public Guid? CreatedBy { get; set; }
//        public bool IsDeleted { get; set; }
//    }

//    public class UpdateExpenseItemDto
//    {
//        public long? CategoryId { get; set; }
//        public decimal? Amount { get; set; }
//        public string Description { get; set; }
//        public Guid? UpdatedBy { get; set; }
//        public bool? IsDeleted { get; set; }
//    }

//    public class ExpenseCategoryDto
//    {
//        public long Id { get; set; }
//        public string Name { get; set; }
//        public string Description { get; set; }
//        public Guid? CreatedBy { get; set; }
//        public DateTime CreatedAt { get; set; }
//        public Guid? UpdatedBy { get; set; }
//        public DateTime? UpdatedAt { get; set; }
//        public bool IsDeleted { get; set; }

//        // Optionally, include items or subcategories if needed
//        public List<ExpenseItemDto> ExpenseItems { get; set; }
//            = new List<ExpenseItemDto>();
//    }

//    public class CreateExpenseCategoryDto
//    {
//        public string Name { get; set; }
//        public string Description { get; set; }
//        public Guid? CreatedBy { get; set; }
//        public bool IsDeleted { get; set; }
//    }

//    public class UpdateExpenseCategoryDto
//    {
//        public string Name { get; set; }
//        public string Description { get; set; }
//        public Guid? UpdatedBy { get; set; }
//        public bool? IsDeleted { get; set; }
//    }


//}
