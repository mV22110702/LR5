namespace LR5.file_logger
{
    [ProviderAlias("FileLogger")]
    public sealed class FileLoggerProvider : ILoggerProvider
    {
        string path;
        public FileLoggerProvider(string path)
        {
            this.path = path;
        }
        public ILogger CreateLogger(string categoryName) => new FileLogger(path);
        public void Dispose() { }

    }
}
