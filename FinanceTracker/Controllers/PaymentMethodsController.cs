using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        public PaymentMethodsController(Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
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
        public async Task<IActionResult> Create([FromBody] PaymentMethod method)
        {
            var response = await _supabaseClient.From<PaymentMethod>().Insert(method);
            var createdMethod = response.Models.First();

            // Map to DTO
            var dto = new PaymentMethodDto
            {
                Id = createdMethod.Id,
                MethodName = createdMethod.MethodName,
                CreatedBy = createdMethod.CreatedBy,
                CreatedAt = createdMethod.CreatedAt,
                UpdatedBy = createdMethod.UpdatedBy,
                UpdatedAt = createdMethod.UpdatedAt,
                IsDeleted = createdMethod.IsDeleted
            };

            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PaymentMethod method)
        {
            method.Id = id;
            var response = await _supabaseClient.From<PaymentMethod>().Update(method);
            var updatedMethod = response.Models.First();

            // Map to DTO
            var dto = new PaymentMethodDto
            {
                Id = updatedMethod.Id,
                MethodName = updatedMethod.MethodName,
                CreatedBy = updatedMethod.CreatedBy,
                CreatedAt = updatedMethod.CreatedAt,
                UpdatedBy = updatedMethod.UpdatedBy,
                UpdatedAt = updatedMethod.UpdatedAt,
                IsDeleted = updatedMethod.IsDeleted
            };

            return Ok(dto);
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
    public class PaymentMethod 
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
}
