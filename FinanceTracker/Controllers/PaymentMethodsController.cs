using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc;
using Supabase;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace FinanceTracker.Controllers
{
    [Route("api/[controller]")]
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
            var response = await _supabaseClient.From<PaymentMethod>()
                                                .Filter("id", Supabase.Postgrest.Constants.Operator.Equals, id)
                                                .Single();
            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentMethod method)
        {
            var response = await _supabaseClient.From<PaymentMethod>().Insert(method);
            return CreatedAtAction(nameof(GetAll), new { id = response.Models.First().Id }, response.Models.First());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PaymentMethod method)
        {
            method.Id = id;
            var response = await _supabaseClient.From<PaymentMethod>().Update(method);
            return Ok(response.Models.First());
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
}
