using EStockMarketCompanyService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EStockMarketCompanyService.Services.Interfaces
{
    public interface ICompanyService
    {
        Task CreateCompany(CompanyRegistrationRequest request);

        Task<GetCompanyResponse> GetCompanies();

        Task<GetCompanyResponse> GetCompanybyCode(string companyCode);

        Task DeleteCompany(string companyCode);
    }
}
