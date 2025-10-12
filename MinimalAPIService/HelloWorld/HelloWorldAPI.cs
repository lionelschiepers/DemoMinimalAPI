using MediatR;
using System.Text.Json;

namespace MinimalAPIService.HelloWorld
{
    public static class HelloWorldAPI
    {
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        public sealed class GetRequest : IRequest<Response>
        {
            public static readonly GetRequest Empty = new GetRequest();
        }

        public sealed class Response
        {
            public string? Message { get; set; }
        }

        public static void Register(WebApplication app)
        {
            app.MapGet("/helloworld", async (IRequestHandler<GetRequest, Response> handler, CancellationToken cancellationToken) =>
            {
                var response = await handler.Handle(GetRequest.Empty, cancellationToken);
                return TypedResults.Json(response, _jsonOptions);
            })
            .Produces<Response>(200);
        }

        public sealed class Handler()
            : IRequestHandler<GetRequest, Response>
        {
            public async Task<Response> Handle(GetRequest content, CancellationToken cancellationToken)
            {
                return new Response()
                {
                    Message = "Hello, World!"
                };
            }
        }
    }
}
