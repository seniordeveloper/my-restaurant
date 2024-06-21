namespace MyRestaurant.Core.Configuration
{
    public class AppConfiguration
    {
        /// <summary>
        /// GEts or sets environment name.
        /// </summary>
        public string EnvironmentName { get; set; }

        /// <summary>
        /// Gets or sets domain url.
        /// </summary>
        public string DomainUrl { get; set; }

        /// <summary>
        /// Gets or sets a boolean flag indicating whether app is running on local machine or not.
        /// </summary>
        public bool IsLocal { get; set; }

        /// <summary>
        /// Gets or sets a boolean flag indicating whether stack trace is enabled in reponse.
        /// </summary>
        public bool EnableStackTrace { get; set; }
    }
}
