using Microsoft.AspNetCore.Mvc;
using ChatAppBackend.Data;
using ChatAppBackend.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly ChatContext _context;

    public ChatController(ChatContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Message>>> GetMessages()
    {
        return await _context.Messages.ToListAsync();
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
        return CreatedAtAction(nameof(GetMessages), new { id = message.Id }, message);
    }

    private string GenerateReply(string userMessage)
    {
        // Simulate LLM response
        return "This is a response from the LLM";
    }
}