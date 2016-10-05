using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHive.Server.Core.DataModel
{
    public partial class Application
    {
        /// <summary>
        /// Short name - used in the url part to indicate an active app (in host mode)
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Full app name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A bit more verbal info on the application
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The application's entry point(s).
        /// Usually there will only be one url, but during the dev or even in production, when there are multiple envs maintained, providing a pipe (|) separated list of urls,
        /// simplifies the app setup. App urls listed here are required when an app requires authentication.
        /// </summary>
        public string Urls { get; set; }

        /// <summary>
        /// Whether or not own application's splashscreen should be used, or the host should use own load mask
        /// </summary>
        public bool UseSplashscreen { get; set; }

        /// <summary>
        /// Whether or not the application requires authentication in order to be used.
        /// </summary>
        public bool RequiresAuth { get; set; }

        /// <summary>
        /// Whether or not the information about an application can be publicly accessible
        /// a common app means every user can get the information about the application such as its name, short name, description, url, etc. without having to authenticate
        /// if an application is marked as common it may be shown in the app switcher, so user can launch it (provided the env is configured for this of course)
        /// </summary>
        public bool IsCommon { get; set; }

        /// <summary>
        /// Whether or not an app is default; when flagged as default, the app will be automatically loaded if there were no other means of enforcing the current app.
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Whether or not the app is retrievable through the usual means
        /// Note: this is an experimental property
        /// </summary>
        public bool IsHidden { get; set; }

        //TODO
        //provider
        //comments
        //tags
        //price
        //trial periods
        //etc

    }
}
