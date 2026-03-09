using Clientes.Api.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;
using Xunit;

namespace Clientes.Tests.Middleware;

public class ExceptionMiddlewareTests
{
    private readonly Mock<ILogger<ExceptionMiddleware>> _loggerMock;

    public ExceptionMiddlewareTests()
    {
        _loggerMock = new Mock<ILogger<ExceptionMiddleware>>();
    }

    private DefaultHttpContext CreateContext()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        return context;
    }

    private async Task<string> ReadResponseBody(DefaultHttpContext context)
    {
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        return await new StreamReader(context.Response.Body).ReadToEndAsync();
    }

    [Fact]
    public async Task InvokeAsync_DeveContinuarFluxo_QuandoNaoHaExcecao()
    {
        var context = CreateContext();

        var middleware = new ExceptionMiddleware(
            (innerHttpContext) => Task.CompletedTask,
            _loggerMock.Object
        );

        await middleware.InvokeAsync(context);

        Assert.Equal(200, context.Response.StatusCode == 0 ? 200 : context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_DeveRetornarBadRequest_QuandoArgumentException()
    {
        var context = CreateContext();

        var middleware = new ExceptionMiddleware(
            (innerHttpContext) => throw new ArgumentException("Erro de argumento"),
            _loggerMock.Object
        );

        await middleware.InvokeAsync(context);

        var body = await ReadResponseBody(context);
        var json = JsonDocument.Parse(body);

        Assert.Equal(400, context.Response.StatusCode);
        Assert.Equal("Erro de argumento", json.RootElement.GetProperty("message").GetString());
    }

    [Fact]
    public async Task InvokeAsync_DeveRetornarBadRequest_QuandoInvalidOperationException()
    {
        var context = CreateContext();

        var middleware = new ExceptionMiddleware(
            (innerHttpContext) => throw new InvalidOperationException("Operação inválida"),
            _loggerMock.Object
        );

        await middleware.InvokeAsync(context);

        var body = await ReadResponseBody(context);
        var json = JsonDocument.Parse(body);

        Assert.Equal(400, context.Response.StatusCode);
        Assert.Equal("Operação inválida", json.RootElement.GetProperty("message").GetString());
    }

    [Fact]
    public async Task InvokeAsync_DeveRetornarNotFound_QuandoKeyNotFoundException()
    {
        var context = CreateContext();

        var middleware = new ExceptionMiddleware(
            (innerHttpContext) => throw new KeyNotFoundException("Não encontrado"),
            _loggerMock.Object
        );

        await middleware.InvokeAsync(context);

        var body = await ReadResponseBody(context);
        var json = JsonDocument.Parse(body);

        Assert.Equal(404, context.Response.StatusCode);
        Assert.Equal("Não encontrado", json.RootElement.GetProperty("message").GetString());
    }

    [Fact]
    public async Task InvokeAsync_DeveRetornarInternalServerError_QuandoExcecaoGenerica()
    {
        var context = CreateContext();

        var middleware = new ExceptionMiddleware(
            (innerHttpContext) => throw new Exception("Erro inesperado"),
            _loggerMock.Object
        );

        await middleware.InvokeAsync(context);

        var body = await ReadResponseBody(context);
        var json = JsonDocument.Parse(body);

        Assert.Equal(500, context.Response.StatusCode);
        Assert.Equal("Erro inesperado", json.RootElement.GetProperty("message").GetString());
    }
}