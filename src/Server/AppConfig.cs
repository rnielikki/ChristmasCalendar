namespace joulukalenteri.Server
{
    /// <summary>
    /// Constant configuration for the app.
    /// </summary>
    public static class AppConfig
    {
        //path of the christmas calendar markdown files
        /// <remarks>
        /// The each markdown file path is contents/{Year}/{Day}.md.
        /// If the file's target day is future, it won't shown even if the file exists.
        /// </remarks>
        public const string __dirpath = "contents/";
    }
}
