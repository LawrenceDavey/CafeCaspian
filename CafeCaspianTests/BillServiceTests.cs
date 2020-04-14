using CafeCaspian.Models;
using CafeCaspian.Repositories;
using CafeCaspian.Repositories.Interfaces;
using CafeCaspian.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace CafeCaspianTests
{
    public class BillServiceTests
    {
        private List<Product> PRODUCTS = new List<Product>()
        {
            new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Cola",
                Cost = 0.50m
            },
            new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Coffee",
                Cost = 1.00m
            },
            new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Cheese Sandwich",
                Cost = 2.00m
            },
            new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Steak Sandwich",
                Cost = 4.50m
            }
        };

        [Fact]
        public void GetTotalBill_Success()
        {
            // Arrange
            var mockProducts = new List<string>()
            {
                "Cola",
                "Coffee",
                "Cheese Sandwich"
            };

            var mockProductRepo = new Mock<IProductRepository>();
            foreach (var item in mockProducts)
            {
                mockProductRepo.Setup(x => x.GetByName(item)).Returns(PRODUCTS.Find(p => p.Name == item));
            }

            var billService = new BillService(mockProductRepo.Object);

            // Act
            var totalBill = billService.GetTotalBill(mockProducts);

            // Assert
            Assert.Equal(3.5m, totalBill);
        }

        [Fact]
        public void GetTotalBill_SingleItem_Success()
        {
            // Arrange
            var mockProducts = new List<string>()
            {
                "Cheese Sandwich"
            };

            var mockProductRepo = new Mock<IProductRepository>();
            foreach (var item in mockProducts)
            {
                mockProductRepo.Setup(x => x.GetByName(item)).Returns(PRODUCTS.Find(p => p.Name == item));
            }

            var billService = new BillService(mockProductRepo.Object);

            // Act
            var totalBill = billService.GetTotalBill(mockProducts);

            // Assert
            Assert.Equal(2.0m, totalBill);
        }

        [Fact]
        public void GetTotalBill_InvalidProduct()
        {
            // Arrange
            var mockProducts = new List<string>()
            {
                "Cola",
                "Coffee",
                "Ham Sandwich"
            };

            var mockProductRepo = new Mock<IProductRepository>();
            foreach (var item in mockProducts)
            {
                mockProductRepo.Setup(x => x.GetByName(item)).Returns(PRODUCTS.Find(p => p.Name == item));
            }

            var billService = new BillService(mockProductRepo.Object);

            // Act
            var totalBill = billService.GetTotalBill(mockProducts);

            // Assert
            var ex = Assert.Throws<ValidationException>(() => billService.GetTotalBill(mockProducts));
            Assert.Equal("Invalid product provided", ex.Message);
        }
    }
}