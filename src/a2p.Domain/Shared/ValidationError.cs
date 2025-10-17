namespace a2p.Domain.Shared
{
    public class ValidationError
    {
        public string Field { get; }
        public string Message { get; }

        public ValidationError(string field, string message)
        {
            Field = field;
            Message = message;
        }

        public override string ToString() => $"{Field}: {Message}";
    }
}