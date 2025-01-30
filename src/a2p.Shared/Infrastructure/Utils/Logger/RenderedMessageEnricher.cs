using System.Text;

using Serilog.Core;
using Serilog.Events;

public class RenderedMessageEnricher : ILogEventEnricher
{
 public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
 {
  if (logEvent==null)
  {
   return;
  }

  // Render the message
  string renderedMessage = logEvent.RenderMessage();

  // Add the cleaned message as a property
  logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RenderedMessage", renderedMessage));
 }

 public static string SanitizeForJson(string input)
 {
  if (string.IsNullOrEmpty(input))
  {
   return input;
  }
  StringBuilder sanitizedString = new();
  foreach (char c in input)
  {
   _=c switch
   {
    // Double quote
    '\"' => sanitizedString.Append("\\\""),
    // Backslash
    '\\' => sanitizedString.Append("\\\\"),
    // Backspace
    '\b' => sanitizedString.Append("\\b"),
    // Formfeed
    '\f' => sanitizedString.Append("\\f"),
    // Newline
    '\n' => sanitizedString.Append("\\n"),
    // Carriage return
    '\r' => sanitizedString.Append("\\r"),
    // Horizontal tab
    '\t' => sanitizedString.Append("\\t"),
    _ => c<0x20 ? sanitizedString.Append("\\u"+((int)c).ToString("x4")) : sanitizedString.Append(c),// Add character directly if it's valid for JSON
   };
  }
  return sanitizedString.ToString();
 }
}
