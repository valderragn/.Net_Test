using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTest.Data;
using NetTest.Models;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NetTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Index page visited at {DT}", DateTime.UtcNow);
            var dataRecords = await _context.DataRecords
                                    .OrderByDescending(r => r.DataId)
                                    .ToListAsync();
            return View(dataRecords);
        }

        public IActionResult Create()
        {
            _logger.LogInformation("Create page visited at {DT}", DateTime.UtcNow);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DataName,DataDesc,InquiryDate,InquiryUser")] DataRecord dataRecord, IFormFile? DataImage)
        {
            if (!ModelState.IsValid)
            {
                // Log all validation errors
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        _logger.LogError($"Property: {state.Key}, Error: {error.ErrorMessage}");
                    }
                }
                return View(dataRecord);
            }

            dataRecord.InquiryDate = DateTime.Now;
            dataRecord.InquiryUser = "BaseTest";

            if (DataImage != null && DataImage.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await DataImage.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();
                    dataRecord.DataImage = Convert.ToBase64String(fileBytes);
                }
            }

            _context.Add(dataRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataRecord = await _context.DataRecords.FindAsync(id);
            if (dataRecord == null)
            {
                return NotFound();
            }
            _logger.LogInformation("Edit Id page visited at {DT}", DateTime.UtcNow);
            return View(dataRecord);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int DataId, string DataName, string DataDesc, IFormFile? DataImage)
        {
            var record = _context.DataRecords.Find(DataId);
            if (record != null)
            {
                record.DataName = DataName;
                record.DataDesc = DataDesc;
                record.UpdateDate = DateTime.Now;
                record.UpdateUser = "BaseTest";

                _logger.LogInformation("Data Image is : " + DataImage);
                if (DataImage != null && DataImage.Length > 0)
                {
                    
                    using (var ms = new MemoryStream())
                    {
                        await DataImage.CopyToAsync(ms);
                        var fileBytes = ms.ToArray();
                        record.DataImage = Convert.ToBase64String(fileBytes);
                    }
                }

                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return NotFound();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int DataId)
        {
            var record = _context.DataRecords.Find(DataId);
            if (record != null)
            {
                _context.DataRecords.Remove(record);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home"); // Redirect to the Index action of the Home controller
            }
            return NotFound();
        }


        private bool DataRecordExists(int id)
        {
            return _context.DataRecords.Any(e => e.DataId == id);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
