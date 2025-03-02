using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DxReportingWeb.Services
{
    public class NwindDataService
    {
        public async Task<IEnumerable<Customer>> GetCustomersAsync(CancellationToken ct = default)
        {
            // Simulasikan data untuk keperluan testing
            var customers = new List<Customer>
            {
                new Customer { Price = 100, Article = "Product A", Code = "A001" },
                new Customer { Price = 200, Article = "Product B", Code = "B002" },
                new Customer { Price = 150, Article = "Product C", Code = "C003" }
            };

            // Simulasi penundaan async
            await Task.Delay(100, ct);

            return customers;
        }

        public class Customer
        {
            public int Price { get; set; }
            public string? Article { get; set; }
            public string? Code { get; set; }
        }


    }

}
