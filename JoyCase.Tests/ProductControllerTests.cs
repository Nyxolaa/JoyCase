using JoyCase.Application.Product.Command.CreateProductCommand;
using JoyCase.Application.Product.Command.DeleteProductCommand;
using JoyCase.Application.Product.Command.UpdateProductCommand;
using JoyCase.Application.Product.Query.GetProductByIdQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using JoyCase.Application.Product.Dto;

namespace JoyCase.Tests
{
    public class ProductControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ProductController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnProduct()
        {
            var productId = 1;
            var productDto = new ProductDto { ProductId = 1, ProductName = "Test Product", Price = 10.0m };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), default))
                         .ReturnsAsync(productDto);

            var result = await _controller.GetProductById(new GetProductByIdQuery { Id = productId });

            // ActionResult<ProductDto> yerine doğrudan OkObjectResult ile kontrol yapıyoruz
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProductDto>(okResult.Value);

            Assert.Equal(productDto.ProductId, returnValue.ProductId);
            Assert.Equal(productDto.ProductName, returnValue.ProductName);
            Assert.Equal(productDto.Price, returnValue.Price);
        }


        [Fact]
        public async Task CreateProduct_ShouldReturnCreated()
        {
            var command = new CreateProductCommand { Name = "New Product", Price = 200, CategoryId = 1 };
            var productId = 10;

            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(productId);
            var result = await _controller.CreateProduct(command);

            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, actionResult.StatusCode);
            Assert.Equal(productId, Convert.ToInt64(actionResult.Value));
        }

        [Fact]
        public async Task UpdateProduct_WhenSuccess_ShouldReturnOk()
        {
            var command = new UpdateProductCommand { Id = 1, Name = "Updated Product" };

            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);
            var result = await _controller.UpdateProduct(command);

            var actionResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, actionResult.StatusCode);
        }

        [Fact]
        public async Task UpdateProduct_WhenNotFound_ShouldReturnNotFound()
        {
            var command = new UpdateProductCommand { Id = 1, Name = "Updated Product" };

            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(false);
            var result = await _controller.UpdateProduct(command);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_WhenSuccess_ShouldReturnOk()
        {
            var request = new DeleteProductCommand { Id = 1 };

            _mediatorMock.Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var result = await _controller.DeleteProduct(request);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_WhenNotFound_ShouldReturnNotFound()
        {
            var request = new DeleteProductCommand { Id = 1 };
            _mediatorMock.Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(false);
            var result = await _controller.DeleteProduct(request);
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
