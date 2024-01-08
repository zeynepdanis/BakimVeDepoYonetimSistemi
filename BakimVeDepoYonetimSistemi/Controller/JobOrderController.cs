
using System.Data.Entity.Infrastructure;
using BakimVeDepoYonetimSistemi.Model;
using BakimVeDepoYonetimSistemi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakimVeDepoYonetimSistemi.Controller
{
    
    [ApiController]
    public class JobOrderController : ControllerBase
    {

        private readonly JobOrderRepository _isEmriService;
        private readonly MaintenanceRepository _bakimTalepService;
        private readonly RepositoryContext _context;

        public JobOrderController(JobOrderRepository isEmriService, MaintenanceRepository bakimTalepService, RepositoryContext context)
        {
            _isEmriService = isEmriService;
            _bakimTalepService = bakimTalepService;
            _context = context;
        }

        [HttpPost("create-new-job-order")]
    public IActionResult AddIsEmriWithTrigger([FromBody] JobOrderRequest jobOrderRequest)
    {
       
        try
        {  
            var jobOrder= new JobOrder{
                
                VarlikId = _bakimTalepService.GetVarlikId((int)jobOrderRequest.BakimTalepId),
                OlusturulmaTarihi = DateTime.Now,
                BakimTalepId = jobOrderRequest.BakimTalepId,
                Aciklama = jobOrderRequest.Aciklama,
                IsEmriDurumId = 1,
            };
            

            _isEmriService.AddIsEmriWithTrigger(jobOrder,jobOrderRequest.CalisanEkipUyeleri);
            return Ok("new job order created");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Hata oluştu: {ex.Message}");
        }
    }

     [HttpGet("IsEmriSayisi")]
        public IActionResult GetIsEmriSayisi()
        { 
            var durumId = 3;

            var result = from ie in _context.IsEmri
                         where ie.IsEmriDurumId == durumId
                         group ie by ie.OlusturulmaTarihi into g
                         select new { OlusturulmaTarihi = g.Key, IsEmriSayisi = g.Count() };

            return Ok(result);
        }
        [HttpPut("UpdateIsEmriDurum/{isEmriId}")]
    public async Task<IActionResult> UpdateIsEmriDurumtoDone(int isEmriId)
    {
        var yeniId=_context.IsEmriDurumTable.FirstOrDefault(x=>x.Tanim=="Tamamlandı").IsEmriDurumId;
        var isEmri = await _context.IsEmri.FindAsync(isEmriId);

        if (isEmri == null)
        {
            return NotFound();
        }

        isEmri.IsEmriDurumId = yeniId;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            if (!IsEmriExists(isEmriId))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    private bool IsEmriExists(int id)
    {
        return _context.IsEmri.Any(e => e.IsEmriId == id);
    }

   
        [HttpGet("get-job-order-details")]
        public async Task<ActionResult<IEnumerable<object>>> GetIsEmriDurum()
        {
            var query = from isEmri in _context.IsEmri
                        join durum in _context.IsEmriDurumTable
                        on isEmri.IsEmriDurumId equals durum.IsEmriDurumId
                        select new
                        {
                            IsEmriId = isEmri.IsEmriId,
                            IsEmriAciklama = isEmri.Aciklama,
                            Durum = durum.Tanim
                        };

            return await query.ToListAsync();
        }
       

    

       
        
    }
}