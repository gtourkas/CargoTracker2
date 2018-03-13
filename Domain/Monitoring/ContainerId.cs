namespace Domain.Monitoring
{
    public class ContainerId
    {
        public string Value { get; }

        public ContainerId(string value)
        {
            // TODO: validate

            Value = value;
        }
    }
}
