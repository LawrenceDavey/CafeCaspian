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
                Cost = 0.50m,
                Type = CafeCaspian.Enums.ProductType.Drink,
                IsHot = false
            },
            new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Coffee",
                Cost = 1.00m,
                Type = CafeCaspian.Enums.ProductType.Drink,
                IsHot = true
            },
            new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Cheese Sandwich",
                Cost = 2.00m,
                Type = CafeCaspian.Enums.ProductType.Food,
                IsHot = false
            },
            new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Steak Sandwich",
                Cost = 4.50m,
                Type = CafeCaspian.Enums.ProductType.Food,
                IsHot = true
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
            Assert.Equal(3.85m, totalBill);
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
            Assert.Equal(2.20m, totalBill);
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
            
            // Assert
            var ex = Assert.Throws<ProductNotFoundException>(() => billService.GetTotalBill(mockProducts));
            Assert.Equal("Invalid products provided", ex.Message);
        }

        [Fact]
        public void GetBillTotal_NoServiceChargeForDrinks()
        {
            // Arrange
            var mockProductItems = new List<Product>()
            {
                new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = "Cola",
                    Cost = 0.50m,
                    Type = CafeCaspian.Enums.ProductType.Drink
                },
                new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = "Coffee",
                    Cost = 1.00m,
                    Type = CafeCaspian.Enums.ProductType.Drink
                }
            };

            var mockProducts = new List<string>()
            {
                "Cola",
                "Coffee"
            };

            var mockProductRepo = new Mock<IProductRepository>();
            foreach (var item in mockProducts)
            {
                mockProductRepo.Setup(x => x.GetByName(item)).Returns(PRODUCTS.Find(p => p.Name == item));
            }

            var billService = new BillService(mockProductRepo.Object);

            // Act
            var serviceCharge = billService.GetServiceCharge(mockProductItems);

            var totalBill = billService.GetTotalBill(mockProducts);

            // Assert
            Assert.Equal(1.5m, totalBill);
            Assert.Equal(0, serviceCharge);
        }

        [Fact]
        public void GetBillTotal_ServiceChargeForFood()
        {
            // Arrange
            var mockProductItems = new List<Product>()
            {
                new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = "Cola",
                    Cost = 0.50m,
                    Type = CafeCaspian.Enums.ProductType.Drink
                },
                new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = "Cheese Sandwich",
                    Cost = 2.00m,
                    Type = CafeCaspian.Enums.ProductType.Food
                }
            };

            var mockProducts = new List<string>()
            {
                "Cola",
                "Cheese Sandwich"
            };

            var mockProductRepo = new Mock<IProductRepository>();
            foreach (var item in mockProducts)
            {
                mockProductRepo.Setup(x => x.GetByName(item)).Returns(PRODUCTS.Find(p => p.Name == item));
            }

            var billService = new BillService(mockProductRepo.Object);

            // Act
            var serviceCharge = billService.GetServiceCharge(mockProductItems);

            var totalBill = billService.GetTotalBill(mockProducts);

            // Assert
            Assert.Equal(2.75m, totalBill);
            Assert.Equal(0.10m, serviceCharge);
        }

        [Fact]
        public void GetBillTotal_ServiceChargeForHotFood()
        {
            // Arrange
            var mockProductItems = new List<Product>()
            {
                new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = "Cola",
                    Cost = 0.50m,
                    Type = CafeCaspian.Enums.ProductType.Drink,
                    IsHot = false
                },
                new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = "Steak Sandwich",
                    Cost = 2.00m,
                    Type = CafeCaspian.Enums.ProductType.Food,
                    IsHot = true
                }
            };

            var mockProducts = new List<string>()
            {
                "Cola",
                "Steak Sandwich"
            };

            var mockProductRepo = new Mock<IProductRepository>();
            foreach (var item in mockProducts)
            {
                mockProductRepo.Setup(x => x.GetByName(item)).Returns(PRODUCTS.Find(p => p.Name == item));
            }

            var billService = new BillService(mockProductRepo.Object);

            // Act
            var serviceCharge = billService.GetServiceCharge(mockProductItems);

            var totalBill = billService.GetTotalBill(mockProducts);

            // Assert
            Assert.Equal(6.00m, totalBill);
            Assert.Equal(0.20m, serviceCharge);
        }

        [Fact]
        public void GetBillTotal_ServiceChargeMax()
        {
            // Arrange
            var mockProductItems = new List<Product>()
            {
                new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = "Cola",
                    Cost = 100.50m,
                    Type = CafeCaspian.Enums.ProductType.Drink,
                    IsHot = false
                },
                new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = "Steak Sandwich",
                    Cost = 200.00m,
                    Type = CafeCaspian.Enums.ProductType.Food,
                    IsHot = true
                }
            };

            var mockProducts = new List<string>()
            {
                "Cola",
                "Steak Sandwich"
            };

            var mockProductRepo = new Mock<IProductRepository>();
            foreach (var item in mockProducts)
            {
                mockProductRepo.Setup(x => x.GetByName(item)).Returns(PRODUCTS.Find(p => p.Name == item));
            }

            var billService = new BillService(mockProductRepo.Object);
            var productCost = 500.00m;
            var productItems = new List<Product>();

            // Act
            var serviceCharge = billService.GetServiceCharge(mockProductItems);
            var maxServiceCharge = billService.GetMaximumServiceCharge(productCost, serviceCharge);

            // Assert
            Assert.Equal(20.00m, maxServiceCharge);
        }

        [Fact]
        public void GetProductTotal()
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
            var totalBill = billService.GetProductTotal(mockProducts, PRODUCTS);

            // Assert
            Assert.Equal(3.50m, totalBill);
        }
    }
}
