using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Supabase;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using static Supabase.Postgrest.Constants;

namespace FinanceTracker.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PaymentMethodsController : ControllerBase
    {
        private readonly Client _supabaseClient;
        private readonly IMapper _mapper;


        public PaymentMethodsController(Client supabaseClient, IMapper mapper)
        {
            _supabaseClient = supabaseClient;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _supabaseClient.From<PaymentMethod>().Get();

            // Map to DTOs
            var paymentMethods = response.Models.Select(pm => new PaymentMethodDto
            {
                Id = pm.Id,
                MethodName = pm.MethodName,
                CreatedBy = pm.CreatedBy,
                CreatedAt = pm.CreatedAt,
                UpdatedBy = pm.UpdatedBy,
                UpdatedAt = pm.UpdatedAt,
                IsDeleted = pm.IsDeleted
            }).ToList();

            return Ok(paymentMethods);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _supabaseClient
                .From<PaymentMethod>()
                .Filter("id", Operator.Equals, id)
                .Single();

            if (response == null)
                return NotFound();

            // Map to DTO
            var dto = new PaymentMethodDto
            {
                Id = response.Id,
                MethodName = response.MethodName,
                CreatedBy = response.CreatedBy,
                CreatedAt = response.CreatedAt,
                UpdatedBy = response.UpdatedBy,
                UpdatedAt = response.UpdatedAt,
                IsDeleted = response.IsDeleted
            };

            return Ok(dto); // Return DTO instead of raw model
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentMethodCreateDto dto)
        {
            // Map to model (CreatedAt is ignored)
            var paymentMethod = _mapper.Map<PaymentMethod>(dto);

            // Insert into Supabase (database sets CreatedAt)
            var response = await _supabaseClient.From<PaymentMethod>().Insert(paymentMethod);
            var createdMethod = response.Models.First();

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdMethod.Id },
                _mapper.Map<PaymentMethodDto>(createdMethod)
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PaymentMethodUpdateDto dto)
        {
            // Fetch existing entity
            var existingMethod = await _supabaseClient
                .From<PaymentMethod>()
                .Filter("id", Operator.Equals, id)
                .Single();

            if (existingMethod == null)
                return NotFound();

            // Map DTO properties to the existing model
            _mapper.Map(dto, existingMethod); // Automatically updates only allowed fields

            // Explicitly set UpdatedAt timestamp
            existingMethod.UpdatedAt = DateTime.UtcNow;

            // Save changes in Supabase
            var updateResponse = await _supabaseClient.From<PaymentMethod>().Update(existingMethod);
            var updatedMethod = updateResponse.Models.First();

            // Return updated DTO
            return Ok(_mapper.Map<PaymentMethodDto>(updatedMethod));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var method = await _supabaseClient.From<PaymentMethod>()
                                              .Filter("id", Supabase.Postgrest.Constants.Operator.Equals, id)
                                              .Single();
            if (method == null)
                return NotFound();

            await _supabaseClient.From<PaymentMethod>().Delete(method);

            return NoContent();
        }
    }

    [Table("payment_methods")]
    public class PaymentMethod : BaseModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }

        [Column("method_name")]
        public string MethodName { get; set; }

        [Column("created_by")]
        public Guid? CreatedBy { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_by")]
        public Guid? UpdatedBy { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("isDeleted")]
        public bool IsDeleted { get; set; }
    }

    public class PaymentMethodDto
    {
        public int Id { get; set; }
        public string MethodName { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class PaymentMethodCreateDto
    {
        public string MethodName { get; set; }
        public Guid? CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class PaymentMethodUpdateDto
    {
        public int Id { get; set; }
        public string MethodName { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
