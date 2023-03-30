namespace Infra
{
    public class BrokerOptions
    {
        public string ConnectionString { get; set; } = string.Empty;

        internal static BrokerOptions FromAction(Action<BrokerOptions> confgure)
        {
            var instance = new BrokerOptions();
            confgure(instance);
            return instance;
        }
    }
}
