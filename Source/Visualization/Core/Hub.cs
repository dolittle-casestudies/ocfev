using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dolittle.Collections;
using Dolittle.Logging;
using Dolittle.Reflection;
using Dolittle.Serialization.Json;
using Dolittle.Strings;
using Microsoft.AspNetCore.Http;

namespace Web
{

    /// <summary>
    /// 
    /// </summary>
    public class Hub
    {
        const string PING = "ping";
        const string PONG = "pong";

        readonly ConcurrentQueue<MethodCall> _methodsToSendToClient;
        readonly List<WebSocket> _webSockets = new List<WebSocket>();
        protected readonly ILogger _logger;
        readonly ISerializer _serializer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serializer"></param>
        public Hub(ILogger logger, ISerializer serializer)
        {
            // Idea: Make the system rely on virtual methods - we override these methods at runtime and call into the client, so you don't have to have the explicit call
            _methodsToSendToClient = new ConcurrentQueue<MethodCall>();

            _logger = logger;
            _serializer = serializer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="webSocket"></param>
        /// <returns></returns>
        public async Task Run(HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            var receivedDataBuffer = new System.ArraySegment<byte>(buffer);

            var result = await webSocket.ReceiveAsync(receivedDataBuffer, CancellationToken.None);
            lock(_webSockets) _webSockets.Add(webSocket);

            while (!result.CloseStatus.HasValue)
            {
                CleanupConnections();

                receivedDataBuffer = new ArraySegment<byte>(buffer);
                result = await webSocket.ReceiveAsync(receivedDataBuffer, CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text && result.EndOfMessage)
                {
                    var payload = receivedDataBuffer.Array.Where(b => b != 0).ToArray();
                    var receivedString = System.Text.Encoding.UTF8.GetString(payload, 0, payload.Length);
                    if (receivedString == PING)
                    {
                        this.Ping();
                    }
                    else
                    {
                        try
                        {
                            var methodCall = _serializer.FromJson<MethodCall>(receivedString);
                            var methodName = methodCall.Method.ToPascalCase();
                            var method = GetType().GetMethod(methodName);
                            if (method != null)
                            {
                                try
                                {
                                    method.Invoke(this, methodCall.Arguments);
                                }
                                catch (Exception ex)
                                {
                                    _logger.Error(ex, $"Mismatched method signature - '{receivedString}'");
                                }
                            }
                            else
                            {
                                _logger.Information("Unknown method");

                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(ex, $"Unsupported payload - '{receivedString}'");
                        }
                    }
                }
            }

            lock(_webSockets) _webSockets.Remove(webSocket);

            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        public void Invoke(Expression<Action> p)
        {
            var methodInfo = p.GetMethodInfo();
            var call = new MethodCall
            {
                Method = methodInfo.Name.ToCamelCase(),
                Arguments = p.GetMethodArguments()
            };

            _logger.Information($"Add '{call.Method}' for call to the client");

            var json = _serializer.ToJson(call, SerializationOptions.CamelCase);
            Send(json, "Invoked");

            //_methodsToSendToClient.Enqueue(call);
        }

        void Send(string payload, string logMessage)
        {
            var bytes = Encoding.UTF8.GetBytes(payload);
            lock(_webSockets)
            {
                _webSockets.ForEach(webSocket =>
                {
                    webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None).ContinueWith(o =>
                    {
                        _logger.Information(logMessage);
                    });

                });
            }

        }

        void Ping()
        {
            Send(PONG, "ping - pong");
        }

        void CleanupConnections()
        {
            lock(_webSockets)
            {
                var socketsToRemove = _webSockets.Where(
                    webSocket =>
                    (webSocket.State != WebSocketState.Open && webSocket.State != WebSocketState.Connecting) ||
                    webSocket.CloseStatus.HasValue
                );

                socketsToRemove.ForEach(webSocket => _webSockets.Remove(webSocket));
            }
        }
    }

}