using System;
using System.Collections.Generic;
using System.Linq;
using Task1.DoNotChange;

namespace Task1
{
    public static class LinqTask
    {
        public static IEnumerable<Customer> Linq1(IEnumerable<Customer> customers, decimal limit)
        {
            return customers
                .Where(c => c.Orders.Sum(o => o.Total) > limit);
        }


        public static IEnumerable<(Customer customer, IEnumerable<Supplier> suppliers)> Linq2(
             IEnumerable<Customer> customers,
             IEnumerable<Supplier> suppliers)
        {
            return customers
                .Select(c => (
                    customer: c,
                    suppliers: suppliers.Where(s => s.City == c.City && s.Country == c.Country)
                ));
        }

        public static IEnumerable<(Customer customer, IEnumerable<Supplier> suppliers)> Linq2UsingGroup(
            IEnumerable<Customer> customers,
            IEnumerable<Supplier> suppliers)
        {
            return customers.GroupJoin(suppliers,
                c => new { c.City, c.Country },
                s => new { s.City, s.Country },
                (c, suppGroup) => (customer: c, suppliers: suppGroup)
            );
        }


        public static IEnumerable<Customer> Linq3(IEnumerable<Customer> customers, decimal limit)
        {
            return customers.Where(customer => customer.Orders.Sum(order => order.Total) >= limit);
        }

        public static IEnumerable<(Customer customer, DateTime dateOfEntry)> Linq4(
             IEnumerable<Customer> customers)
        {
            return customers
                .Where(c => c.Orders.Any())
                .Select(c => (customer: c, dateOfEntry: c.Orders.Min(o => o.OrderDate)));
        }


        public static IEnumerable<(Customer customer, DateTime dateOfEntry)> Linq5(
             IEnumerable<Customer> customers)
        {
            return Linq4(customers)
                .OrderBy(c => c.dateOfEntry.Year)
                .ThenBy(c => c.dateOfEntry.Month)
                .ThenByDescending(c => c.customer.Orders.Sum(o => o.Total))
                .ThenBy(c => c.customer.CompanyName);
        }


        public static IEnumerable<Customer> Linq6(IEnumerable<Customer> customers)
        {
            return customers
                .Where(c => !c.PostalCode.All(char.IsDigit) ||
                            string.IsNullOrWhiteSpace(c.Region) ||
                            !c.Phone.Contains("("));
        }


        public static IEnumerable<Linq7CategoryGroup> Linq7(IEnumerable<Product> products)
        {
            return products
                .GroupBy(p => p.Category)
                .Select(g => new Linq7CategoryGroup
                {
                    Category = g.Key,
                    UnitsInStockGroup = g.GroupBy(p => p.UnitsInStock)
                        .Select(ug => new Linq7UnitsInStockGroup
                        {
                            UnitsInStock = ug.Key,
                            Prices = ug.Select(p => p.UnitPrice).OrderBy(p => p)
                        })
                });
        }


        public static IEnumerable<(string category, List<Product> products)> Linq8(IEnumerable<Product> products, decimal cheap, decimal middle, decimal expensive)
        {
            return products.GroupBy(p => p.UnitPrice <= cheap ? "cheap" :
                                         p.UnitPrice <= middle ? "middle" :
                                         p.UnitPrice <= expensive ? "expensive" : "luxury")
                           .Select(g => (g.Key, g.ToList()));
        }

        public static IEnumerable<(string city, int averageIncome, int averageIntensity)> Linq9(IEnumerable<Customer> customers)
        {
            return customers.GroupBy(c => c.City)
                            .Select(g => (
                                city: g.Key,
                                averageIncome: (int)g.Average(c => c.Orders.Sum(o => o.Total)),
                                averageIntensity: (int)g.Average(c => c.Orders.Length)
                            ));
        }

        public static string Linq10(IEnumerable<Supplier> suppliers)
        {
            return string.Join("", suppliers.Select(s => s.Country).Distinct());
        }
    }
}