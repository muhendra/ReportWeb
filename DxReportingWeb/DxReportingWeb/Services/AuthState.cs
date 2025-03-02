namespace DxReportingWeb.Services
{
    public class AuthState
    {
        public bool IsAuthenticated { get; private set; }

        public event Action OnChange;

        public void SetAuthenticated(bool isAuthenticated)
        {
            IsAuthenticated = isAuthenticated;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
