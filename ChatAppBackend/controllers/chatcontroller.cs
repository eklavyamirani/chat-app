using Microsoft.AspNetCore.Mvc;
using ChatAppBackend.Data;
using ChatAppBackend.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly ChatContext _context;
    private readonly ILogger<ChatController> _logger;
    private static ConcurrentBag<StreamWriter> _clients = new ConcurrentBag<StreamWriter>();

    public ChatController(ChatContext context, ILogger<ChatController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Message>>> GetMessages()
    {
        return await _context.Messages.ToListAsync();
    }

    [HttpGet("stream")]
    public async Task Stream()
    {
        Response.Headers.Add("Content-Type", "text/event-stream");

        var client = Response.BodyWriter.AsStream();
        var streamWriter = new StreamWriter(client);
        _logger.LogInformation($"adding client {_clients.Count}");
        _clients.Add(streamWriter);

        try
        {
            await Task.Delay(Timeout.Infinite, Request.HttpContext.RequestAborted);
        }
        catch (TaskCanceledException)
        {
            _logger.LogInformation($"trying to remove client {_clients.TryTake(out streamWriter)}");
            streamWriter.Dispose();
        }
    }

    [HttpPost]
    public async Task<ActionResult<Message>> PostMessage(Message message)
    {
        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
        // Simulate interaction with LLM
        var reply = new Message
        {
            User = "LLM",
            Content = GenerateReply(message.Content ?? "[NO CONTENT]"),
            Timestamp = DateTime.UtcNow
        };
        _context.Messages.Add(reply);
        await _context.SaveChangesAsync();

        // Notify all clients
        var eventString = $"data: {reply} \n\n";
        foreach (var client in _clients)
        {
            await client.WriteAsync(eventString);
            await client.FlushAsync();
        }

        return CreatedAtAction(nameof(GetMessages), new { id = message.Id }, message);
    }

    private string GenerateReply(string userMessage)
    {
        // Simulate LLM response
        return "This is a response from the LLM";
    }
}