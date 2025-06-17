using API.Controllers.UserManagement;
using API.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.Dto.SystemSetup;
using Service;
using Service.SystemSetup.Interface;
using Service.Transaction.Interface;
using Service.Dto.Transaction;
using Microsoft.EntityFrameworkCore;
using Service.SystemSetup;
using Service.Cache;
using EF.Models;
using Service.Helpers;
using System.Data;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace API.Controllers.Transaction
{
    [Route("api/[controller]")]
    [ApiController]
    public class PpmpController : ControllerBase
    {
        private readonly IPpmpService _ppmpService;
        private readonly CachedItems _cachedItems;

        private readonly ILogger<PpmpController> _logger;

        public PpmpController(IPpmpService ppmpService, ILogger<PpmpController> logger, CachedItems cachedItems)
        {
            _cachedItems = cachedItems;
            _ppmpService = ppmpService;
            _logger = logger;
        }

        #region CRUD
        // GET: api/ppmps
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<VPpmpindex>>> GetAll([FromQuery] PagingParameters pagingParameters)
        {
            try
            {
                pagingParameters.ApplyFilterCriteria = true;
                pagingParameters.ParentModule = 1;

                var paginatedResult = await _ppmpService.GetComplexPagedAndFilteredAsync<VPpmpindex>(pagingParameters);

                var response = new PaginatedResponse<VPpmpindex>
                {
                    Items = paginatedResult.Data,
                    TotalCount = paginatedResult.TotalRecords,
                    PageNumber = pagingParameters.PageNumber,
                    PageSize = pagingParameters.PageSize
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/ppmps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PPMPDto>> GetById(int id)
        {
            try
            {
                var model = await _ppmpService.GetByIdAsync(id);

                if (model == null)
                {
                    return NotFound();
                }

                return model;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
            }

            return BadRequest();
        }

        // POST: api/ppmps        
        [HttpPost]
        public async Task<ActionResult<PPMPDto>> Post(PPMPDto model)
        {
            try
            {
                await _ppmpService.AddAsync(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return BadRequest(new { message = ex.Message });
            }

            return CreatedAtAction("GetById", new { id = model.Id }, model);
        }

        // PUT: api/ppmps/5        
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, PPMPDto model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            try
            {
                await _ppmpService.UpdateAsync(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return NoContent();
        }

        //// DELETE: api/ppmps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _ppmpService.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return NoContent();
        }

        #endregion

        #region Reports
        [HttpGet("printpreview/{id}")]
        public async Task<ActionResult> GetPPMPReport(int id)
        {
            try
            {
                //return await _ppmpService.GenerateReport(id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return BadRequest();
        }

        [HttpGet("export/{id}")]
        public async Task<ActionResult> ExportExcel(int id)
        {
            try
            {
                //await _ppmpService.GenerateReport(id);
                var data = await _ppmpService.GetByIdAsync(id);
                // actual data from ppmp(data)
                if (data == null)
                {
                    return NotFound();
                }

                IWorkbook workbook = new XSSFWorkbook();

                // Create a DataTable for the PPMP data
                var ppmpTable = new DataTable("PPMP");
                ppmpTable.Columns.Add("BudgetYear");
                ppmpTable.Columns.Add("");
                ppmpTable.Columns.Add("PPMPNumber");
                ppmpTable.Columns.Add("");
                ppmpTable.Columns.Add("RequestingOffice");

                ppmpTable.Rows.Add("BudgetYear", data.BudgetYear);
                ppmpTable.Rows.Add("PPMPNumber", data.Ppmpno);
                ppmpTable.Rows.Add("RequestingOffice", data.RequestingOffice.Name);
                ppmpTable.Rows.Add("PreparedBy", data.CreatedBy);
                ppmpTable.Rows.Add("CreatedDate", data.CreatedDateStr);
                //ppmpTable.Rows.Add("BudgetYear", data.BudgetYear, "", "CatalogueAmount", data.CatalogueAmount);
                //ppmpTable.Rows.Add("PPMPNumber", data.Ppmpno, "", "SupplementaryAmount", data.SupplementaryAmount);
                //ppmpTable.Rows.Add("RequestingOffice", data.RequestingOffice, "", "ProjectAmount", data.ProjectAmount);
                //ppmpTable.Rows.Add("PreparedBy", data.CreatedBy.ToString(), "", "TotalAmount", data.TotalAmount);
                //ppmpTable.Rows.Add("", "", "", "AdditionalInflationValue", data.AdditionalInflationValue);
                //ppmpTable.Rows.Add("", "", "", "GrandTotalAmount", data.GrandTotalAmount);
                ////ppmpTable.Rows.Add("", "", "", "AdditionalInflationValue", data.AdditionalInflationValue);

                //data.BudgetYear,
                //    data.Ppmpno,
                //    "test",
                //    "test",
                //    data.CatalogueAmount,
                //    data.SupplementaryAmount,
                //    data.ProjectAmount,
                //    data.TotalAmount,
                //    data.AdditionalTenPercent,
                //    data.GrandTotalAmount
                //);

                // Create a DataTable for the PSDBMCatalogue data
                var psdbmTable = new DataTable("PSDBMCatalogue");
                psdbmTable.Columns.Add("ItemCode");
                psdbmTable.Columns.Add("Category");
                psdbmTable.Columns.Add("AccountCode");
                psdbmTable.Columns.Add("UnitOfMeasure");
                psdbmTable.Columns.Add("Description");
                psdbmTable.Columns.Add("UnitPrice", typeof(decimal));
                psdbmTable.Columns.Add("1stQtr", typeof(int));
                psdbmTable.Columns.Add("2ndQtr", typeof(int));
                psdbmTable.Columns.Add("3rdQtr", typeof(int));
                psdbmTable.Columns.Add("4thQtr", typeof(int));
                psdbmTable.Columns.Add("Total", typeof(int));
                psdbmTable.Columns.Add("Amount", typeof(decimal));
                psdbmTable.Columns.Add("Remarks");

                data.Ppmpcatalogues.ToList().ForEach(f =>
                psdbmTable.Rows.Add(
                    f.Catalogue.Code,
                    f.Catalogue.MajorCategoryName,
                    f.Catalogue.AccountCode,
                    f.Catalogue.UnitOfMeasurementCode,
                    f.Description,
                    f.UnitPrice,
                    f.FirstQuarter,
                    f.SecondQuarter,
                    f.ThirdQuarter,
                    f.FourthQuarter,
                    f.FirstQuarter + f.SecondQuarter + f.ThirdQuarter + f.FourthQuarter,
                    f.Amount,
                    f.Remarks
                ));

                // Create a DataTable for the Project data
                var supplementaryTable = new DataTable("Supplementary");
                supplementaryTable.Columns.Add("ItemCode");
                supplementaryTable.Columns.Add("Category");
                supplementaryTable.Columns.Add("AccountCode");
                supplementaryTable.Columns.Add("UnitOfMeasure");
                supplementaryTable.Columns.Add("Description");
                supplementaryTable.Columns.Add("UnitPrice", typeof(decimal));
                supplementaryTable.Columns.Add("1stQtr", typeof(int));
                supplementaryTable.Columns.Add("2ndQtr", typeof(int));
                supplementaryTable.Columns.Add("3rdQtr", typeof(int));
                supplementaryTable.Columns.Add("4thQtr", typeof(int));
                supplementaryTable.Columns.Add("Total", typeof(int));
                supplementaryTable.Columns.Add("Amount", typeof(decimal));
                supplementaryTable.Columns.Add("Remarks");

                data.Ppmpsupplementaries.ToList().ForEach(f =>
                supplementaryTable.Rows.Add(
                    f.Supplementary.Code,
                    f.Supplementary.MajorCategoryName,
                    f.Supplementary.AccountCode,
                    f.Supplementary.UnitOfMeasurementCode,
                    f.Description,
                    f.UnitPrice,
                    f.FirstQuarter,
                    f.SecondQuarter,
                    f.ThirdQuarter,
                    f.FourthQuarter,
                    f.FirstQuarter + f.SecondQuarter + f.ThirdQuarter + f.FourthQuarter,
                    f.Amount,
                    f.Remarks
                ));

                // Create a DataTable for the Supplementary data
                var projectTable = new DataTable("Projects");
                projectTable.Columns.Add("AccountCode");
                projectTable.Columns.Add("ProjectName");
                projectTable.Columns.Add("Description");
                projectTable.Columns.Add("Quarter", typeof(int));
                projectTable.Columns.Add("Cost", typeof(decimal));

                data.PpmpProjects.ToList().ForEach(f =>
                projectTable.Rows.Add(
                    f.AccountCode.Code,
                    f.ProjectName,
                    f.Description,
                    f.Quarter,
                    f.Cost
                ));

                var sheetsData = new Dictionary<string, DataTable>
                {
                    //{ "PPMP", ppmpTable },
                    { "PSDBMCatalogue", psdbmTable },
                    { "Supplementary", supplementaryTable },
                    { "Project", projectTable }
                };

                var ppmpSheet = ExcelHelper.CreateSideBySideSheet(workbook, data, "PPMP");
                var itemSheets = ExcelHelper.CreateExcelSheets(workbook, sheetsData);

                var fileContents = await ExcelHelper.GenerateExcelAsync(workbook);
                return File(fileContents,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"PPMP_{DateTime.Now.ToString("MMddyyyy_hhmmss")}.xlsx");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return BadRequest();
        }
        #endregion

        [HttpGet("budgetyears")]
        public async Task<ActionResult<IList<int>>> GetBudgetYears()
        {
            try
            {
                var result = _ppmpService.GetBudgetYears();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return BadRequest();
        }
    }
}
