using System.Dynamic;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Lucilvio.Study.Htmx;

public class HtmxResult : ActionResult
{
    private readonly string _triggers;
    private readonly bool _isError;
    private readonly string? _errorMesssage;

    private HtmxResult(HtmxTriggersList? triggers = null, bool error = false, string? errorMessage = null)
    {
        if (triggers is null)
            this._triggers = string.Empty;
        else
            this._triggers = triggers.Value!;

        this._isError = error;
        this._errorMesssage = errorMessage;
    }

    public static HtmxResult Ok()
    {
        return new HtmxResult();
    }

    public static HtmxResult OkWithTriggers(params HtmxTrigger[] triggers)
    {
        return new HtmxResult(new HtmxTriggersList(triggers));
    }

    public static HtmxResult Error(string errorMessage)
    {
        return new HtmxResult(null, true, errorMessage);
    }

    public static HtmxResult ErrorWithTriggers(string errorMessage, params HtmxTrigger[] triggers)
    {
        return new HtmxResult(new HtmxTriggersList(triggers), true, errorMessage);
    }

    public override Task ExecuteResultAsync(ActionContext context)
    {
        if (this._triggers.Length > 0)
            context.HttpContext.Response.Headers.Append("HX-Trigger", this._triggers);

        if (this._isError)
        {
            context.HttpContext.Response.Headers.Append("X-ErrorMessage", this._errorMesssage ?? string.Empty);

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }
        else
        {
            context.HttpContext.Response.Headers.Append("HX-Trigger-After-Settle", "success");
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
        }

        return base.ExecuteResultAsync(context);
    }
}

public record HtmxTriggersList
{
    private readonly string _name;
    private readonly object? _parameters;

    public HtmxTriggersList(params HtmxTrigger[] triggers)
    {
        if (triggers.Length == 0)
            return;

        var triggerObject = new ExpandoObject();

        foreach (var trigger in triggers)
        {
            if (trigger is null)
                continue;

            if (trigger.Parameters is null)
                triggerObject.TryAdd(trigger.Name, null);
            else
                triggerObject.TryAdd(trigger.Name, trigger.Parameters);
        }

        this.Value = JsonSerializer.Serialize(triggerObject, new JsonSerializerOptions
        {
            WriteIndented = false
        });
    }

    public string? Value { get; }
}

public record HtmxTrigger
{
    public HtmxTrigger(string name, object? parameters = null)
    {
        this.Name = name;
        this.Parameters = parameters;
    }

    public string Name { get; }
    public object? Parameters { get; }
}