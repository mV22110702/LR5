namespace LR5.file_logger
{
    public static class FileLoggerExtensions

    {

        public static ILoggingBuilder AddFile(this ILoggingBuilder builder, string filePath)

        {

            builder.AddProvider(new FileLoggerProvider(filePath));

            return builder;

        }

    }
}
