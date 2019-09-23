namespace EasyApi.RestEase
{
    public class HttpContextOptions
    {
        public bool IsDefaultCredentialEnabled { get; private set; }

        public HttpContextOptions EnableDefaultCredential()
        {
            IsDefaultCredentialEnabled = true;

            return this;
        }
    }
}