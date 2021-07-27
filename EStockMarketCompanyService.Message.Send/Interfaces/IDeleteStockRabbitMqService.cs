using System;
using System.Collections.Generic;
using System.Text;

namespace EStockMarketCompanyService.Message.Send.Interfaces
{
    public interface IDeleteStockRabbitMqService
    {
        void SendDeleteCompany(string deleteCompanyCode);
    }
}
