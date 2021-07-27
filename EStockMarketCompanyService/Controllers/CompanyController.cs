using EStockMarketCompanyService.Models;
using EStockMarketCompanyService.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EStockMarketCompanyService.Controllers
{
    [ApiController]
    [Route("api/v1.0/market/company")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterCompany(CompanyRegistrationRequest request)
        {
            try
            {
                await _companyService.CreateCompany(request);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex?.Message);
            }
        }

        [HttpDelete]
        [Route("delete/{companyCode}")]
        public async Task<IActionResult> DeleteCompany(string companyCode)
        {
            try
            {
                await _companyService.DeleteCompany(companyCode);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex?.Message);
            }
        }

        [HttpGet]
        [Route("info/{companyCode}")]
        public async Task<IActionResult> GetCompanybyCode(string companyCode)
        {
            try
            {
                var company = await _companyService.GetCompanybyCode(companyCode);

                return Ok(company);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex?.Message);
            }
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetCompanies()
        {
            try
            {
                var companies = await _companyService.GetCompanies();

                return Ok(companies);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex?.Message);
            }
        }
    }
}
