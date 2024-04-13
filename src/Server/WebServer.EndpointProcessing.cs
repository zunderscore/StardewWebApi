using Microsoft.AspNetCore.Components.Routing;
using StardewValley;
using StardewWebApi.Game;
using System.Collections;
using System.Net;
using System.Reflection;

namespace StardewWebApi.Server;

internal partial class WebServer
{
    private static void ProcessEndpointRequest(MethodInfo endpoint, HttpListenerContext context)
    {
        if ((endpoint.GetCustomAttribute<RequireLoadedGameAttribute>() != null
            || endpoint.DeclaringType?.GetCustomAttribute<RequireLoadedGameAttribute>() != null)
            && !Game1.hasLoadedGame)
        {
            context.Response.BadRequest("No save loaded. Please load a save and try again.");
            return;
        }

        var controller = Activator.CreateInstance(endpoint.DeclaringType!);
        (controller as ApiControllerBase)!.HttpContext = context;

        if (TryPopulateEndpointParameters(endpoint, context, out var parameters))
        {
            endpoint.Invoke(controller, parameters);
        }
    }

    private static bool TryPopulateEndpointParameters(MethodInfo endpoint, HttpListenerContext context, out object?[]? outParams)
    {
        try
        {
            var sortedParams = new SortedDictionary<int, ParameterInfo>(
                endpoint.GetParameters().ToDictionary(p => p.Position)
            );

            var parameters = new List<object?>();

            if (sortedParams.Count > 0)
            {
                foreach (var param in sortedParams.Values)
                {
                    if (context.Request.QueryString[param.Name] is null)
                    {
                        if (param.IsOptional)
                        {
                            parameters.Add(param.DefaultValue);
                            continue;
                        }
                        else
                        {
                            context.Response.BadRequest($"Missing parameter: {param.Name}");
                            outParams = null;
                            return false;
                        }
                    }

                    if (param.ParameterType.IsEnum)
                    {
                        if (Enum.TryParse(param.ParameterType, context.Request.QueryString[param.Name], true, out var result))
                        {
                            parameters.Add(result);
                            continue;
                        }
                        else
                        {
                            context.Response.BadRequest($"'{context.Request.QueryString[param.Name]}' is not a valid value for {param.Name}");
                            outParams = null;
                            return false;
                        }
                    }

                    if (UrlValueConstraint.TryGetByTargetType(param.ParameterType, out var constraint))
                    {
                        if (constraint.TryParse(context.Request.QueryString[param.Name]!, out var result))
                        {
                            parameters.Add(result);
                            continue;
                        }
                        else
                        {
                            context.Response.BadRequest($"'{context.Request.QueryString[param.Name]}' is not a valid value for {param.Name}");
                            outParams = null;
                            return false;
                        }
                    }
                    else
                    {
                        parameters.Add(Activator.CreateInstance(param.ParameterType));
                    }
                }
            }

            outParams = parameters.ToArray();
            return true;
        }
        catch (Exception ex)
        {
            SMAPIWrapper.Instance.Log($"Error populating endpoint parameters: {ex.Message}");
        }

        outParams = null;
        return false;
    }
}